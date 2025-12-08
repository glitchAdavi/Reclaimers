using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Basic : Projectile
{
    public override void ManageOnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.layer == 22) //LevelCollision
        {
            SeparateFromWall(collider.collider, collider.transform.position, collider.transform.rotation);
            if (hitSprite != null) Instantiate(GameManager.current.gameInfo.weaponHitPrefab, transform.position, Quaternion.identity).GetComponent<WeaponHit>().Activate(hitSprite, hitColor);
            ResetAndReturn();
        }
    }

    public override void ManageOnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && other.gameObject.name == "DamageCollider") //Enemy
        {
            Pawn enemyHit = GetPawnFromCollision(other);

            if (currentPenetration >= 0)
            {
                if (!hitEnemies.Contains(enemyHit.gameObject))
                {
                    hitEnemies.Add(enemyHit.gameObject);
                    if (CheckIfCrit()) enemyHit.GetHit(damage * critMult, true, knockback * critMult, transform.forward);
                    else enemyHit.GetHit(damage, false, knockback, transform.forward);
                    currentPenetration--;
                }
            }
            else
            {
                if (hitSprite != null) Instantiate(GameManager.current.gameInfo.weaponHitPrefab, transform.position, Quaternion.identity).GetComponent<WeaponHit>().Activate(hitSprite, hitColor);
                ResetAndReturn();
            }
        }
    }
}
