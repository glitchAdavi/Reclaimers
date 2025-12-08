using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P_Explosive : Projectile
{
    public bool stuck = false;
    public bool armed = false;

    Timer timerWallExplosion;

    protected override void OnEnable()
    {
        stuck = false;
        armed = false;
        timerWallExplosion = null;

        base.OnEnable();
    }

    public override void ExecuteUpdate()
    {
        if (active)
        {
            if (!stuck) Move();

            LookAtCamera();

            if (!stuck && currentLifetime > armingLifetime)
            {
                armed = true;
                _sprt.sprite = projSpriteAux;
                _lgt.color = projSpriteColorAux;
                if (explodeImmediately) Explode();
            }
        }
    }

    public void Explode()
    {
        Explosion e = Instantiate(GameManager.current.gameInfo.explosionPrefab, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity).GetComponent<Explosion>();
        e.Init(damage, explosionRadius);
        e.Explode();
        ResetAndReturn();
    }

    public override void ManageOnCollisionEnter(Collision collider)
    {
        if (collider.gameObject.layer == 22) //LevelCollision
        {
            SeparateFromWall(collider.collider, collider.transform.position, collider.transform.rotation);

            if (armed)
            {
                Explode();
            } else
            {
                stuck = true;
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

    public override void ProjectileSetup(float projScaleVar,
                                float damageVar,
                                float damageRadiusVar,
                                float speedVar,
                                float critChanceVar,
                                float critMultiplierVar,
                                int penetrationVar,
                                bool useDistanceVar,
                                float maxLifetimeVar,
                                float knockbackVar,
                                float armingLifetimeVar,
                                bool explodeImmediately,
                                float explosionRadiusVar,
                                Sprite projSprite,
                                Color projColor,
                                Sprite projSpriteAux,
                                Color projColorAux,
                                Color hitColor,
                                Sprite hitSprite = null)
    {
        scale = projScaleVar;
        _sprt.transform.localScale = _sprt.transform.localScale * projScaleVar;
        _damageCollider.transform.localScale = _damageCollider.transform.localScale * projScaleVar;
        _tr.startWidth = _tr.startWidth * projScaleVar;

        _lgt.enabled = true;
        _lgt.color = projColor;

        damage = damageVar;
        damageRadius = damageRadiusVar;
        speed = speedVar;
        critChance = critChanceVar;
        critMult = critMultiplierVar;
        penetration = penetrationVar;
        useDistance = useDistanceVar;
        maxLifetime = maxLifetimeVar;
        knockback = knockbackVar;
        armingLifetime = armingLifetimeVar;
        this.explodeImmediately = explodeImmediately;
        explosionRadius = explosionRadiusVar;

        if (projSprite != null) _sprt.sprite = projSprite;
        _sprt.color = projColor;

        if (projSpriteAux != null) this.projSpriteAux = projSpriteAux;
        projSpriteColorAux = projColorAux;

        this.hitColor = hitColor;
        this.hitSprite = hitSprite;

        currentPenetration = penetration;
    }
}
