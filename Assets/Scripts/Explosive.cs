using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    [SerializeField] Light flash;
    [SerializeField] Modifier toApply;

    [SerializeField] float radius = 0f;
    [SerializeField] float damage = 0f;

    Timer timerDelay;
    Timer timerDeath;

    public void Init(float damage, float radius, float delay = 0f, Modifier toApply = null)
    {
        this.radius = radius;
        this.damage = damage;
        this.toApply = toApply;
        if (delay >= 0f)
        {
            if (delay == 0f) Explode();
            else timerDelay = GameManager.current.timerService.StartTimer(delay, Explode);
        }
    }

    public void Explode()
    {
        flash.enabled = true;
        Collider[] allHits = Physics.OverlapSphere(transform.position, radius, 1 << 11);
        Debug.Log(allHits.Length);
        foreach (Collider c in allHits)
        {
            if (c.gameObject.name.Equals("DamageCollider"))
            {
                Debug.Log("Hit");
                EnemyPawn e = c.gameObject.GetComponentInParent<EnemyPawn>();
                if (damage > 0) e.GetHit(damage, false);
                if (toApply != null)
                {
                    e.AddModifier(toApply);
                }
            }
        }
        

        timerDeath = GameManager.current.timerService.StartTimer(0.05f, () => Destroy(gameObject));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
