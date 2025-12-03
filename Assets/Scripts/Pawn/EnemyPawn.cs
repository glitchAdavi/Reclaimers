using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPawn : Pawn
{
    [SerializeField] protected Pawn target;
    [SerializeField] protected bool active;
    public bool irregularSpawn = false;

    protected bool dropXp = true;

    [SerializeField] protected int pathUpdateTimer = 0;
    [SerializeField] protected int pathUpdateTimerLimit = 1;

    protected bool canMeleeAttack = true;
    Timer attackCooldown;

    [SerializeField] protected bool isIdle = false;
    [SerializeField] protected int playerDetectionRange = 10;

    [SerializeField] protected float onDamageSpawnPartialLeftover = 0f;

    protected override void OnEnable()
    {
        active = true;
        isDead = false;
        base.OnEnable();

    }

    public void InitializeEnemyPawn(PawnStatBlock pawnStatBlock)
    {
        if (pawnStatBlock != null) baseStatBlock = pawnStatBlock;
        else baseStatBlock = GameManager.current.gameInfo.defaultEnemyStatBlock;
        statBlock = ScriptableObject.CreateInstance<PawnStatBlock>();
        statBlock.CopyValues(baseStatBlock);

        FirstStatApplication();

        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
    }
    
    protected override void PawnUpdate()
    {
        base.PawnUpdate();

        if (isIdle)
        {
            if (CheckIfPlayerIsClose())
            {
                GameManager.current.eventService.RequestUISpawnFloatingText(transform.position, "<size=12>!", Color.yellow, 0f, 0.5f);
                isIdle = false;
            }

        } else
        {
            if (target != null) Move();
            else target = GameManager.current.pawnService.GetTarget();

            if (Vector3.Distance(transform.position, target.transform.position) <= _nav.radius + target.GetNavRadius() + interactionRange)
            {
                if (canMeleeAttack) MeleeHit();
            }
        }
    }

    protected override void PawnPause()
    {
        base.PawnPause();

        if (active)
        {
            attackCooldown?.Pause(isPaused);

            if (!_rb.isKinematic) _rb.isKinematic = true;

            _nav.velocity = Vector3.zero;

            _anm.enabled = !isPaused;

            if (_nav.isOnNavMesh)
            {
                if (isPaused) _nav.isStopped = true;
                else _nav.isStopped = false;
            }
        }
    }

    protected override void OnDisable()
    {
        active = false;
        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }

    #region Movement
    protected void Move()
    {
        if (!active) return;

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
        if (!active) return;
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

        isIdle = false;

        string damageText = $"Crit!\n{damage}";
        if (isCrit) GameManager.current.eventService.RequestUISpawnFloatingText(transform.position, damageText, Color.red, 2f, 0.5f);
        else GameManager.current.eventService.RequestUISpawnFloatingText(transform.position, $"{damage}", Color.white, 2f, 0.5f);

        if (statBlock.onDamageSpawnEnemy && statBlock.onDamageEnemyToSpawn != null)
        {
            float temp = damage + onDamageSpawnPartialLeftover;
            while (temp >= statBlock.onDamageInterval.Value())
            {
                if (statBlock.scale.Mult > 1) statBlock.scale.SetMult(statBlock.scale.Mult - 0.2f);
                else statBlock.scale.SetMult(1f);
                ApplyScale();
                GameManager.current.eventService.RequestEnemySpawn(statBlock.onDamageEnemyToSpawn, new Vector3(transform.position.x, 0.5f, transform.position.z), 2f);
                temp -= statBlock.onDamageInterval.Value();
            }
            onDamageSpawnPartialLeftover = temp;
        }
    }

    protected override void Die()
    {
        if (!isDead)
        {
            if (statBlock.onDeathSpawnEnemy && statBlock.onDeathEnemyToSpawn != null)
            {
                int eToSpawn = statBlock.onDeathNumToSpawn.ValueInt();
                while (eToSpawn > 0)
                {
                    GameManager.current.eventService.RequestEnemySpawn(statBlock.onDeathEnemyToSpawn, new Vector3(transform.position.x, 0.5f, transform.position.z), 2f);
                    eToSpawn--;
                }
            }
        }

        isDead = true;
        ResetAndReturn();
    }

    public void DieSilently()
    {
        isDead = true;
        GameManager.current.pawnService.enemyBuilder.ReturnEnemyPawn(this);
    }

    public void SetIsIdle(bool idle)
    {
        isIdle = idle;
    }

    protected bool CheckIfPlayerIsClose()
    {
        return GameManager.current.pawnService.IsPlayerClose(transform.position, playerDetectionRange);
    }


    #region PoolIO
    protected void ResetAndReturn()
    {
        GameManager.current.eventService.SpawnXp(transform.position, xpKillValue);
        if (!irregularSpawn) GameManager.current.eventService.EnemyDeath(this);
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
        ApplyInteractionRange();
    }
}
