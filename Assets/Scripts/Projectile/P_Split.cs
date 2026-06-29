using Reclaimers.DeveloperConsole;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class P_Split : Projectile
{
    public override void ManageOnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 20)
        {
            SeparateFromWall(collision.collider, collision.transform.position, collision.transform.rotation);
            shooter.projectileBuilder.ReturnObject(this);
        }

        if (collision.gameObject.layer == 11)
        {
            Pawn enemyHit = collision.collider.gameObject.GetComponentInParent<Pawn>();

            if (enemyHit == null) return;

            if (currentPen > 0)
            {
                if (!hitEnemies.Contains(enemyHit.gameObject))
                {
                    hitEnemies.Add(enemyHit.gameObject);

                    HitPawn(enemyHit);

                    currentPen--;

                    InstantiateProjectile(transform.forward, -20f);
                    InstantiateProjectile(transform.forward, 20f);

                    shooter.projectileBuilder.ReturnObject(this);
                }
            }
            else
            {
                if (!hitEnemies.Contains(enemyHit.gameObject))
                {
                    hitEnemies.Add(enemyHit.gameObject);

                    HitPawn(enemyHit);
                }

                shooter.projectileBuilder.ReturnObject(this);
            }
        }
    }

    public void InstantiateProjectile(Vector3 forw, float spread)
    {
        Projectile proj = shooter.projectileBuilder.GetObject();
        proj.transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
        proj.transform.forward = forw;
        proj.transform.Rotate(Vector3.up, spread);
        proj.SetShooter(shooter, currentPen, hitEnemies);
    }
}
