using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PlayablePawn : Pawn
{
    [SerializeField] protected bool isActivePlayer;

    public GameObject weaponStretcher;
    public SpriteRenderer weaponSprite;
    public Weapon equippedWeapon;
    public Ability equippedAbility;
    public Light muzzleFlash;
    Timer timerMuzzleFlash;

    public Dictionary<string, int> appliedUpgrades = new Dictionary<string, int>();

    public int levelThreshold = 10;

    public int pendingLevelUps = 0;

    public InteractableObject closestInteractable;
    public InteractableArea currentArea;
    public PlayablePawn closestPlayablePawn;
    public PlayablePawn changeToPawn;
    Timer timerClosestInteractable;
    Timer timerClosestPlayablePawn;

    bool isMoving = false;


    float useThreshold = 1f;
    float useTimer = 0f;

    public bool isAlive = true;


    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected void InitializeInactivePlayer(PawnStatBlock pawnStatBlock = null)
    {
        timerClosestInteractable?.Cancel();
        timerClosestPlayablePawn?.Cancel();

        if (pawnStatBlock != null) baseStatBlock = pawnStatBlock;
        else if (baseStatBlock == null) baseStatBlock = GameManager.current.gameInfo.defaultStatBlock;

        statBlock = ScriptableObject.CreateInstance<PawnStatBlock>();
        statBlock.CopyValues(baseStatBlock);

        BaseStatApplication();

        Destroy(equippedWeapon);
        Destroy(equippedAbility);

        weaponSprite.sprite = statBlock.equippedWeapon.weaponSprite;

        GameManager.current.eventService.onGivePlayerXp -= GainXp;
        GameManager.current.eventService.onGivePlayerLevel -= GainLevel;
        GameManager.current.eventService.onGivePlayerMaterial -= GainMaterials;

        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);

        _anm.enabled = false;
        _anmColor.enabled = false;

        changeToPawn = null;
    }

    protected void InitializeActivePlayer(PawnStatBlock currentPlayerStatBlock = null)
    {
        if (currentPlayerStatBlock != null) baseStatBlock = GameManager.current.gameInfo.currentPlayerStatBlock;
        else if (baseStatBlock == null) baseStatBlock = GameManager.current.gameInfo.defaultStatBlock;

        statBlock = ScriptableObject.CreateInstance<PawnStatBlock>();
        statBlock.CopyValues(baseStatBlock);

        FirstStatApplication();

        ApplyWeapon();
        ApplyAbility();
        
        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);

        GameManager.current.eventService.onGivePlayerXp += GainXp;
        GameManager.current.eventService.onGivePlayerLevel += GainLevel;
        GameManager.current.eventService.onGivePlayerMaterial += GainMaterials;

        timerClosestInteractable = GameManager.current.timerService.StartTimer(3600f, null, 0.2f, FindClosestInteractable);
        if (level < 1) timerClosestPlayablePawn = GameManager.current.timerService.StartTimer(3600f, null, 0.2f, FindClosestPlayablePawn);

        changeToPawn = null;
    }

    protected override void PawnUpdate()
    {
        base.PawnUpdate();

        if (isActivePlayer) UpdateSprite();

        if (pendingLevelUps > 0)
        {
            pendingLevelUps--;
            GameManager.current.eventService.QueueLevelUp();
        }
    }

    protected override void PawnPause()
    {
        base.PawnPause();

        timerClosestInteractable?.Pause(isPaused);
        timerClosestPlayablePawn?.Pause(isPaused);

        // necessary or else pawn drifts for a moment if pausing while moving
        _nav.velocity = Vector3.zero;

        if (isPaused) _nav.isStopped = true;
        else _nav.isStopped = false;
    }

    protected override void OnDisable()
    {
        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }

    protected void BaseStatApplication()
    {
        if (statBlock.pawnMainSprite != null) _sr.sprite = statBlock.pawnMainSprite;
        if (statBlock.pawnColorSprite != null) _srColor.sprite = statBlock.pawnColorSprite;
        _sr.color = statBlock.pawnMainSpriteColor;
        _srColor.color = statBlock.pawnColorSpriteColor;
    }

    protected override void FirstStatApplication(bool useScaling = false)
    {
        BaseStatApplication();
        base.FirstStatApplication();
        ApplyLpRegen();
        ApplyLpRegenDelay();
        ApplyLpRegenTickTime();
        ApplyHealingMultiplier();
        ApplyIFrameDuration();
        ApplyXp();
        ApplyXpGain();
        ApplyLevel();
        ApplyPickUpRange();
        ApplyInteractionRange();
        ApplyMaterials();
        ApplyMaterialGain();
        ApplyUpgradeDictionary();

        GameManager.current.eventService.RequestUIUpdateHealth(lifepoints, maxLifepoints);
        GameManager.current.eventService.RequestUIUpdateXpBar(xp, levelThreshold);
        GameManager.current.eventService.RequestUIUpdateLevelCounter(level);
    }

    public void ApplyUpgradeDictionary()
    {
        if (statBlock.keyUpgrade == null || statBlock.keyUpgrade.Count < 1) return;

        for (int i = 0; i < statBlock.keyUpgrade.Count; i++)
        {
            appliedUpgrades.Add(statBlock.keyUpgrade[i], statBlock.valueUpgrade[i]);
        }
    }

    public void SaveUpgradeDictionary()
    {
        statBlock.keyUpgrade.Clear();
        statBlock.valueUpgrade.Clear();

        if (appliedUpgrades == null || appliedUpgrades.Count < 1) return;

        foreach (KeyValuePair<string, int> kvp in appliedUpgrades)
        {
            statBlock.keyUpgrade.Add(kvp.Key);
            statBlock.valueUpgrade.Add(kvp.Value);
        }
    }


    public void Move(float xInput, float yInput)
    {
        if (xInput != 0 || yInput != 0) isMoving = true;
        else isMoving = false;

        yInput *= 1.4f;

        Vector3 movement = new Vector3(xInput, 0f, yInput);
        Vector3 moveDest = transform.position + (movement / 2);

        _nav.destination = moveDest;
    }

    public override void GetHit(float damage, bool isCrit, float knockback = 0, Vector3? knockbackPush = null)
    {
        base.GetHit(damage, isCrit, knockback, knockbackPush);

        GameManager.current.audioService.PlaySound(GameManager.current.gameInfo.acPlayerHurt, _as);
        GameManager.current.eventService.RequestUIUpdateHealth(lifepoints, maxLifepoints);
    }

    public override void Heal(float value)
    {
        base.Heal(value);

        GameManager.current.eventService.RequestUISpawnFloatingText(transform.position,
                                                                    $"<size=4>+{value}",
                                                                    Color.green,
                                                                    0f,
                                                                    0.5f);
        GameManager.current.eventService.RequestUIUpdateHealth(lifepoints, maxLifepoints);
    }

    protected override void Die()
    {
        isAlive = false;
        GameManager.current.eventService.PlayerDeath();
    }

    #region XP
    public void GainXp(float v)
    {
        totalXp += (v * xpGain);
        xp += (v * xpGain);
        while (xp >= levelThreshold)
        {
            xp -= levelThreshold;
            LevelThreshold();
            GainLevel(1);
        }

        statBlock.totalXp.SetValues(totalXp, 0f, 1f);
        statBlock.xp.SetValues(xp, 0f, 1f);
        statBlock.level.SetValues(level, 0f, 1f);

        GameManager.current.eventService.RequestUIUpdateXpBar(xp, levelThreshold);
    }

    public void GainLevel(int l, bool noLevelUp = false)
    {
        level += l;
        if (!noLevelUp) LevelUpEffect(l);
        GameManager.current.eventService.RequestUIUpdateLevelCounter(level);
    }

    protected void LevelUpEffect(int l)
    {
        pendingLevelUps += l;
    }

    protected void LevelThreshold()
    {
        levelThreshold += 10 + (2 * (level - 1));
    }
    #endregion

    #region Materials
    public void GainMaterials(float mats)
    {
        materials += mats * materialGain;

        statBlock.materials.SetValues(materials, 0f, 1f);
    }


    #endregion

    #region Weapon
    public WeaponStatBlock ChangeWeapon(WeaponStatBlock newWeapon)
    {
        WeaponStatBlock currentWeapon = statBlock.equippedWeapon;
        statBlock.equippedWeapon = newWeapon;
        ApplyWeapon();
        return currentWeapon;
    }

    private void ApplyWeapon()
    {
        if (statBlock.equippedWeapon != null)
        {
            Weapon currentWeapon = equippedWeapon;

            Type wType = Type.GetType(statBlock.equippedWeapon.weaponType);
            equippedWeapon = gameObject.AddComponent(wType) as Weapon;
            equippedWeapon.owner = this;
            equippedWeapon.baseStatBlock = statBlock.equippedWeapon;
            equippedWeapon.FirstStatApplication();

            if (equippedWeapon != null) Destroy(currentWeapon);
        } else
        {
            if (equippedWeapon != null) Destroy(equippedWeapon);
            GameManager.current.eventService.RequestUIWeaponShow(false);
        }
    }

    public void MuzzleFlash()
    {
        muzzleFlash.enabled = true;
        timerMuzzleFlash = GameManager.current.timerService.StartTimer(0.05f, () => muzzleFlash.enabled = false);
    }

    #endregion

    #region Ability
    public AbilityStatBlock ChangeAbility(AbilityStatBlock newAbility)
    {
        AbilityStatBlock currentAbility = statBlock.equippedAbility;
        statBlock.equippedAbility = newAbility;
        ApplyAbility();
        return currentAbility;
    }

    private void ApplyAbility()
    {
        if (statBlock.equippedAbility != null)
        {
            Ability currentAbility = equippedAbility;

            Type aType = Type.GetType(statBlock.equippedAbility.abilityType);
            equippedAbility = gameObject.AddComponent(aType) as Ability;
            equippedAbility.owner = this;
            equippedAbility.baseStatBlock = statBlock.equippedAbility;
            equippedAbility.FirstStatApplication();

            if (equippedAbility != null) Destroy(currentAbility);
        }
        else
        {
            if (equippedAbility != null) Destroy(equippedAbility);
            GameManager.current.eventService.RequestUIAbilityShow(false);
        }
    }
    #endregion

    #region Upgrades
    public void AddUpgrade(Upgrade u)
    {
        if (appliedUpgrades.ContainsKey(u.name))
        {
            appliedUpgrades[u.name]++;
        } else
        {
            appliedUpgrades[u.name] = 1;
        }

        u.Apply(this);
    }

    public void RemoveUpgrade(Upgrade u)
    {
        if (appliedUpgrades.ContainsKey(u.name) && appliedUpgrades[u.name] > 0)
        {
            appliedUpgrades[u.name]--;
            if (appliedUpgrades[u.name] < 1)
            {
                appliedUpgrades.Remove(u.name);
            }

            u.Remove(this);
        }
    }

    public Upgrade GetUpgradeByName(string uName)
    {
        Upgrade result = null;

        foreach (PawnUpgrade pu in GameManager.current.allPawnUpgrades)
        {
            if (pu.name.Equals(uName)) result = pu;
        }

        foreach (WeaponUpgrade wu in GameManager.current.allWeaponUpgrades)
        {
            if (wu.name.Equals(uName)) result = wu;
        }

        return result;
    }

    #endregion

    #region Interaction
    public void Interact()
    {
        timerClosestInteractable?.Pause(true);
        timerClosestPlayablePawn?.Pause(true);

        closestInteractable?.Use();
        closestPlayablePawn?.Use();
        currentArea?.Use();
    }

    public void InteractReset()
    {
        timerClosestInteractable?.Pause(false);
        timerClosestPlayablePawn?.Pause(false);

        closestInteractable?.UseReset();
        closestPlayablePawn?.UseReset();
        currentArea?.UseReset();
    }

    public void FindClosestInteractable()
    {
        if (GameManager.current.levelService.IsPlayerInsideAnInteractableArea()) return;

        string intText = "";
        closestInteractable = GameManager.current.levelService.GetClosestInteractable(transform.position, interactionRange);
        if (closestInteractable != null)
        {
            intText = closestInteractable.IOVerb + closestInteractable.IOName;
            GameManager.current.eventService.RequestUIUpdateInteractText(intText, true);
        }
        else if (closestPlayablePawn == null) GameManager.current.eventService.RequestUIUpdateInteractText("", false);
    }

    public void FindClosestPlayablePawn()
    {
        string intText = "";
        closestPlayablePawn = GameManager.current.pawnService.GetClosestPlayablePawn(transform.position, interactionRange);
        if (closestPlayablePawn != null)
        {
            intText = $"Change to {closestPlayablePawn.statBlock.pawnName}";
            GameManager.current.eventService.RequestUIUpdateInteractText(intText, true);
        }
        else if (currentArea == null && closestInteractable == null) GameManager.current.eventService.RequestUIUpdateInteractText("", false);
    }

    public virtual void Use()
    {
        useTimer += Time.deltaTime;
        GameManager.current.eventService.RequestUIUpdateInteractFill(useTimer, useThreshold, true);
        if (useTimer >= useThreshold) OnFinish();
    }

    public virtual void UseReset()
    {
        useTimer = 0;
        GameManager.current.eventService.RequestUIUpdateInteractFill(0f, 0f, false);
    }

    protected virtual void OnFinish()
    {
        GameManager.current.eventService.RequestUIUpdateInteractFill(0f, 0f, false);
        GameManager.current.eventService.RequestUIUpdateInteractText("", false);
        useTimer = 0;
        OnFinishEffect();
    }

    protected virtual void OnFinishEffect()
    {
        GameManager.current.SetNewActivePlayer(this);
    }
    #endregion

    #region Sprite
    private void UpdateSprite()
    {
        float posX = Input.mousePosition.x - (Screen.width / 2);
        float posY = Input.mousePosition.y - (Screen.height / 2);

        if (!isMoving)
        {
            _sr.sprite = statBlock.pawnMainSprite;
            _srColor.sprite = statBlock.pawnColorSprite;
        }
        _anm.enabled = isMoving;
        _anmColor.enabled = isMoving;

        if (posX > 0)
        {
            _sr.flipX = true;
            _srColor.flipX = true;
            weaponSprite.flipX = false;
            weaponStretcher.transform.localPosition = new Vector3(0.15f, 0f, -0.2f);
            if (Physics.Raycast(GameManager.current.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out var info, 100f, 1 << 20))
            {
                Vector3 dir = info.point - transform.position;
                weaponSprite.transform.rotation = Quaternion.LookRotation(Vector3.up, dir) * Quaternion.AngleAxis(90, Vector3.forward);
            }

        } else
        {
            _sr.flipX = false;
            _srColor.flipX = false;
            weaponSprite.flipX = true;
            weaponStretcher.transform.localPosition = new Vector3(-0.15f, 0f, -0.2f);
            if (Physics.Raycast(GameManager.current.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition), out var info, 100f, 1 << 20))
            {
                Vector3 dir = info.point - transform.position;
                weaponSprite.transform.rotation = Quaternion.LookRotation(Vector3.up, -dir) * Quaternion.AngleAxis(90, Vector3.forward);
            }
        }

    }




    #endregion


    public void SetInactivePlayer(PawnStatBlock pawnStatBlock = null)
    {
        isActivePlayer = false;
        if (pawnStatBlock != null) pawnStatBlock.equippedWeapon = GameManager.current.GetRandomWeaponStatBlock();
        InitializeInactivePlayer(pawnStatBlock);
    }

    public void SetActivePlayer(PawnStatBlock currentPlayerStatBlock = null)
    {
        isActivePlayer = true;
        InitializeActivePlayer(currentPlayerStatBlock);
    }

    public bool IsActivePlayer()
    {
        return isActivePlayer;
    }
}
