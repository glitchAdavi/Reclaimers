using System.Collections.Generic;
using Reclaimers.UpdateSystem;
using UnityEngine;

public enum ProjectileCollection
{
    P_Basic,
    P_Bounce
}

public abstract class Projectile : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] protected Pawn shooter;

    [SerializeField] protected ExternalCollider _extCol;
    [SerializeField] protected SpriteRenderer _sprt;
    [SerializeField] protected List<GameObject> hitEnemies = new List<GameObject>();

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
        _extCol.onCollisionEnter += ManageOnCollisionEnter;
    }

    protected void OnDisable()
    {
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
        Vector3 movement = transform.forward * shooter.weaponBlock.projSpeed.Value() * Time.deltaTime;
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


    public void SetShooter(Pawn shooter)
    {
        this.shooter = shooter;
        currentPen = shooter.weaponBlock.projPenetration.ValueInt();
    }

    public void Setup()
    {
        _extCol = gameObject.GetComponentInChildren<ExternalCollider>();
        _sprt = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public static void TurnOn(Projectile p)
    {
        p.gameObject.SetActive(true);
    }

    public static void TurnOff(Projectile p)
    {
        p.gameObject.SetActive(false);
    }
}
