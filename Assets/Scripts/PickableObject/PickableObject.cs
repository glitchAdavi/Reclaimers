using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickableObject : MonoBehaviour, IUpdate, IFixedUpdate, IPause
{
    [SerializeField] protected DamageCollider _dmgColl;
    [SerializeField] protected SpriteRenderer _sr;

    protected Pawn target;
    protected bool canBeActive = false;
    protected bool active = false;
    [SerializeField] protected float graceTime = 0.15f;
    protected float speed = 0f;

    protected bool isPaused = false;

    Timer gracePeriodTimer;

    private void OnEnable()
    {
        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterFixedUpdate(this);
        GameManager.current.updateService.RegisterPause(this);

        _dmgColl.onTriggerEnter += ManageTriggerEnter;

        if (graceTime > 0) EnableGracePeriod();
        if (target == null) target = GameManager.current.playerPawn;
    }

    public void ExecuteUpdate()
    {
        if (isPaused) return;

        if (canBeActive)
        {
            if (target == null) target = GameManager.current.playerPawn;
            if (Vector3.Distance(transform.position, GameManager.current.playerPawn.transform.position) < GameManager.current.playerPawn.GetPickUpRange())
            {
                Activate();
            }
        }

        if (active && target != null) Move();
    }

    public void ExecuteFixedUpdate()
    {
        if (isPaused) return;

    }

    public void Pause(bool paused)
    {
        isPaused = paused;
        if (gracePeriodTimer != null) gracePeriodTimer.Pause(paused);
    }

    private void OnDisable()
    {
        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterFixedUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }

    protected virtual void Move()
    {
        speed = target.GetSpeed() * 2f;
        Vector3 thisPos = new Vector3(transform.position.x, 0.25f, transform.position.z);
        Vector3 targetPos = new Vector3(target.transform.position.x, 0.25f, target.transform.position.z);
        transform.position = Vector3.MoveTowards(thisPos, targetPos, speed * Time.deltaTime);
    }

    protected virtual void ManageTriggerEnter(Collider other) { }

    protected virtual void Effect() { }

    public virtual void Activate()
    {
        active = true;
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void EnableGracePeriod()
    {
        gracePeriodTimer = GameManager.current.timerService.StartTimer(graceTime, () => canBeActive = true);
    }
}
