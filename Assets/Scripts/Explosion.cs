using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] Light flash;

    [SerializeField] float radius = 0f;
    [SerializeField] float damage = 0f;

    Timer timerDeath;

    public void Init(float damage, float radius)
    {
        this.radius = radius;
        this.damage = damage;
    }

    public void Explode()
    {        
        Collider[] allHits = Physics.OverlapSphere(transform.position, radius, 1 << 11);
        foreach (Collider c in allHits)
        {
            if (c.gameObject.name.Equals("DamageCollider")) c.gameObject.GetComponentInParent<EnemyPawn>().GetHit(damage, false);
        }
        

        timerDeath = GameManager.current.timerService.StartTimer(0.05f, () => Destroy(gameObject));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
