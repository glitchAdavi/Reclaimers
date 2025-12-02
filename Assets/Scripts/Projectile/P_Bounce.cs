using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Bounce : Projectile
{
    public GameObject lastHit = null;

    protected override void ResetAndReturn()
    {
        lastHit = null;
        base.ResetAndReturn();
    }

    public override void ManageOnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.layer == 22) //LevelCollision
        {
            GameObject hit = collider.gameObject;

            if (currentPenetration > 0)
            {
                if (lastHit == null || !lastHit.Equals(hit))
                {
                    lastHit = hit;

                    if (hitSprite != null) Instantiate(GameManager.current.gameInfo.weaponHitPrefab, transform.position, Quaternion.identity).GetComponent<WeaponHit>().Activate(hitSprite, hitColor);

                    currentLifetime = 0f;

                    if (Physics.Raycast(transform.position - (transform.forward.normalized * 3), transform.forward.normalized, out RaycastHit info, 10f, 1 << 22))
                    {
                        Vector3 newForward = Vector3.Reflect(transform.forward.normalized, info.normal);
                        transform.forward = new Vector3(newForward.x, transform.forward.y, newForward.z);
                    }

                    currentPenetration--;
                }
            }
            else
            {
                ResetAndReturn();
            }
        }
    }

    public override void ManageOnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && other.gameObject.name == "DamageCollider") //Enemy
        {
            Pawn enemyHit = GetPawnFromCollision(other);

            if (currentPenetration > 0)
            {
                if (lastHit == null || !lastHit.Equals(enemyHit.gameObject))
                {
                    lastHit = enemyHit.gameObject;

                    if (hitSprite != null) Instantiate(GameManager.current.gameInfo.weaponHitPrefab, transform.position, Quaternion.identity).GetComponent<WeaponHit>().Activate(hitSprite, hitColor);

                    currentLifetime = 0f;

                    if (CheckIfCrit()) enemyHit.GetHit(damage * critMult, true, knockback * critMult, transform.forward);
                    else enemyHit.GetHit(damage, false, knockback, transform.forward);

                    if (Physics.Raycast(transform.position - transform.forward.normalized, transform.forward.normalized, out RaycastHit info, 10f, 1 << 11))
                    {
                        Vector3 newForward = Vector3.Reflect(transform.forward.normalized, info.normal);
                        transform.forward = new Vector3(newForward.x, transform.forward.y, newForward.z);
                    }

                    currentPenetration--;
                }
            }
            else
            {
                ResetAndReturn();
            }
        }
    }
}
