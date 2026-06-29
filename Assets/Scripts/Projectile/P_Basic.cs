using UnityEngine;

public class P_Basic : Projectile
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

            if (currentPen > 0)
            {
                if (!hitEnemies.Contains(enemyHit.gameObject))
                {
                    hitEnemies.Add(enemyHit.gameObject);
                    HitPawn(enemyHit);

                    currentPen--;
                }
            } else
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
    
}
