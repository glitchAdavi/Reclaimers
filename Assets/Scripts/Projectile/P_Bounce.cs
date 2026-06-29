using UnityEngine;

public class P_Bounce : Projectile
{
    public override void ManageOnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 20)
        {
            SeparateFromWall(collision.collider, collision.transform.position, collision.transform.rotation);
            
            if (currentPen > 0)
            {
                if (lastHit == null || !lastHit.Equals(collision.gameObject))
                {
                    lastHit = collision.gameObject;
                    Reflect(20);
                }
            } else
            {
                shooter.projectileBuilder.ReturnObject(this);
            }

        }

        if (collision.gameObject.layer == 11)
        {
            Pawn enemyHit = collision.collider.gameObject.GetComponentInParent<Pawn>();

            if (currentPen > 0)
            {
                if (lastHit == null || !lastHit.Equals(collision.gameObject))
                {
                    lastHit = collision.gameObject;

                    HitPawn(enemyHit);

                    Reflect(11);
                }
            }
            else
            {
                if (lastHit == null || !lastHit.Equals(collision.gameObject))
                {
                    lastHit = collision.gameObject;

                    HitPawn(enemyHit);

                    Reflect(11);
                }

                shooter.projectileBuilder.ReturnObject(this);
            }
        }
    }

    private void Reflect(int layer)
    {
        if (Physics.Raycast(transform.position - (transform.forward.normalized * 3), transform.forward.normalized, out RaycastHit info, 10f, 1 << layer))
        {
            Vector3 newForward = Vector3.Reflect(transform.forward.normalized, info.normal);
            transform.forward = new Vector3(newForward.x, transform.forward.y, newForward.z);
        }

        currentPen--;
    }
}
