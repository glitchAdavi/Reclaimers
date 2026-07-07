using System.Collections.Generic;
using Reclaimers.Systems;
using Reclaimers.UpdateSystem;
using UnityEngine;

public enum ProjectileType
{
    P_Basic,
    P_Bounce,
    P_Split
}

public abstract class Projectile : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] protected Pawn p;

    [SerializeField] protected ExternalCollider _extCol;
    [SerializeField] protected SpriteRenderer _sprt;
    [SerializeField] protected List<GameObject> hitEnemies = new List<GameObject>();
    [SerializeField] protected List<string> onHitEffectsToApply = new List<string>();

    [SerializeField] protected int currentPen = 0;

    protected GameObject lastHit = null;
    protected bool isActive = false;
    protected bool isPaused = false;


    protected void OnEnable()
    {
        this.Register();

        if (_extCol == null || _sprt == null) Setup();

        hitEnemies.Clear();
        lastHit = null;
        _extCol.transform.localPosition = new Vector3(0f, 0f, 0f);
        _extCol.onCollisionEnter += ManageOnCollisionEnter;

        EventSystem.Instance.onEnemyDeath += RemoveFromHit;
    }

    protected void OnDisable()
    {
        EventSystem.Instance.onEnemyDeath -= RemoveFromHit;

        _extCol.onCollisionEnter -= ManageOnCollisionEnter;

        this.Unregister();
    }

    public void ManagedUpdate()
    {
        if (!isPaused)
        {
            Move();
            LookAtCamera();
        }
    }

    public void ManagedPause(bool paused)
    {
        isPaused = paused;

    }

    protected void Move()
    {
        _extCol.transform.localPosition = new Vector3(0f, 0f, 0f);
        Vector3 movement = transform.forward * p.statBlock.projSpeed.Value() * Time.deltaTime;
        transform.position += new Vector3(movement.x, 0f, movement.z);
    }

    protected void LookAtCamera()
    {
        _sprt.transform.rotation = Quaternion.LookRotation(GameManager.Instance.playerCamera.transform.forward, -transform.right);
    }

    public abstract void ManageOnCollisionEnter(Collision collision);

    public void SeparateFromWall(Collider wallCol, Vector3 wallPos, Quaternion wallRot)
    {
        Collider projCollider = _extCol.gameObject.GetComponent<Collider>();
        Vector3 projPos = transform.position + new Vector3(0f, 1f, 0f);
        if (Physics.ComputePenetration(projCollider, projPos, transform.rotation, wallCol, wallPos, wallRot, out Vector3 dir, out float dist))
        {
            Vector3 temp = transform.position + (dir * dist);
            if (GameManager.Instance.DistanceToPlayer(temp) > GameManager.Instance.DistanceToPlayer(transform.position))
            {
                Vector3 dirToPlayer = GameManager.Instance.gameInfo.playerPositionVar.Value - transform.position;
                dirToPlayer = new Vector3(dirToPlayer.x, 0.5f, dirToPlayer.z);

                Debug.DrawLine(projPos, projPos + (dirToPlayer.normalized * 0.7f));

                projPos = transform.position + (dirToPlayer.normalized * 0.7f) + new Vector3(0f, 1f, 0f);
                transform.position += dirToPlayer.normalized * 0.7f;
                if (Physics.ComputePenetration(projCollider, projPos, transform.rotation, wallCol, wallPos, wallRot, out Vector3 dir2, out float dist2))
                {
                    transform.position += dir2 * dist2;
                }
            }
            else
            {
                transform.position += dir * dist;
            }
        }
    }


    public void SetShooter(Pawn p, int overridePen = -1, List<GameObject> inheritHitEnemies = null)
    {
        this.p = p;

        if (overridePen < 0) currentPen = p.statBlock.projPenetration.ValueInt();
        else currentPen = overridePen;

        if (inheritHitEnemies != null) hitEnemies = new List<GameObject>(inheritHitEnemies);
        else hitEnemies = new List<GameObject>();
    }

    public void HitPawn(Pawn p)
    {
        bool crit = this.p.IsCrit();
        if (crit) p.GetHit(-this.p.statBlock.projDamage.Value() * this.p.statBlock.critMult.Value(), true, this.p.statBlock.projStun.Value() * this.p.statBlock.critMult.Value());
        else p.GetHit(-this.p.statBlock.projDamage.Value(), false, this.p.statBlock.projStun.Value());
        OnHit(p);
    }

    public void OnHit(Pawn p)
    {
        foreach (string se in onHitEffectsToApply)
        {
            GameManager.Instance.GetModifier(se).Apply(p);
        }
    }

    public void Setup()
    {
        _extCol = gameObject.GetComponentInChildren<ExternalCollider>();
        _sprt = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void RemoveFromHit(Pawn enemy)
    {
        if (hitEnemies.Count < 1) return;

        GameObject enemyGO = enemy.GetComponentInChildren<Collider>().gameObject;
        if (hitEnemies.Contains(enemyGO)) hitEnemies.Remove(enemyGO);
    }
}
