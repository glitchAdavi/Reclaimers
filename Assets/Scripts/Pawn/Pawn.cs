using Reclaimers.DeveloperConsole;
using Reclaimers.Systems;
using Reclaimers.UpdateSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pawn : MonoBehaviour, IUpdateable, IFixedUpdateable, ILateUpdateable, IPauseable
{
    public StatBlock statBlock;

    public ObjectBuilder<Projectile> projectileBuilder;
    public GameObject projParent;

    [SerializeField] private NavMeshAgent _nav;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _col;
    [SerializeField] private MaterialPropertyBlock _matBlock;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _shadowSpr;
    [SerializeField] private SpriteRenderer _statusEffectSprite;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Ability _ability;

    public Sprite frame1;
    public Sprite frame2;

    public int spriteVariableHueMult = 0;

    public float currentHp = 0f;

    [SerializeField] private int p_levelThreshold = 10;
    [SerializeField] private int p_pendingLevelUps = 0;
    private float np_navUpdateInterval = 0.25f;
    private float np_dissolveTime = 0.25f;
    private float p_dissolveTime = 3f;

    [SerializeField] private List<Pawn> p_nearbyEnemies = new List<Pawn>();
    [SerializeField] private List<Interactable> p_nearbyInteractables = new List<Interactable>();
    [SerializeField] private List<StatusEffect> activeStatusEffects = new List<StatusEffect>();

    public bool isActive = false;
    public bool isIdle = false;
    public bool isDead = false;
    public bool isPaused = false;
    public bool isPlayer = false;
    public bool isMoving = false;
    public bool isInvulnerable = false;
    public bool isMeleeAvailable = false;
    public bool isStunned = false;
    public bool isFrame1 = true;
    public bool isTeleportable = true;

    private Timer p_iFrameTimer;
    private Timer p_hpRegenStartTimer;
    private Timer p_hpRegenTickTimer;
    private Timer meleeCooldownTimer;
    private Timer stunTimer;
    private Timer np_navUpdateTimer;
    private Timer damageEffectTimer;
    private Timer animationTimer;
    private Timer np_dissolveTimer;
    private Timer p_dissolveTimer;

    public Pawn target = null;

    Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, 0.25f, 0f));

    public void SetAsPlayer(StatBlock toClone)
    {
        statBlock = toClone.Clone() as StatBlock;

        SetupCleanup();

        isPlayer = true;
        _nav.avoidancePriority = 1;
        SetTarget(null);

        GameManager.Instance.eventSystem.onGivePlayerXp += GainXp;
        GameManager.Instance.eventSystem.onGivePlayerLevel += GainLevel;
        GameManager.Instance.eventSystem.onGivePlayerMoney += GainMoney;

        if (_weapon == null)
        {
            if (statBlock.weaponBlock == null)
            {
                statBlock.weaponBlock = GameManager.Instance.gameInfo.defaultWeaponBlock;
                
            }
            Utilities.CopyValues(statBlock.weaponBlock, statBlock, typeof(WeaponVar));

            _weapon = gameObject.AddComponent<Weapon>();
            _weapon.SetOwner(this);
        }

        if (_ability == null)
        {
            if (statBlock.abilityBlock == null)
            {
                statBlock.abilityBlock = GameManager.Instance.gameInfo.defaultAbilityBlock;
            }
            Utilities.CopyValues(statBlock.abilityBlock, statBlock, typeof(AbilityVar));

            _ability = gameObject.AddComponent(Type.GetType(statBlock.abilityScript.ToString())) as Ability;
            _ability.SetOwner(this);
        }

        projParent = new GameObject();
        projParent.name = "ProjectileParent";
        projectileBuilder = new ObjectBuilder<Projectile>();
        projectileBuilder.BuildPool(ProjectileFactory, 50, projParent.transform);

        statBlock.hitSprite = GameManager.Instance.gameInfo.hitPlayer;

        _col.gameObject.layer = 10;

        LevelThreshold();

        GameManager.Instance.eventSystem.UpdateHp(statBlock.hp.Value(), statBlock.hp.Value());
        GameManager.Instance.eventSystem.UpdateXp(statBlock.xpCur.Value(), p_levelThreshold);
        GameManager.Instance.eventSystem.UpdateLevel(statBlock.level.ValueInt());

        Setup();
    }

    public void SetAsNonPlayer(StatBlock toClone, Pawn target = null)
    {
        statBlock = toClone.Clone() as StatBlock;

        SetupCleanup();

        isPlayer = false;
        _nav.enabled = true;
        _nav.avoidancePriority = 50;
        _nav.isStopped = false;
        SetTarget(target);

        statBlock.hitSprite = GameManager.Instance.gameInfo.hitEnemy;

        _col.gameObject.layer = 11;

        if (_weapon == null && statBlock.weaponBlock != null)
        {
            Utilities.CopyValues(statBlock.weaponBlock, statBlock, typeof(WeaponVar));
            _weapon = gameObject.AddComponent<Weapon>();
            _weapon.SetOwner(this);
            _weapon.SetTargetLayer(10);

            projParent = new GameObject();
            projParent.name = "ProjectileParentNP";
            projectileBuilder = new ObjectBuilder<Projectile>();
            projectileBuilder.BuildPool(ProjectileFactory, 10, projParent.transform);
        }

        statBlock.sprNames = GameManager.Instance.GetRandomSpriteParts(statBlock.sprCat);

        Setup();
    }

    public void Setup()
    {
        isActive = true;
        isDead = false;
        isInvulnerable = false;
        isMeleeAvailable = true;

        _col.enabled = true;

        _nav.updateRotation = false;

        SetSprites();
        SetScale();

        _matBlock.SetFloat("_AlphaClip", 0.01f);
        if (statBlock.spriteVariableHue)
        {
            spriteVariableHueMult = UnityEngine.Random.Range(-1, 2);
            _matBlock.SetFloat("_HueOffset", statBlock.spriteVariableHueRange * spriteVariableHueMult);
        }
        _sprite.SetPropertyBlock(_matBlock);
        _shadowSpr.SetPropertyBlock(_matBlock);

        currentHp = statBlock.hp.Value();

    }

    private void SetupCleanup()
    {
        if (projectileBuilder != null)
        {
            projectileBuilder.DestroyBuilder();
            projectileBuilder = null;
            Destroy(projParent);
        }
        if (_weapon != null)
        {
            Destroy(_weapon);
            _weapon = null;
        }
        if (_ability != null)
        {
            Destroy(_ability);
            _ability = null;
        }
        GameManager.Instance.eventSystem.onGivePlayerXp -= GainXp;
        GameManager.Instance.eventSystem.onGivePlayerLevel -= GainLevel;
        GameManager.Instance.eventSystem.onGivePlayerMoney -= GainMoney;
    }

    private void Awake()
    {
        _matBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        this.Register();
    }

    private void OnDisable()
    {
        CancelTimer(ref p_iFrameTimer);
        CancelTimer(ref p_hpRegenStartTimer);
        CancelTimer(ref p_hpRegenTickTimer);
        CancelTimer(ref np_navUpdateTimer);
        CancelTimer(ref meleeCooldownTimer);
        CancelTimer(ref stunTimer);
        CancelTimer(ref damageEffectTimer);
        CancelTimer(ref animationTimer);

        GameManager.Instance.eventSystem.onGivePlayerXp -= GainXp;
        GameManager.Instance.eventSystem.onGivePlayerLevel -= GainLevel;

        this.Unregister();
    }

    public void ManagedUpdate()
    {
        if (isPaused) return;

        if (isPlayer)
        {
            if (p_pendingLevelUps > 0)
            {
                p_pendingLevelUps--;
                GameManager.Instance.eventSystem.QueueLevelUp();
            }

            GameManager.Instance.gameInfo.playerPositionVar.SetValue(transform.position);

            UpdateSpriteFlip();
            WeaponAim();
            DetectionCast();

        } else
        {
            if (isMeleeAvailable && CheckIfTargetIsInRange(_nav.radius + target.GetNavRadius() + statBlock.meleeRange.Value())) Melee();
        }
        
        

    }

    public void ManagedFixedUpdate()
    {
        if (isPaused) return;

    }

    public void ManagedLateUpdate()
    {
        if (isPaused) return;

    }

    public void ManagedPause(bool paused)
    {
        isPaused = paused;

        if (isPlayer)
        {
            p_iFrameTimer?.Pause(isPaused);
            p_hpRegenStartTimer?.Pause(isPaused);
            p_hpRegenTickTimer?.Pause(isPaused);

        } else {
            _nav.enabled = !isPaused;

            np_navUpdateTimer?.Pause(isPaused);
            stunTimer?.Pause(isPaused);
            damageEffectTimer?.Pause(isPaused);
            meleeCooldownTimer?.Pause(isPaused);
        }

        animationTimer?.Pause(isPaused);
    }

    public void Move(float xInput = 0f, float yInput = 0f)
    {
        if (!_nav.isActiveAndEnabled) return;

        _nav.speed = statBlock.speed.Value();
        Vector3 finalPos = Vector3.zero;

        if (isPlayer)
        {
            yInput *= 1.4f;

            Vector3 movement = new Vector3(xInput, 0f, yInput);
            finalPos = transform.position + (movement / 2);

            isMoving = (movement != Vector3.zero) ? true : false;
        } else
        {
            if (target != null)
            {
                if (isTeleportable && GameManager.Instance.pawnSystem.IsOutsidePlayerRange(transform.position))
                {
                    ResetSelf();
                    return;
                }

                finalPos = target.GetPosition();

                Vector2 targetPos = GameManager.Instance.playerCamera.GetComponent<Camera>().WorldToScreenPoint(finalPos);
                Vector2 currentPos = GameManager.Instance.playerCamera.GetComponent<Camera>().WorldToScreenPoint(transform.position);

                if (targetPos.x >= currentPos.x)
                {
                    _sprite.flipX = false;
                    _shadowSpr.flipX = false;
                }
                else
                {
                    _sprite.flipX = true;
                    _shadowSpr.flipX = true;
                }
            }
            else finalPos = transform.position;

            isMoving = !_nav.isStopped;
        }

        if (isMoving)
        {
            if (animationTimer == null) animationTimer = GameManager.Instance.timerSystem.SetForeverTimer(0.25f, MoveAnimation, "PAWN_MoveAnimation");
        }
        else
        {
            CancelTimer(ref animationTimer);

            _sprite.sprite = frame1;
            isFrame1 = true;
        }

        _nav.destination = finalPos;
    }

    public void MoveAnimation()
    {
        if (statBlock.spriteVariableHue) _matBlock.SetFloat("_HueOffset", statBlock.spriteVariableHueRange * spriteVariableHueMult);
        _sprite.SetPropertyBlock(_matBlock);

        if (isFrame1)
        {
            if (frame2 != null)
            {
                _sprite.sprite = frame2;
                _shadowSpr.sprite = frame2;
            }
            isFrame1 = false;
        } else
        {
            if (frame1 != null)
            {
                _sprite.sprite = frame1;
                _shadowSpr.sprite = frame1;
            }
            isFrame1 = true;
        }
    }

    public void UpdateSpriteFlip()
    {
        float posX = Input.mousePosition.x - (Screen.width / 2);
        float posY = Input.mousePosition.y - (Screen.height / 2);

        if (posX > 0)
        {
            _sprite.flipX = false;
            _shadowSpr.flipX = false;
        } else
        {
            _sprite.flipX = true;
            _shadowSpr.flipX = true;
        }
    }


    public void GetHit(float value, bool isCrit = false, float stunDur = 0)
    {
        if (isDead) return;

        if (value < 0)
        {
            if (isInvulnerable) return;

            Damage(value, isCrit);
            Stun(stunDur);

            if (isPlayer)
            {
                CancelTimer(ref p_hpRegenStartTimer);
                CancelTimer(ref p_hpRegenTickTimer);

                isInvulnerable = true;
                p_iFrameTimer = GameManager.Instance.timerSystem.SetTimer(statBlock.iFrameDuration.Value(), () => isInvulnerable = false, "PAWN_P_iFrame");

                p_hpRegenStartTimer = GameManager.Instance.timerSystem.SetTimer(statBlock.hpRegenDelay.Value(), StartHpRegen, "PAWN_P_HpRegenStart");
            }
        }
        else if (value > 0)
        {
            Heal(value);
        }
        else
        {
            LogSystem.Log("0 damage");
        }
    }

    public void Damage(float damageVal, bool isCrit = false)
    {
        float totalDamage = (-damageVal) * statBlock.damageModifier.Value();

        if (!isPlayer)
        {
            if (!isCrit) GameManager.Instance.eventSystem.SpawnFloatingText(transform.position, totalDamage.ToString("#.##"), null, Color.red);
            else GameManager.Instance.eventSystem.SpawnFloatingText(transform.position, totalDamage.ToString("#.##!"), null, Color.red, 2, 1.5f);
        }

        if (currentHp <= totalDamage)
        {
            currentHp = 0;
            Die();
            return;
        } else
        {
            currentHp -= totalDamage;
        }
        DamageEffect();
    }

    public void DamageEffect()
    {
        if (isPlayer) GameManager.Instance.eventSystem.UpdateHp(currentHp, statBlock.hp.Value());

        _sprite.color = Color.red;
        damageEffectTimer = GameManager.Instance.timerSystem.SetTimer(0.1f, () => _sprite.color = Color.white, "PAWN_DamageEffect");
    }

    public void Heal(float healVal)
    {
        float totalHealing = healVal * statBlock.healingModifier.Value();
        if (statBlock.hp.Value() - currentHp < totalHealing)
        {
            CancelTimer(ref p_hpRegenStartTimer);
            CancelTimer(ref p_hpRegenTickTimer);
            currentHp = statBlock.hp.Value();
        } else
        {
            currentHp += totalHealing;
        }
        HealEffect();
    }

    public void HealEffect()
    {
        if (isPlayer) GameManager.Instance.eventSystem.UpdateHp(currentHp, statBlock.hp.Value());

        _sprite.color = Color.green;
        damageEffectTimer = GameManager.Instance.timerSystem.SetTimer(0.1f, () => _sprite.color = Color.white, "PAWN_HealEffect");
    }

    public void StartHpRegen()
    {
        p_hpRegenTickTimer = GameManager.Instance.timerSystem.SetForeverTimer(statBlock.hpRegenTickTime.Value(), () => Heal(statBlock.hpRegen.Value()), "PAWN_P_HpRegenTick");
    }

    public void Stun(float stunDur)
    {
        if (isStunned) return;

        float tempSpeedMult = statBlock.speed.Mult;

        isStunned = true;
        statBlock.speed.SetMult(tempSpeedMult * 0.25f);
        _nav.speed = statBlock.speed.Value();

        stunTimer = GameManager.Instance.timerSystem.SetTimer(stunDur, () =>
        {
            statBlock.speed.SetMult(tempSpeedMult);
            _nav.speed = statBlock.speed.Value();
            isStunned = false;
        }, "PAWN_Stun");
    }

    public void Melee()
    {
        isMeleeAvailable = false;
        if (_weapon == null)
        {
            target.GetHit(-statBlock.meleeDamage.Value(), false);
        } else
        {
            Vector3 overrideForward = GameManager.Instance.playerPawn.transform.position - transform.position;
            _weapon.Shoot(new Vector3(overrideForward.x, 0.25f, overrideForward.z));
        }
        meleeCooldownTimer = GameManager.Instance.timerSystem.SetTimer(statBlock.meleeCooldown.Value(), () => isMeleeAvailable = true, "PAWN_MeleeCooldown");
    }


    public void Die()
    {
        isActive = false;
        isDead = true;

        _col.enabled = false;

        CancelTimer(ref animationTimer);
        CancelTimer(ref damageEffectTimer);

        if (isPlayer)
        {
            CancelTimer(ref p_iFrameTimer);
            CancelTimer(ref p_hpRegenStartTimer);
            CancelTimer(ref p_hpRegenTickTimer);

            GameManager.Instance.eventSystem.SetPause(true);
            GameManager.Instance.eventSystem.EnableAllControls(false);

            float dissolveStep = 0.05f;
            p_dissolveTimer = GameManager.Instance.timerSystem.SetTimer(p_dissolveTime, GameManager.Instance.eventSystem.PlayerDeath, dissolveStep, () =>
            {
                _matBlock.SetFloat("_AlphaClip", _matBlock.GetFloat("_AlphaClip") + (dissolveStep / p_dissolveTime));
                _sprite.SetPropertyBlock(_matBlock);
                _shadowSpr.SetPropertyBlock(_matBlock);
            }, "PAWN_Player_DeathDissolve", false, false);
        } else
        {
            CancelTimer(ref np_navUpdateTimer);
            CancelTimer(ref meleeCooldownTimer);
            CancelTimer(ref stunTimer);

            _nav.isStopped = true;
            _nav.enabled = false;

            if (statBlock.xpInstant) GameManager.Instance.eventSystem.GivePlayerXp(statBlock.xpKillValue.Value());
            else GameManager.Instance.eventSystem.SpawnXp(transform.position, statBlock.xpKillValue.Value());

            if (statBlock.moneyInstant) GameManager.Instance.eventSystem.GivePlayerMoney(statBlock.moneyKillValue.Value());
            else GameManager.Instance.eventSystem.SpawnMoney(transform.position, statBlock.moneyKillValue.Value());

            float dissolveStep = 0.05f;
            np_dissolveTimer = GameManager.Instance.timerSystem.SetTimer(np_dissolveTime, ConfirmDeath, dissolveStep, () =>
            {
                _matBlock.SetFloat("_AlphaClip", _matBlock.GetFloat("_AlphaClip") + (dissolveStep / np_dissolveTime));
                _sprite.SetPropertyBlock(_matBlock);
                _shadowSpr.SetPropertyBlock(_matBlock);
            }, "PAWN_Player_DeathDissolve");
        }

    }

    public void ConfirmDeath()
    {
        GameManager.Instance.eventSystem.EnemyDeath(this);
    }

    #region XP
    public void GainXp(float v)
    {
        if (isDead || !isPlayer) return;

        statBlock.xpTotal.ModB(v * statBlock.xpGain.Value());
        statBlock.xpCur.ModB(v * statBlock.xpGain.Value());

        while (statBlock.xpCur.Value() >= p_levelThreshold)
        {
            statBlock.xpCur.ModB(-p_levelThreshold);
            LevelThreshold();
            GainLevel(1);
        }

        GameManager.Instance.eventSystem.UpdateXp(statBlock.xpCur.Value(), p_levelThreshold);
    }

    public void GainLevel(int l, bool silent = false)
    {
        if (isDead || !isPlayer) return;

        statBlock.level.ModB(l);
        if (!silent) LevelUpEffect(l);

        GameManager.Instance.eventSystem.UpdateLevel(statBlock.level.ValueInt());
    }

    protected void LevelUpEffect(int l)
    {
        p_pendingLevelUps += l;
    }

    protected void LevelThreshold()
    {
        p_levelThreshold += 10 + (2 * (statBlock.level.ValueInt() - 1));
    }
    #endregion

    #region Money
    public void GainMoney(float v)
    {
        statBlock.money.ModB(v * statBlock.moneyGain.Value());
    }

    public void RemoveMoney(float v)
    {
        statBlock.money.ModB(-(v * statBlock.moneyGain.Value()));
    }
    #endregion

    #region Weapon
    public void WeaponAim()
    {
        Vector3 tempForward = Vector3.zero;
        Ray ray = GameManager.Instance.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (aimPlane.Raycast(ray, out float enterDistance))
        {
            tempForward = ray.GetPoint(enterDistance) - transform.position;


        }
    }

    public void WeaponFire()
    {
        if (_weapon == null) return;

        _weapon.Shoot();
    }

    public void WeaponReload()
    {
        if (_weapon == null) return;

        _weapon.Reload();
    }
    #endregion

    #region Ability
    public void UseAbility()
    {
        _ability?.AbilityUse();
    }
    #endregion

    #region Interact
    public void Interact()
    {

    }

    public void InteractReset()
    {

    }
    #endregion

    public void AddModifier(Modifier m)
    {
        if (m is StatusEffect) AddStatusEffect((StatusEffect)m);
        if (m is Upgrade) AddUpgrade((Upgrade)m);
        if (m is Item) return;
    }

    #region Upgrade
    public void AddUpgrade(Upgrade u)
    {
        int index = statBlock.upgrades.FindIndex(su => su.modId == u.modId);
        if (index > -1) statBlock.upgrades[index].modStacks += (int)u.quality;
        else statBlock.upgrades.Add(new StatUpgrade(u.modId, 1));
        u.Apply(this);
    }

    #endregion

    #region Items
    public void AddItem(Item i)
    {

    }



    #endregion

    #region StatusEffect
    public StatusEffect GetStatusEffect(string id)
    {
        foreach (StatusEffect se in activeStatusEffects)
        {
            if (se.modId.Equals(id)) return se;
        }
        return null;
    }

    public void AddStatusEffect(StatusEffect newStatusEffect)
    {
        StatusEffect activeSE = GetStatusEffect(newStatusEffect.modId);
        if (activeSE != null)
        {
            activeSE.ResetStatusEffect();
            return;
        }

        StatusEffect newSE = Instantiate(newStatusEffect);
        activeStatusEffects.Add(newSE);
        SetStatusEffectSprite();
        newSE.Apply(this);
    }

    public void RemoveStatusEffect(string id)
    {
        activeStatusEffects.Remove(GetStatusEffect(id));
        SetStatusEffectSprite();
    }

    public void SetStatusEffectSprite()
    {
        if (activeStatusEffects.Count == 0)
        {
            _statusEffectSprite = null;
            return;
        }

        List<Sprite> seSprites = new List<Sprite>();

        foreach (StatusEffect se in activeStatusEffects)
        {
            if (se.statusEffectSprite != null) seSprites.Add(se.statusEffectSprite);
        }

        if (seSprites.Count > 0) _statusEffectSprite.sprite = SetSprite(seSprites.ToArray(), new Color[seSprites.Count - 1]);
    }
    #endregion


    #region Detection
    private void DetectionCast()
    {
        p_nearbyEnemies.Clear();

        int mask = (1 << 11) | (1 << 13) | (1 << 14); // 11 - Enemy | 13 - Orb | 14 - Interactable
        Collider[] detected = Physics.OverlapSphere(transform.position, statBlock.pickUpRange.Value(), mask);

        foreach (Collider c in detected)
        {
            switch (c.gameObject.layer)
            {
                case 11:
                    p_nearbyEnemies.Add(c.gameObject.GetComponent<Pawn>());
                    break;
                case 13:
                    c.gameObject.GetComponentInParent<Orb>().Trigger(this);
                    break;
                case 14:
                    p_nearbyInteractables.Add(c.gameObject.GetComponent<Interactable>());
                    break;
                default:
                    break;
            }
        }
    }



    #endregion

    #region Utilities
    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public float GetNavRadius()
    {
        return _nav.radius;
    }

    public void SetTarget(Pawn target)
    {
        CancelTimer(ref np_navUpdateTimer);

        if (target == null)
        {
            isIdle = true;
            return;
        }

        this.target = target;
        np_navUpdateTimer = GameManager.Instance.timerSystem.SetForeverTimer(np_navUpdateInterval, () => Move(), "PAWN_NP_NavUpdateTimer");
    }

    public void ChangeNavUpdateInterval(float newInterval)
    {
        np_navUpdateInterval = newInterval;
        CancelTimer(ref np_navUpdateTimer);
        np_navUpdateTimer = GameManager.Instance.timerSystem.SetForeverTimer(np_navUpdateInterval, () => Move(), "PAWN_NP_NavUpdateTimer");
    }

    public void CancelTimer(ref Timer t)
    {
        if (t == null) return;
        t.Cancel();
        t = null;
    }

    public bool CheckIfTargetIsInRange(float range)
    {
        float maxRangeSqr = range * range;
        Vector3 dir = target.GetPosition() - transform.position;
        return dir.sqrMagnitude <= maxRangeSqr;
    }

    public void SetScale()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * statBlock.scale.Value();
    }

    public void SetSprites()
    {
        Sprite[] sprFrame1 = new Sprite[8];
        Sprite[] sprFrame2 = new Sprite[8];
        Color[] sprFrameColors = new Color[8];

        for (int i = 0; i < statBlock.sprNames.Length; i++)
        {
            if (string.IsNullOrEmpty(statBlock.sprNames[i])) continue;
            int j = i * 2;
            SpritePart part = GameManager.Instance.GetSpritePart(statBlock.sprNames[i]);
            if (part != null)
            {

                sprFrame1[j] = part.frame1;
                sprFrame2[j] = part.frame2;
                sprFrameColors[j] = part.defaultColor;

                if (part.frame1Det != null) sprFrame1[j + 1] = part.frame1Det;
                if (part.frame2Det != null) sprFrame2[j + 1] = part.frame2Det;
                sprFrameColors[j + 1] = part.detDefaultColor;
            }
        }

        Sprite aux1 = SetSprite(sprFrame1, sprFrameColors);
        Sprite aux2 = SetSprite(sprFrame2, sprFrameColors);

        if (aux1 != null)
        {
            frame1 = aux1;
            _sprite.sprite = frame1;
            _shadowSpr.sprite = frame1;
        }
        else LogSystem.Log($"{(isPlayer ? "Player" : "Pawn")}: frame 1 empty.");

        if (aux2 != null) frame2 = aux2;
        else LogSystem.Log($"{(isPlayer ? "Player" : "Pawn")}: frame 2 empty.");
    }

    public Sprite SetSprite(Sprite[] sprFrame, Color[] sprFrameColor)
    {
        List<Texture2D> allSprites = new List<Texture2D>();

        for (int i = 0; i < sprFrame.Length; i++)
        {
            if (sprFrame[i] != null)
            {
                if (sprFrameColor[i] != null) allSprites.Add(CreateTexture2DFromSprite(sprFrame[i], sprFrameColor[i]));
                else allSprites.Add(CreateTexture2DFromSprite(sprFrame[i], Color.white));
            }
        }

        if (allSprites.Count == 0) return null;

        Texture2D merged = new Texture2D(32, 32, TextureFormat.RGBA32, false);
        merged.filterMode = FilterMode.Point;

        Color[] clear = new Color[32 * 32];

        for (int i = 0; i < clear.Length; i++)
        {
            clear[i] = Color.clear;
        }

        merged.SetPixels(clear);

        foreach (Texture2D spr in allSprites)
        {
            Color[] pixels = spr.GetPixels(0);

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    int index = (y * 32) + x;
                    if (pixels[index].a > 0) merged.SetPixel(x, y, pixels[index]);
                }
            }
        }

        merged.Apply();

        Sprite mergedSprite = Sprite.Create(merged, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
        

        return mergedSprite;
    }

    private Texture2D CreateTexture2DFromSprite(Sprite toCopy, Color colorToApply)
    {
        if (toCopy.textureRect.width == 0 || toCopy.textureRect.height == 0) return null;

        Texture2D result = new Texture2D((int)toCopy.textureRect.width, (int)toCopy.textureRect.height, TextureFormat.RGBA32, false);
        result.filterMode = FilterMode.Point;

        Color[] pixels = toCopy.texture.GetPixels((int)toCopy.textureRect.x, (int)toCopy.textureRect.y, (int)toCopy.textureRect.width, (int)toCopy.textureRect.height, 0);

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i].r *= colorToApply.r;
            pixels[i].g *= colorToApply.g;
            pixels[i].b *= colorToApply.b;
        }

        result.SetPixels(pixels);
        result.Apply();

        return result;
    }

    public Projectile ProjectileFactory(Transform parent = null)
    {
        Type aux = Type.GetType(statBlock.projType.ToString());

        GameObject newProjectile = Instantiate(GameManager.Instance.gameInfo.projectilePrefab, new Vector3(0f, 0.25f, 0f), Quaternion.identity, parent);
        newProjectile.AddComponent(aux);

        return newProjectile.GetComponent(aux) as Projectile;
    }

    public bool IsCrit()
    {
        if (statBlock.critChance.Value() >= UnityEngine.Random.Range(0f, 100f)) return true;
        return false;
    }

    public void Teleport(Vector3 pos)
    {
        _nav.Warp(pos);
    }

    private void ResetSelf()
    {
        GameManager.Instance.pawnSystem.ResetEnemy(transform.position - target.transform.position, this);
    }

    public bool HasPendingLevelUps()
    {
        return p_pendingLevelUps > 0;
    }
    #endregion
}
