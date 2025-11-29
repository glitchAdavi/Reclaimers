using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Explosive : Projectile
{
    public bool stuck = false;
    public bool armed = false;

    Timer timerWallExplosion;

    public override void ExecuteUpdate()
    {
        if (active)
        {
            Move();

            LookAtCamera();

            if (!stuck)
            {
                if (currentDistance > armingDistance || currentLifetime > armingLifetime)
                {
                    armed = true;
                    if (explodeImmediately) Explode();
                }
            }
        }
    }

    public void Explode()
    {
        Debug.Log("BOOM!");
        ResetAndReturn();
    }

    public override void ManageOnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.layer == 22) //LevelCollision
        {
            if (armed)
            {
                Explode();
            } else
            {
                stuck = true;
                speed = 0f;
                if (timerWallExplosion == null) timerWallExplosion = GameManager.current.timerService.StartTimer(1f, Explode);
            }
        }
    }

    public override void ManageOnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 && other.gameObject.name == "DamageCollider") //Enemy
        {
            Pawn enemyHit = GetPawnFromCollision(other);

            if (!armed)
            {
                if (currentPenetration >= 0)
                {
                    if (!hitEnemies.Contains(enemyHit.gameObject))
                    {
                        hitEnemies.Add(enemyHit.gameObject);
                        if (CheckIfCrit()) enemyHit.GetHit(damage * critMult, true, knockback * critMult, transform.forward);
                        else enemyHit.GetHit(damage, false, knockback, transform.forward);
                        currentPenetration--;
                    }
                } else
                {
                    Explode();
                }
            } 
            else
            {
                Explode();
            }
        }
    }
}
