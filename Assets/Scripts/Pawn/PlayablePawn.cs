using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class PlayablePawn : Pawn
{
    [SerializeField] protected bool isActivePlayer;

    public Weapon equippedWeapon;

    public Dictionary<string, int> appliedUpgrades = new Dictionary<string, int>();


    //TEMP
    public int levelThreshold = 5;

    //TEMP

    public InteractableObject closestInteractable;
    public InteractableArea currentArea;
    public PlayablePawn closestPlayablePawn;
    public PlayablePawn changeToPawn;
    Timer timerClosestInteractable;
    Timer timerClosestPlayablePawn;


    float useThreshold = 1f;
    float useTimer = 0f;


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

        GameManager.current.eventService.onGivePlayerXp -= GainXp;
        GameManager.current.eventService.onGivePlayerLevel -= GainLevel;
        GameManager.current.eventService.onGivePlayerMaterial -= GainMaterials;

        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);

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
    }

    protected override void PawnPause()
    {
        base.PawnPause();

        timerClosestInteractable?.Pause(isPaused);
        timerClosestPlayablePawn?.Pause(isPaused);

        // necessary or else pawn drifts for a moment if moving while pausing
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
        if (statBlock.pawnSprite != null) _sr.sprite = statBlock.pawnSprite;
        _sr.color = statBlock.pawnSpriteColor;
    }

    protected override void FirstStatApplication()
    {
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

        GameManager.current.eventService.RequestUIUpdateHealth(lifepoints, maxLifepoints);
        GameManager.current.eventService.RequestUIUpdateXpBar(xp, levelThreshold);
        GameManager.current.eventService.RequestUIUpdateLevelCounter(level);
    }


    public void Move(float xInput, float yInput)
    {
        yInput *= 1.4f;

        Vector3 movement = new Vector3(xInput, 0f, yInput);
        Vector3 moveDest = transform.position + (movement / 2);

        _nav.destination = moveDest;
    }

    public override void GetHit(float damage, bool isCrit, float knockback = 0, Vector3? knockbackPush = null)
    {
        base.GetHit(damage, isCrit, knockback, knockbackPush);

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

    #region XP
    public void GainXp(float v)
    {
        totalXp += v;
        xp += v;
        while (xp >= levelThreshold)
        {
            xp -= levelThreshold;
            GainLevel(1);
        }

        statBlock.totalXp.SetValues(totalXp, 0f, 1f);
        statBlock.xp.SetValues(xp, 0f, 1f);
        statBlock.level.SetValues(level, 0f, 1f);

        GameManager.current.eventService.RequestUIUpdateXpBar(xp, levelThreshold);
    }

    public void GainLevel(int l)
    {
        level++;
        GameManager.current.eventService.RequestUIUpdateLevelCounter(level);
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
            equippedWeapon.baseStatBlock = statBlock.equippedWeapon;
            equippedWeapon.FirstStatApplication();

            if (equippedWeapon != null) Destroy(currentWeapon);
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
        timerClosestInteractable.Pause(true);
        timerClosestPlayablePawn.Pause(true);

        closestInteractable?.Use();
        closestPlayablePawn?.Use();
        currentArea?.Use();
    }

    public void InteractReset()
    {
        timerClosestInteractable.Pause(false);
        timerClosestPlayablePawn.Pause(false);

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
        } else GameManager.current.eventService.RequestUIUpdateInteractText("", false);
    }

    public void FindClosestPlayablePawn()
    {
        string intText = "";
        closestPlayablePawn = GameManager.current.pawnService.GetClosestPlayablePawn(transform.position, interactionRange);
        if (closestPlayablePawn != null)
        {
            intText = $"Change to {closestPlayablePawn.statBlock.pawnName}";
            GameManager.current.eventService.RequestUIUpdateInteractText(intText, true);
        } else GameManager.current.eventService.RequestUIUpdateInteractText("", false);
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

    public void SetInactivePlayer(PawnStatBlock pawnStatBlock = null)
    {
        isActivePlayer = false;
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
