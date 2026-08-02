using Reclaimers.DeveloperConsole;
using UnityEngine;

public class P_Basic : Projectile
{
    protected override void OnEnable()
    {
        hitSprite = GameManager.Instance.gameInfo.hitWall;

        base.OnEnable();
    }

    public override void ManageOnCollisionEnter(Collision collision)
    {
        LogSystem.Log("Detects");

        if (collision.gameObject.layer == 20)
        {
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

        if (collision.gameObject.layer == targetLayer)
        {
            LogSystem.Log("Hits");
            Pawn enemyHit = collision.collider.gameObject.GetComponentInParent<Pawn>();

            if (enemyHit == null) return;

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

                Return();
            }

            if (enemyHit.statBlock.hitSprite != null)
            {
                Vector3 yVar = new Vector3(0f, Random.Range(0.25f, 0.75f), 0f);
                Instantiate(GameManager.Instance.gameInfo.hitPrefab, transform.position + yVar, Quaternion.Euler(45f, 0f, 0f)).
                    GetComponent<Hit>().
                    Activate(enemyHit.statBlock.hitSprite);
            }
        }
    }
    
}
