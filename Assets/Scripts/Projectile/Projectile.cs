using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TreeEditor;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IUpdate, IPause
{
    protected bool active = false;

    protected bool useDistance = true;
    [SerializeField] protected float currentLifetime;

    [SerializeField] protected Light _lgt;

    [SerializeField] protected List<GameObject> hitEnemies = new List<GameObject>();

    [SerializeField] protected SpriteRenderer _sprt;
    [SerializeField] protected Vector3 _sprtBaseScale = Vector3.zero;
    [SerializeField] protected DamageCollider _damageCollider;
    [SerializeField] protected Vector3 _damageColliderBaseScale = Vector3.zero;
    [SerializeField] protected TrailRenderer _tr;
    [SerializeField] protected float _trBaseScale = 0f;

    protected float scale;
    protected float damage;
    protected float damageRadius;
    protected float speed;
    protected float critChance;
    protected float critMult;
    protected int penetration;
    protected float maxLifetime;
    protected float knockback;
    protected float armingLifetime;
    protected bool explodeImmediately;
    protected float explosionRadius;

    protected Sprite projSpriteAux;
    protected Color projSpriteColorAux;

    protected Color hitColor;
    protected Sprite hitSprite;

    [SerializeField] protected int currentPenetration;

    protected virtual void OnEnable()
    {
        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);

        _lgt = GetComponentInChildren<Light>();
        _sprt = GetComponentInChildren<SpriteRenderer>();
        _tr = GetComponentInChildren<TrailRenderer>();
        _damageCollider = GetComponentInChildren<DamageCollider>();
        _damageCollider.onCollisionEnter += ManageOnCollisionEnter;
        _damageCollider.onTriggerEnter += ManageOnTriggerEnter;

        if (_sprtBaseScale == Vector3.zero) _sprtBaseScale = _sprt.transform.localScale;
        if (_damageColliderBaseScale == Vector3.zero) _damageColliderBaseScale = _damageCollider.transform.localScale;
        if (_trBaseScale == 0) _trBaseScale = _tr.startWidth;

        _sprt.transform.localScale = _sprtBaseScale;
        _damageCollider.transform.localScale = _damageColliderBaseScale;
        _tr.startWidth = _trBaseScale;

        currentLifetime = 0f;

        active = true;
    }

    public virtual void ExecuteUpdate()
    {
        if (active)
        {
            Move();

            LookAtCamera();

            if (!_tr.emitting) _tr.emitting = true;

            if (currentLifetime >= maxLifetime) ResetAndReturn();
        }
    }

    public void Pause(bool paused)
    {
        active = !paused;
    }

    protected void OnDisable()
    {
        hitEnemies.Clear();

        _tr.emitting = false;
        _lgt.enabled = false;

        _damageCollider.onCollisionEnter -= ManageOnCollisionEnter;
        _damageCollider.onTriggerEnter -= ManageOnTriggerEnter;

        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }

    protected void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);

        if (!GameManager.current.tileService.IsPositionInsidePlayableArea(transform.position)) ResetAndReturn();

        if (useDistance)
        {
            float dot = Mathf.Abs(Vector3.Dot(transform.forward, Vector3.forward));
            currentLifetime += speed * Time.deltaTime * Mathf.Lerp(1f, 0.7f, dot);
        }
        else currentLifetime += Time.deltaTime;
    }

    protected bool CheckIfCrit()
    {
        if (critChance >= Random.Range(0f, 100f)) return true;
        return false;
    }

    protected void LookAtCamera()
    {
        _sprt.transform.rotation = Quaternion.LookRotation(GameManager.current.playerCamera.transform.forward, -transform.right);
    }

    public virtual void ManageOnCollisionEnter(Collision collider) { }

    public virtual void ManageOnTriggerEnter(Collider other) { }

    public Pawn GetPawnFromCollision(Collider collider)
    {
        return collider.gameObject.transform.parent.GetComponentInChildren<Pawn>();
    }

    protected virtual void ResetAndReturn()
    {
        active = false;
        hitEnemies.Clear();

        currentLifetime = 0f;

        GameManager.current.projectileBuilder.ReturnProjectile(this);
    }

    public static void TurnOn(Projectile p)
    {
        p.gameObject.SetActive(true);
    }

    public static void TurnOff(Projectile p)
    {
        p.gameObject.SetActive(false);
    }

    public virtual void ProjectileSetup(float projScaleVar,
                                float damageVar,
                                float damageRadiusVar,
                                float speedVar,
                                float critChanceVar,
                                float critMultiplierVar,
                                int penetrationVar,
                                bool useDistanceVar,
                                float maxLifetimeVar,
                                float knockbackVar,
                                float armingLifetimeVar,
                                bool explodeImmediately,
                                float explosionRadiusVar,
                                Sprite projSprite,
                                Color projColor,
                                Sprite projSpriteAux,
                                Color projColorAux,
                                Color hitColor,
                                Sprite hitSprite = null)
    {
        scale = projScaleVar;
        _sprt.transform.localScale = _sprt.transform.localScale * projScaleVar;
        _damageCollider.transform.localScale = _damageCollider.transform.localScale * projScaleVar;
        _tr.startWidth = _tr.startWidth * projScaleVar;

        damage = damageVar;
        damageRadius = damageRadiusVar;
        speed = speedVar;
        critChance = critChanceVar;
        critMult = critMultiplierVar;
        penetration = penetrationVar;
        useDistance = useDistanceVar;
        maxLifetime = maxLifetimeVar;
        knockback = knockbackVar;
        armingLifetime = armingLifetimeVar;
        this.explodeImmediately = explodeImmediately;
        explosionRadius = explosionRadiusVar;

        if (projSprite != null) _sprt.sprite = projSprite;
        _sprt.color = projColor;

        if (projSpriteAux != null) this.projSpriteAux = projSpriteAux;
        projSpriteColorAux = projColorAux;

        this.hitColor = hitColor;
        this.hitSprite = hitSprite;

        currentPenetration = penetration;
    }


    public void SeparateFromWall(Collider wallCol, Vector3 wallPos, Quaternion wallRot)
    {
        Collider projCollider = _damageCollider.gameObject.GetComponent<Collider>();
        Vector3 projPos = transform.position + new Vector3(0f, 1f, 0f);
        if (Physics.ComputePenetration(projCollider, projPos, transform.rotation, wallCol, wallPos, wallRot, out Vector3 dir, out float dist))
        {
            Vector3 temp = transform.position + (dir * dist);
            if (GameManager.current.DistanceToPlayer(temp) > GameManager.current.DistanceToPlayer(transform.position))
            {
                Vector3 dirToPlayer = GameManager.current.gameInfo.playerPositionVar.Value - transform.position;
                dirToPlayer = new Vector3(dirToPlayer.x, 0.5f, dirToPlayer.z);

                Debug.DrawLine(projPos, projPos + (dirToPlayer.normalized * 0.7f));

                projPos = transform.position + (dirToPlayer.normalized * 0.7f) + new Vector3(0f, 1f, 0f);
                transform.position += dirToPlayer.normalized * 0.7f;
                if (Physics.ComputePenetration(projCollider, projPos, transform.rotation, wallCol, wallPos, wallRot, out Vector3 dir2, out float dist2))
                {
                    transform.position += dir2 * dist2;
                }
            } else
            {
                transform.position += dir * dist;
            }
        }
    }
}
