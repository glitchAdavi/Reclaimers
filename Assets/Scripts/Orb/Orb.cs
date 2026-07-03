using UnityEngine;
using Reclaimers.Systems;
using Reclaimers.UpdateSystem;
using Reclaimers.DeveloperConsole;

public class Orb : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] protected ExternalCollider _extColl;
    [SerializeField] protected SpriteRenderer _sr;

    [SerializeField] protected Pawn target;
    [SerializeField] protected bool active = false;
    [SerializeField] protected bool triggered = false;
    [SerializeField] protected float gracePeriod = 0.15f;
    [SerializeField] protected int decay = 60;
    protected float speed = 0f;

    protected bool isPaused = false;
    protected bool isVisible = false;

    Timer gracePeriodTimer;
    Timer decayTimer;

    protected virtual void OnEnable()
    {
        this.Register();

        _extColl.onTriggerEnter += ManageOnTriggerEnter;

        decay = 60;
        decayTimer = GameManager.Instance.timerSystem.SetTimer(decay, Decay, 1f, () => decay--, "ORB_Decay");

        if (gracePeriod > 0) gracePeriodTimer = GameManager.Instance.timerSystem.SetTimer(gracePeriod, () =>
        {
            active = true;
            gracePeriodTimer = null;
        }, "ORB_GracePeriod");
    }

    public virtual void ManagedUpdate()
    {
        if (isPaused) return;

        if (!triggered) UpdateVisibility();

        if (triggered && target != null) Move();
    }

    public virtual void ManagedPause(bool paused)
    {
        isPaused = paused;

        gracePeriodTimer?.Pause(isPaused);
        decayTimer?.Pause(isPaused);
    }

    protected virtual void OnDisable()
    {
        active = false;
        triggered = false;
        target = null;

        decayTimer?.Cancel();
        decayTimer = null;
        gracePeriodTimer?.Cancel();
        gracePeriodTimer = null;

        this.Unregister();
    }

    protected virtual void Move()
    {
        speed = target.statBlock.speed.Value() * 2f;
        Vector3 thisPos = new Vector3(transform.position.x, 0.25f, transform.position.z);
        Vector3 targetPos = new Vector3(target.transform.position.x, 0.25f, target.transform.position.z);
        transform.position = Vector3.MoveTowards(thisPos, targetPos, speed * Time.deltaTime);
    }

    public virtual void Trigger(Pawn target)
    {
        if (!active) return;

        decayTimer?.Cancel();
        decayTimer = null;
        this.target = target;
        triggered = true;
    }

    protected virtual void ManageOnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Effect();
            Die();
        }
    }

    protected virtual void Effect() { }

    protected virtual void Die()
    {
        
    }

    protected virtual void Decay()
    {
        Die();
    }

    public virtual void ForceDecay()
    {
        Decay();
    }

    protected virtual void UpdateVisibility()
    {
        if (GameManager.Instance.IsVisibleToPlayerCamera(transform.position)) isVisible = true;
        else isVisible = false;

        decayTimer?.Pause(isVisible);
    }
}
