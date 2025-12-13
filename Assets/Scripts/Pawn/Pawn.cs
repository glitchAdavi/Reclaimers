using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pawn : MonoBehaviour, IUpdate, IFixedUpdate, ILateUpdate, IPause
{
    public PawnStatBlock baseStatBlock;
    public PawnStatBlock statBlock;
    [SerializeField] bool destroyBlockOnDisable = true;

    [SerializeField] protected List<Modifier> currentModifiers = new List<Modifier>();

    [SerializeField] protected NavMeshAgent _nav;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected SpriteRenderer _sr;
    [SerializeField] protected SpriteRenderer _srColor;
    [SerializeField] protected DamageCollider _damageCollider;
    [SerializeField] protected Animator _anm;
    [SerializeField] protected Animator _anmColor;
    [SerializeField] protected AudioSource _as;

    [SerializeField] protected float maxLifepoints = 1f;
    [SerializeField] protected float lifepoints = 1f;

    [SerializeField] protected float lpRegen = 0f;
    [SerializeField] protected float lpRegenDelay = 1f;
    [SerializeField] protected float lpRegenTickTime = 1f;
    Timer lpRegenDelayTimer;
    Timer lpRegenTickTimer;

    [SerializeField] protected float damageMultiplier = 1f;
    [SerializeField] protected float healingMultiplier = 1f;

    [SerializeField] protected float meleeDamage = 0f;
    [SerializeField] protected float meleeCooldown = 0f;

    [SerializeField] protected float pickUpRange = 0f;
    [SerializeField] protected float interactionRange = 0f;

    [SerializeField] protected float iFrameDuration = 0f;
    Timer iFrameTimer;
    protected bool hasIFrames = false;

    [SerializeField] protected float knockBackResist = 0f;

    [SerializeField] protected bool isPaused = false;
    [SerializeField] protected bool isDead = false;

    [SerializeField] protected float totalXp = 0f;
    [SerializeField] protected float xp = 0f;
    [SerializeField] protected float xpGain = 0f;
    [SerializeField] protected int level = 0;
    [SerializeField] protected float xpKillValue = 0f;

    [SerializeField] protected float materials = 0f;
    [SerializeField] protected float materialGain = 0f;

    Timer knockbackTimer;
    Timer hitEffectTimer;


    protected virtual void OnEnable()
    {
        _nav.updateRotation = false;
        _rb.isKinematic = true;        
    }

    #region InterfaceUpdates
    public void ExecuteUpdate()
    {
        if (isPaused || isDead) return;

        PawnUpdate();
    }

    public void ExecuteFixedUpdate()
    {
        if (isPaused || isDead) return;

        PawnFixedUpdate();
    }

    public void ExecuteLateUpdate()
    {
        if (isPaused || isDead) return;

        PawnLateUpdate();
    }

    public void Pause(bool paused)
    {
        isPaused = paused;

        PawnPause();
    }
    #endregion

    protected virtual void PawnUpdate() { }
    protected virtual void PawnFixedUpdate() { }
    protected virtual void PawnLateUpdate() { }
    protected virtual void PawnPause()
    {
        lpRegenDelayTimer?.Pause(isPaused);
        lpRegenTickTimer?.Pause(isPaused);
        iFrameTimer?.Pause(isPaused);
        knockbackTimer?.Pause(isPaused);
        hitEffectTimer?.Pause(isPaused);
    }

    protected virtual void OnDisable()
    {
        if (destroyBlockOnDisable) Destroy(statBlock);
    }




    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Teleport(Vector3 pos)
    {
        _nav.Warp(pos);
    }

    public virtual void GetHit(float damage, bool isCrit, float knockback = 0, Vector3? knockbackPush = null)
    {
        if (hasIFrames) return;

        if (lpRegenDelayTimer != null) lpRegenDelayTimer.Cancel();
        if (lpRegen > 0) lpRegenDelayTimer = GameManager.current.timerService.StartTimer(lpRegenDelay, StartLpRegen);

        if (lpRegenTickTimer != null) lpRegenTickTimer.Cancel();

        lifepoints -= damage * damageMultiplier;
        if (lifepoints <= 0)
        {
            lifepoints = 0;
            Die();
        } else
        {
            HitEffect();
            Knockback(knockback, knockbackPush);
        }
        if (lifepoints > maxLifepoints) lifepoints = maxLifepoints;

        hasIFrames = true;
        iFrameTimer = GameManager.current.timerService.StartTimer(iFrameDuration, () => hasIFrames = false);
    }

    protected virtual void HitEffect() { }

    protected virtual void Knockback(float knockback, Vector3? knockbackPush = null)
    {
        if (knockback <= 0 || !knockbackPush.HasValue) return;

        _nav.enabled = false;
        _rb.isKinematic = false;
        _rb.AddForce(((Vector3)knockbackPush).normalized * knockback * (1 - knockBackResist), ForceMode.Impulse);

        knockbackTimer = GameManager.current.timerService.StartTimer(0.1f, () =>
        {
            _rb.isKinematic = false;
            _nav.enabled = true;
            _rb.linearVelocity = Vector3.zero;
            _rb.isKinematic = true;
        });
    }

    #region LpRegen
    public virtual void StartLpRegen()
    {
        lpRegenTickTimer = GameManager.current.timerService.StartTimer(3600f, null, lpRegenTickTime, LpRegen);
    }

    public virtual void LpRegen()
    {
        Heal(lpRegen);
    }
    #endregion

    public virtual void Heal(float value)
    {
        lifepoints += value * healingMultiplier;
        if (lifepoints > maxLifepoints) lifepoints = maxLifepoints;
    }

    protected virtual void Die()
    {
        Debug.Log("I should be dead");
    }

    public Pawn GetPawnFromCollision(Collision collision)
    {
        return collision.gameObject.transform.parent.GetComponentInChildren<Pawn>();
    }

    #region Modifiers
    public Modifier GetModifier(string id)
    {
        foreach (Modifier mod in currentModifiers)
        {
            if (mod.id.Equals(id)) return mod;
        }
        return null;
    }

    public void AddModifier(Modifier newModifier)
    {
        Modifier existing = GetModifier(newModifier.id);
        if (existing != null)
        {
            existing.Reset();
            return;
        }

        Modifier newMod = Instantiate(newModifier);
        currentModifiers.Add(newMod);
        newMod.ApplyModifier(this);
    }

    public void RemoveModifier(string id)
    {
        currentModifiers.Remove(GetModifier(id));
    }

    #endregion




    #region ApplyValues
    protected virtual void FirstStatApplication(bool useScaling = false)
    {
        ApplySprite();
        ApplyColorSprite();
        ApplyController();
        ApplyScale();
        ApplySpeed();
        ApplyLifepoints(useScaling);
        ApplyDamageMultiplier();
        lifepoints = maxLifepoints;
    }

    public void ApplySprite()
    {
        if (statBlock.pawnMainSprite != null)
        {
            _sr.sprite = statBlock.pawnMainSprite;
            _anm.enabled = true;
        }
    }

    public void ApplyColorSprite()
    {
        if (statBlock.pawnColorSprite != null)
        {
            _srColor.sprite = statBlock.pawnColorSprite;
            _anmColor.enabled = true;
        }
    }

    public void ApplyController()
    {
        if (statBlock.controller != null) _anm.runtimeAnimatorController = statBlock.controller;
    }

    public void ApplyScale()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * statBlock.scale.Value();
    }

    public virtual void ApplySpeed(bool useScaling = false)
    {
        _nav.speed = statBlock.speed.Value();
        _anm.speed = 0.1f * statBlock.speed.Value();
        if (_anmColor != null) _anmColor.speed = 0.1f * statBlock.speed.Value();
        if (useScaling)
        {
            _nav.speed *= GameManager.current.Scaling();
            _anm.speed *= GameManager.current.Scaling();
            if (_anmColor != null) _anmColor.speed *= GameManager.current.Scaling();
        }
    }

    public void ApplyLifepoints(bool useScaling = false)
    {
        maxLifepoints = statBlock.lifepoints.Value();
        if (useScaling) maxLifepoints *= GameManager.current.Scaling();
        if (this is PlayablePawn) GameManager.current.eventService.RequestUIUpdateHealth(lifepoints, maxLifepoints);
    }

    public void ApplyLpRegen()
    {
        lpRegen = statBlock.lpRegen.Value();
    }

    public void ApplyLpRegenDelay()
    {
        lpRegenDelay = statBlock.lpRegenDelay.Value();
    }

    public void ApplyLpRegenTickTime()
    {
        lpRegenTickTime = statBlock.lpRegenTickTime.Value();
    }

    public void ApplyDamageMultiplier()
    {
        damageMultiplier = statBlock.damageMultiplier.Value();
    }

    public void ApplyHealingMultiplier()
    {
        healingMultiplier = statBlock.healingMultiplier.Value();
    }

    public void ApplyMeleeDamage(bool useScaling = false)
    {
        meleeDamage = statBlock.meleeDamage.Value();
        if (useScaling) meleeDamage *= GameManager.current.Scaling();
    }

    public void ApplyMeleeCooldown()
    {
        meleeCooldown = statBlock.meleeCooldown.Value();
    }

    public void ApplyIFrameDuration()
    {
        iFrameDuration = statBlock.iFrameDuration.Value();
    }

    public void ApplyKnockbackResist()
    {
        knockBackResist = statBlock.knockBackResist.Value();
    }

    public void ApplyXpKillValue(bool useScaling = false)
    {
        xpKillValue = statBlock.xpKillValue.Value();
        if (useScaling) xpKillValue *= GameManager.current.Scaling();
    }

    public void ApplyTotalXp()
    {
        totalXp = statBlock.totalXp.Value();
    }

    public void ApplyXp()
    {
        xp = statBlock.xp.Value();
    }

    public void ApplyXpGain()
    {
        xpGain = statBlock.xpGain.Value();
    }

    public void ApplyLevel()
    {
        level = statBlock.level.ValueInt();
    }

    public void ApplyPickUpRange()
    {
        pickUpRange = statBlock.pickUpRange.Value();
    }

    public void ApplyInteractionRange()
    {
        interactionRange = statBlock.interactionRange.Value();
    }

    public void ApplyMaterials()
    {
        materials = statBlock.materials.Value();
    }

    public void ApplyMaterialGain()
    {
        materialGain = statBlock.materialGain.Value();
    }
    #endregion

    #region GetValues
    public bool IsPawnDead()
    {
        return isDead;
    }

    public int GetLevel()
    {
        return level;
    }

    public float GetSpeed()
    {
        return _nav.speed;
    }

    public float GetCurrentLifepoints()
    {
        return lifepoints;
    }

    public float GetPickUpRange()
    {
        return pickUpRange;
    }

    public float GetNavRadius()
    {
        return _nav.radius;
    }
    #endregion

}
