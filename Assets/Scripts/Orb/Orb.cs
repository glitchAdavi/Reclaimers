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

    Timer timerGracePeriod;
    Timer timerDecay;

    protected virtual void OnEnable()
    {
        this.Register();

        _extColl.onTriggerEnter += ManageOnTriggerEnter;

        decay = 60;
        timerDecay = GameManager.Instance.timerSystem.SetTimer(decay, Decay, 1f, () => decay--);

        if (gracePeriod > 0) timerGracePeriod = GameManager.Instance.timerSystem.SetTimer(gracePeriod, () =>
        {
            active = true;
            timerGracePeriod = null;
        });
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

        if (timerGracePeriod != null) timerGracePeriod.Pause(paused);
    }

    protected virtual void OnDisable()
    {
        if (timerDecay) timerDecay.Cancel();
        if (timerGracePeriod) timerGracePeriod.Cancel();

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

        if (timerDecay) timerDecay.Cancel();
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
        Destroy(gameObject);
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

        if (timerDecay) timerDecay.Pause(isVisible);
    }
}
