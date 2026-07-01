using Reclaimers;
using Reclaimers.DeveloperConsole;
using Reclaimers.Systems;
using Reclaimers.UpdateSystem;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class Pawn : MonoBehaviour, IUpdateable, IFixedUpdateable, ILateUpdateable, IPauseable
{
    public StatBlock statBlock;
    public WeaponBlock weaponBlock;
    public AbilityBlock abilityBlock;

    public ProjectileBuilder projectileBuilder;
    public GameObject projParent;

    [SerializeField] private NavMeshAgent _nav;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _col;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private SpriteRenderer _shadowSpr;
    [SerializeField] private SpriteRenderer _statusEffectSprite;
    [SerializeField] private Weapon _weapon;

    public Sprite frame1;
    public Sprite frame2;

    public float currentHp = 0f;

    private int p_levelThreshold = 10;
    private int p_pendingLevelUps = 0;
    private float np_navUpdateInterval = 0.25f;

    [SerializeField] private List<Pawn> p_nearbyEnemies = new List<Pawn>();
    [SerializeField] private List<Interactable> p_nearbyInteractables = new List<Interactable>();
    [SerializeField] private List<StatusEffect> activeStatusEffects = new List<StatusEffect>();

    public bool isActive = false;
    public bool isDead = false;
    public bool isPaused = false;
    public bool isPlayer = false;
    public bool isMoving = false;
    public bool isInvulnerable = false;
    public bool isMeleeAvailable = false;
    public bool isStunned = false;
    public bool isFrame1 = true;
    //public bool isLookingRight = false;

    private Timer p_iFrameTimer;
    private Timer p_hpRegenStartTimer;
    private Timer p_hpRegenTickTimer;
    private Timer meleeCooldownTimer;
    private Timer stunTimer;
    private Timer np_navUpdateTimer;
    private Timer damageEffectTimer;
    private Timer animationTimer;

    public Pawn target = null;

    Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, 0.25f, 0f));

    public void SetAsPlayer(StatBlock toClone)
    {
        SetupCleanup();

        isPlayer = true;
        _nav.avoidancePriority = 1;
        SetTarget(null);

        EventSystem.Instance.onGivePlayerXp += GainXp;
        EventSystem.Instance.onGivePlayerLevel += GainLevel;
        EventSystem.Instance.onGivePlayerMoney += GainMoney;

        if (_weapon == null)
        {
            if (statBlock.weaponBlock == null) weaponBlock = GameManager.Instance.gameInfo.defaultWeaponBlock;
            _weapon = gameObject.AddComponent<Weapon>();
            _weapon.SetOwner(this);
        }

        projParent = new GameObject();
        projParent.name = "ProjectileParent";
        projectileBuilder = GameManager.Instance.CreateSystem<ProjectileBuilder>();
        projectileBuilder.BuildPool(ProjectileFactory, 50, projParent.transform);

        _col.gameObject.layer = 10;

        Setup(toClone);
    }

    public void SetAsNonPlayer(StatBlock toClone, Pawn target = null)
    {
        SetupCleanup();

        isPlayer = false;
        _nav.avoidancePriority = 50;
        SetTarget(target);

        _col.gameObject.layer = 11;

        Setup(toClone);
    }

    public void Setup(StatBlock toClone)
    {
        isActive = false;
        isDead = false;
        isInvulnerable = false;
        isMeleeAvailable = true;

        _nav.updateRotation = false;

        statBlock = toClone.Clone() as StatBlock;

        SetSprites();

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
        EventSystem.Instance.onGivePlayerXp -= GainXp;
        EventSystem.Instance.onGivePlayerLevel -= GainLevel;
        EventSystem.Instance.onGivePlayerMoney -= GainMoney;
    }

    private void OnEnable()
    {
        this.Register();
    }

    private void OnDisable()
    {
        CancelTimer(ref np_navUpdateTimer);

        EventSystem.Instance.onGivePlayerXp -= GainXp;
        EventSystem.Instance.onGivePlayerLevel -= GainLevel;

        this.Unregister();
    }

    public void ManagedUpdate()
    {
        if (isPaused) return;

        if (isPlayer)
        {
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
                finalPos = target.GetPosition();

                Vector2 targetPos = GameManager.Instance.playerCamera.GetComponent<Camera>().WorldToScreenPoint(finalPos);
                Vector2 currentPos = GameManager.Instance.playerCamera.GetComponent<Camera>().WorldToScreenPoint(transform.position);

                //if (targetPos.x >= myPos.x) _sr.material.SetFloat("_FlipX", 0);
                //else _sr.material.SetFloat("_FlipX", 1);

                if (targetPos.x >= currentPos.x) _sprite.flipX = false;
                else _sprite.flipX = true;
            }
            else finalPos = transform.position;

            isMoving = !_nav.isStopped;
        }

        if (isMoving)
        {
            if (animationTimer == null) animationTimer = GameManager.Instance.timerSystem.SetForeverTimer(0.25f, MoveAnimation);
        } else
        {
            CancelTimer(ref animationTimer);

            _sprite.sprite = frame1;
            isFrame1 = true;
        }


        _nav.destination = finalPos;
    }

    public void MoveAnimation()
    {
        if (isFrame1)
        {
            if (frame2 != null) _sprite.sprite = frame2;
            isFrame1 = false;
        } else
        {
            if (frame1 != null) _sprite.sprite = frame1;
            isFrame1 = true;
        }
    }

    public void UpdateSpriteFlip()
    {
        float posX = Input.mousePosition.x - (Screen.width / 2);

        if (posX > 0)
        {
            _sprite.flipX = false;
        } else
        {
            _sprite.flipX = true;
        }
    }


    public void GetHit(float value, bool isCrit = false, float stunDur = 0)
    {
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
                p_iFrameTimer = GameManager.Instance.timerSystem.SetTimer(statBlock.iFrameDuration.Value(), () => isInvulnerable = false);

                p_hpRegenStartTimer = GameManager.Instance.timerSystem.SetTimer(statBlock.hpRegenDelay.Value(), StartHpRegen);
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
            if (!isCrit) EventSystem.Instance.SpawnFloatingText(transform.position, totalDamage.ToString("#.##"), Color.red, 2);
            else EventSystem.Instance.SpawnFloatingText(transform.position, totalDamage.ToString("#.##!"), Color.red, 2, 8);
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
        _sprite.color = Color.red;
        damageEffectTimer = GameManager.Instance.timerSystem.SetTimer(0.1f, () => _sprite.color = Color.white);
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
        _sprite.color = Color.green;
        damageEffectTimer = GameManager.Instance.timerSystem.SetTimer(0.1f, () => _sprite.color = Color.white);
    }

    public void StartHpRegen()
    {
        p_hpRegenTickTimer = GameManager.Instance.timerSystem.SetForeverTimer(statBlock.hpRegenTickTime.Value(), () => Heal(statBlock.hpRegen.Value()));
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
        });
    }

    public void Melee()
    {
        isMeleeAvailable = false;
        target.GetHit(-statBlock.meleeDamage.Value(), false);
        meleeCooldownTimer = GameManager.Instance.timerSystem.SetTimer(statBlock.meleeCooldown.Value(), () => isMeleeAvailable = true);
    }


    public void Die()
    {
        CancelTimer(ref animationTimer);
        CancelTimer(ref damageEffectTimer);

        if (isPlayer)
        {
            CancelTimer(ref p_iFrameTimer);
            CancelTimer(ref p_hpRegenStartTimer);
            CancelTimer(ref p_hpRegenTickTimer);



        } else
        {
            CancelTimer(ref np_navUpdateTimer);
            CancelTimer(ref meleeCooldownTimer);
            CancelTimer(ref stunTimer);

            if (statBlock.xpInstant) EventSystem.Instance.GivePlayerXp(statBlock.xpKillValue.Value());
            else EventSystem.Instance.SpawnXp(transform.position, statBlock.xpKillValue.Value());

            if (statBlock.moneyInstant) EventSystem.Instance.GivePlayerMoney(statBlock.moneyKillValue.Value());
            else EventSystem.Instance.SpawnMoney(transform.position, statBlock.moneyKillValue.Value());

            EventSystem.Instance.EnemyDeath(this);
        }
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
    }

    public void GainLevel(int l, bool silent = false)
    {
        if (isDead || !isPlayer) return;

        statBlock.level.ModB(l);
        if (!silent) LevelUpEffect(l);

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

    #region Interact
    public void Interact()
    {

    }

    public void InteractReset()
    {

    }



    #endregion

    #region StatusEffect
    public StatusEffect GetStatusEffect(string id)
    {
        foreach (StatusEffect se in activeStatusEffects)
        {
            if (se.id.Equals(id)) return se;
        }
        return null;
    }

    public void AddStatusEffect(StatusEffect newStatusEffect)
    {
        StatusEffect activeSE = GetStatusEffect(newStatusEffect.id);
        if (activeSE != null)
        {
            activeSE.ResetStatusEffect();
            return;
        }

        StatusEffect newSE = Instantiate(newStatusEffect);
        activeStatusEffects.Add(newSE);
        SetStatusEffectSprite();
        newSE.ApplyStatusEffect(this);
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

        _statusEffectSprite.sprite = SetSprite(seSprites.ToArray(), new Color[seSprites.Count - 1]);
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
        this.target = target;
        CancelTimer(ref np_navUpdateTimer);
        if (this.target == null) return;
        np_navUpdateTimer = GameManager.Instance.timerSystem.SetForeverTimer(np_navUpdateInterval, () => Move());
    }

    public void ChangeNavUpdateInterval(float newInterval)
    {
        np_navUpdateInterval = newInterval;
        CancelTimer(ref np_navUpdateTimer);
        np_navUpdateTimer = GameManager.Instance.timerSystem.SetForeverTimer(np_navUpdateInterval, () => Move());
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

    public void SetSprites()
    {
        Sprite[] sprFrame1 = new Sprite[7];
        Sprite[] sprFrame2 = new Sprite[7];

        for (int i = 0; i < statBlock.sprNames.Length; i++)
        {
            if (string.IsNullOrEmpty(statBlock.sprNames[i])) continue;
            SpritePart part = GameManager.Instance.GetSpritePart(statBlock.sprNames[i]);
            if (part != null)
            {
                sprFrame1[i] = part.frame1;
                sprFrame2[i] = part.frame2;
            }
        }

        Sprite aux1 = SetSprite(sprFrame1, statBlock.sprFrameColor);
        Sprite aux2 = SetSprite(sprFrame2, statBlock.sprFrameColor);

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
        Type aux = Type.GetType(weaponBlock.projType.ToString());

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

    public static void TurnOn(Pawn p)
    {
        p.gameObject.SetActive(true);
    }

    public static void TurnOff(Pawn p)
    {
        p.gameObject.SetActive(false);
    }
    #endregion
}
