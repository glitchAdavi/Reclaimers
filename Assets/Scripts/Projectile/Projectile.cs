using System.Collections.Generic;
using Reclaimers.Systems;
using Reclaimers.UpdateSystem;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] protected Pawn p;

    [SerializeField] protected ExternalCollider _extCol;
    [SerializeField] protected Vector3 _extColBaseScale = Vector3.zero;
    [SerializeField] protected SpriteRenderer _sprt;
    [SerializeField] protected Vector3 _sprtBaseScale = Vector3.zero;
    [SerializeField] protected Light _lgt;
    [SerializeField] protected TrailRenderer _tr;
    [SerializeField] protected float _trBaseScale = 0;
    [SerializeField] protected List<GameObject> hitEnemies = new List<GameObject>();
    [SerializeField] protected List<string> onHitEffectsToApply = new List<string>();

    [SerializeField] protected int targetLayer = 11;

    [SerializeField] protected int currentPen = 0;

    protected GameObject lastHit = null;
    protected bool isActive = false;
    protected bool isPaused = false;

    protected Sprite hitSprite;

    protected virtual void OnEnable()
    {
        this.Register();

        if (_extCol == null || _sprt == null) Setup();

        hitEnemies.Clear();
        lastHit = null;
        _extCol.transform.localPosition = new Vector3(0f, 0f, 0f);
        _extCol.onCollisionEnter += ManageOnCollisionEnter;

        if (_extColBaseScale == Vector3.zero) _extColBaseScale = _extCol.transform.localScale;
        if (_sprtBaseScale == Vector3.zero) _sprtBaseScale = _sprt.transform.localScale;
        if (_trBaseScale == 0) _trBaseScale = _tr.startWidth;

        _sprt.transform.localScale = _sprtBaseScale;
        _extCol.transform.localScale = _extColBaseScale;
        _tr.startWidth = _trBaseScale;

        GameManager.Instance.eventSystem.onEnemyDeath += RemoveFromHit;
    }

    protected void OnDisable()
    {
        GameManager.Instance.eventSystem.onEnemyDeath -= RemoveFromHit;

        _extCol.onCollisionEnter -= ManageOnCollisionEnter;

        this.Unregister();
    }

    public void ManagedUpdate()
    {
        if (!isPaused)
        {
            if (!_tr.emitting) _tr.emitting = true;
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
        //_lgt = gameObject.GetComponentInChildren<Light>();
        _tr = gameObject.GetComponentInChildren<TrailRenderer>();
    }

    public void RemoveFromHit(Pawn enemy)
    {
        if (hitEnemies.Count < 1) return;

        GameObject enemyGO = enemy.GetComponentInChildren<Collider>().gameObject;
        if (hitEnemies.Contains(enemyGO)) hitEnemies.Remove(enemyGO);
    }

    public void SetTargetLayer(int targetLayer)
    {
        this.targetLayer = targetLayer;
    }
}
