using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour, IUpdate, IPause
{
    protected bool active = false;

    protected bool useDistance = true;
    [SerializeField] protected float currentDistance;
    [SerializeField] protected float currentLifetime;

    [SerializeField] protected List<GameObject> hitEnemies = new List<GameObject>();

    [SerializeField] protected SpriteRenderer _sprt;
    [SerializeField] protected DamageCollider _damageCollider;

    protected float damage;
    protected float damageRadius;
    protected float speed;
    protected float critChance;
    protected float critMult;
    protected int penetration;
    protected float maxDistance;
    protected float maxLifetime;
    protected float knockback;
    protected float armingDistance;
    protected float armingLifetime;
    protected bool explodeImmediately;

    protected Color hitColor;
    protected Sprite hitSprite;

    [SerializeField] protected int currentPenetration;

    protected void OnEnable()
    {
        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);

        _sprt = GetComponentInChildren<SpriteRenderer>();
        _damageCollider = GetComponentInChildren<DamageCollider>();
        _damageCollider.onCollisionEnter += ManageOnCollisionEnter;
        _damageCollider.onTriggerEnter += ManageOnTriggerEnter;

        currentDistance = 0f;
        currentLifetime = 0f;

        active = true;
    }

    public virtual void ExecuteUpdate()
    {
        if (active)
        {
            Move();

            LookAtCamera();

            if (useDistance)
            {
                if (currentDistance >= maxDistance) ResetAndReturn();
            } else
            {
                if (currentLifetime >= maxLifetime) ResetAndReturn();
            }
        }
    }
    public void Pause(bool paused)
    {
        active = !paused;
    }

    protected void OnDisable()
    {
        hitEnemies.Clear();

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

        if (useDistance) currentDistance += speed * Time.deltaTime;
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

        currentDistance = 0f;
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

    public void ProjectileSetup(float damageVar,
                                        float damageRadiusVar,
                                        float speedVar,
                                        float critChanceVar,
                                        float critMultiplierVar,
                                        int penetrationVar,
                                        bool useDistanceVar,
                                        float maxDistanceVar,
                                        float maxLifetimeVar,
                                        float knockbackVar,
                                        float armingDistanceVar,
                                        float armingLifetimeVar,
                                        bool explodeImmediately,
                                        Sprite projSprite,
                                        Color projColor,
                                        Color hitColor,
                                        Sprite hitSprite = null)
    {
        damage = damageVar;
        damageRadius = damageRadiusVar;
        speed = speedVar;
        critChance = critChanceVar;
        critMult = critMultiplierVar;
        penetration = penetrationVar;
        useDistance = useDistanceVar;
        maxDistance = maxDistanceVar;
        maxLifetime = maxLifetimeVar;
        knockback = knockbackVar;
        armingDistance = armingDistanceVar;
        armingLifetime = armingLifetimeVar;
        this.explodeImmediately = explodeImmediately;

        if (projSprite != null) _sprt.sprite = projSprite;
        _sprt.color = projColor;

        this.hitColor = hitColor;
        this.hitSprite = hitSprite;

        currentPenetration = penetration;
    }
}
