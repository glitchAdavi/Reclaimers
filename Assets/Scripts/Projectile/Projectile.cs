using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IUpdate, IPause
{
    protected bool active = false;

    protected bool useDistance = true;
    [SerializeField] protected float currentDistance;
    [SerializeField] protected float currentLifetime;

    [SerializeField] protected List<GameObject> hitEnemies = new List<GameObject>();

    [SerializeField] protected SpriteRenderer _sprt;
    [SerializeField] protected DamageCollider _damageCollider;

    protected float damage;
    protected float speed;
    protected int penetration;
    protected float maxDistance;
    protected float maxLifetime;
    protected float knockback;

    protected Color hitColor;
    protected Sprite hitSprite;

    [SerializeField] protected int currentPenetration;

    protected void OnEnable()
    {
        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);

        _sprt = GetComponentInChildren<SpriteRenderer>();
        _damageCollider.onTriggerEnter += ManageOnTriggerEnter;

        currentDistance = 0f;
        currentLifetime = 0f;
        active = true;
    }

    public void ExecuteUpdate()
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

        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }

    protected void Move()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);

        if (useDistance) currentDistance += speed * Time.deltaTime;
        else currentLifetime += Time.deltaTime;
    }

    protected void LookAtCamera()
    {
        _sprt.transform.rotation = Quaternion.LookRotation(GameManager.current.playerCamera.transform.forward, -transform.right);
    }

    public void ManageOnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 22) //LevelCollision
        {
            ResetAndReturn();
        }

        if (other.gameObject.layer == 11 && other.gameObject.name == "DamageCollider") //Enemy
        {
            Pawn enemyHit = GetPawnFromCollision(other);

            if (penetration >= 0)
            {
                if (!hitEnemies.Contains(enemyHit.gameObject))
                {
                    hitEnemies.Add(enemyHit.gameObject);
                    enemyHit.GetHit(damage, false, knockback, transform.forward); // IS CRIT?
                    penetration--;
                }
            } else
            {
                ResetAndReturn();
            }
        }
    }

    public Pawn GetPawnFromCollision(Collider collider)
    {
        return collider.gameObject.transform.parent.GetComponentInChildren<Pawn>();
    }

    protected void ResetAndReturn()
    {
        active = false;
        hitEnemies.Clear();

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
                                        float speedVar,
                                        int penetrationVar,
                                        bool useDistanceVar,
                                        float maxDistanceVar,
                                        float maxLifetimeVar,
                                        float knockbackVar,
                                        Color hitColor,
                                        Sprite hitSprite = null)
    {
        damage = damageVar;
        speed = speedVar;
        penetration = penetrationVar;
        useDistance = useDistanceVar;
        maxDistance = maxDistanceVar;
        maxLifetime = maxLifetimeVar;
        knockback = knockbackVar;

        this.hitColor = hitColor;
        this.hitSprite = hitSprite;

        currentPenetration = penetration;
    }
}
