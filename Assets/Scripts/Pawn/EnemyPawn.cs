using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPawn : Pawn
{
    [SerializeField] protected Pawn target;

    protected bool dropXp = true;

    [SerializeField] protected int pathUpdateTimer = 0;
    [SerializeField] protected int pathUpdateTimerLimit = 1;

    protected bool canMeleeAttack = true;
    Timer attackCooldown;

    protected override void OnEnable()
    {
        base.OnEnable();

        FirstStatApplication();

        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
    }
    
    protected override void PawnUpdate()
    {
        base.PawnUpdate();
       
        if (target != null) Move();
        else target = GameManager.current.pawnService.GetTarget(); // replace when pawn service is active

        if (Vector3.Distance(transform.position, target.transform.position) <= _nav.radius + target.GetNavRadius() + 0.5f)
        {
            if (canMeleeAttack) MeleeHit();
        }
    }

    protected override void PawnPause()
    {
        base.PawnPause();

        attackCooldown?.Pause(isPaused);

        _nav.velocity = Vector3.zero;

        if (isPaused) _nav.isStopped = true;
        else _nav.isStopped = false;
    }

    protected override void OnDisable()
    {
        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }

    #region Movement
    protected void Move()
    {
        if (!_nav.hasPath) GetPath();

        pathUpdateTimer++;
        if (pathUpdateTimer > pathUpdateTimerLimit)
        {
            GetPath();
            pathUpdateTimer = 0;
        }

    }

    protected void GetPath()
    {
        if (_nav.enabled == false) return;
        NavMeshPath path = new NavMeshPath();
        _nav.CalculatePath(target.GetPosition(), path);
        _nav.SetPath(path);
    }
    #endregion

    #region Attack
    protected void MeleeHit()
    {
        canMeleeAttack = false;
        target.GetHit(meleeDamage, false);
        attackCooldown = GameManager.current.timerService.StartTimer(meleeCooldown, () => canMeleeAttack = true);
    }


    #endregion

    public override void GetHit(float damage, bool isCrit, float knockback = 0, Vector3? knockbackPush = null)
    {
        base.GetHit(damage, isCrit, knockback, knockbackPush);
        GameManager.current.eventService.EnemyHurt(transform.position, damage, isCrit);
    }

    protected override void Die()
    {
        ResetAndReturn();
    }


    #region PoolIO
    protected void ResetAndReturn()
    {
        GameManager.current.eventService.SpawnXp(transform.position, xpKillValue);
        GameManager.current.eventService.EnemyDeath(this);
        GameManager.current.pawnService.enemyBuilder.ReturnEnemyPawn(this);
    }


    public static void TurnOn(EnemyPawn ep)
    {
        ep.gameObject.SetActive(true);
    }

    public static void TurnOff(EnemyPawn ep)
    {
        ep.gameObject.SetActive(false);
    }
    #endregion

    protected override void FirstStatApplication()
    {
        base.FirstStatApplication();
        ApplyMeleeDamage();
        ApplyMeleeCooldown();
        ApplyXpKillValue();
    }
}
