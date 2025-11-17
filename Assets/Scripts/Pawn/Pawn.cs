using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Pawn : MonoBehaviour, IUpdate, IFixedUpdate, ILateUpdate, IPause
{
    public PawnStatBlock defaultStatBlock;
    public PawnStatBlock statBlock;
    [SerializeField] bool destroyBlockOnDisable = true;

    [SerializeField] protected NavMeshAgent _nav;
    [SerializeField] protected Rigidbody _rb;
    [SerializeField] protected GameObject _sprite;
    [SerializeField] protected DamageCollider _damageCollider;

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


    [SerializeField] protected bool isPaused = false;
    [SerializeField] protected bool isDead = false;

    [SerializeField] protected float totalXp = 0f;
    [SerializeField] protected float xp = 0f;
    [SerializeField] protected float xpGain = 0f;
    [SerializeField] protected int level = 0;
    [SerializeField] protected float xpKillValue = 0f;

    Timer knockbackTimer;
    Timer hitEffectTimer;


    protected virtual void OnEnable()
    {
        _nav.updateRotation = false;
        _rb.isKinematic = true;

        // eventually replace with player saved statblock
        if (defaultStatBlock == null) defaultStatBlock = GameManager.current.gameInfo.defaultStatBlock;
        statBlock = ScriptableObject.CreateInstance<PawnStatBlock>();
        statBlock.CopyValues(defaultStatBlock);

        
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

        Debug.Log(gameObject.name);

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
        HitEffect();
        Knockback(knockback, knockbackPush);
        if (lifepoints <= 0)
        {
            lifepoints = 0;
            Die();
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
        _rb.AddForce(((Vector3)knockbackPush).normalized * knockback, ForceMode.Impulse);

        knockbackTimer = GameManager.current.timerService.StartTimer(0.1f, () =>
        {
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

    #region ApplyValues
    protected virtual void FirstStatApplication()
    {
        ApplyScale();
        ApplySpeed();
        ApplyLifepoints();
        ApplyDamageMultiplier();
        lifepoints = maxLifepoints;
    }

    public void ApplyScale()
    {
        gameObject.transform.localScale = new Vector3(1f, 1f, 1f) * statBlock.scale.Value();
    }

    public void ApplySpeed()
    {
        _nav.speed = statBlock.speed.Value();
    }

    public void ApplyLifepoints()
    {
        maxLifepoints = statBlock.lifepoints.Value();
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

    public void ApplyMeleeDamage()
    {
        meleeDamage = statBlock.meleeDamage.Value();
    }

    public void ApplyMeleeCooldown()
    {
        meleeCooldown = statBlock.meleeCooldown.Value();
    }

    public void ApplyIFrameDuration()
    {
        iFrameDuration = statBlock.iFrameDuration.Value();
    }

    public void ApplyXpKillValue()
    {
        xpKillValue = statBlock.xpKillValue.Value();
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
    #endregion

    #region GetValues
    public float GetSpeed()
    {
        return _nav.speed;
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
