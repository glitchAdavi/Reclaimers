using Reclaimers.DeveloperConsole;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class P_Split : Projectile
{
    protected override void OnEnable()
    {
        hitSprite = GameManager.Instance.gameInfo.hitWall;

        base.OnEnable();
    }

    public override void ManageOnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 20)
        {
            GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.wallHitAudio, transform.position, 0, -1);
            SeparateFromWall(collision.collider, collision.transform.position, collision.transform.rotation);
            Return();

            if (hitSprite != null)
            {
                Vector3 yVar = new Vector3(0f, Random.Range(0.25f, 0.75f), 0f);
                Instantiate(GameManager.Instance.gameInfo.hitPrefab, transform.position + yVar, Quaternion.Euler(45f, 0f, 0f)).
                    GetComponent<Hit>().
                    Activate(hitSprite);
            }
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

                    Return();
                }
            }
            else
            {
                if (!hitEnemies.Contains(enemyHit.gameObject))
                {
                    hitEnemies.Add(enemyHit.gameObject);

                    HitPawn(enemyHit);
                }

                Return();
            }
        }
    }

    public void InstantiateProjectile(Vector3 forw, float spread)
    {
        Projectile proj = p.projectileBuilder.GetObject();
        proj.transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
        proj.transform.forward = forw;
        proj.transform.Rotate(Vector3.up, spread);
        proj.SetShooter(p, currentPen, hitEnemies);
        proj.SetVisible();
    }
}
