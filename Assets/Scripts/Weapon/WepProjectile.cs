using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WepProjectile : Weapon
{
    private Ray myRay;

    protected override void ShootEffect()
    {
        if (bulletsPerShot <= 0) return;
        int bulletsLeft = bulletsPerShot;
        Vector3 tempForward = Vector3.zero;

        Ray ray = GameManager.current.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit info, 100f, 1 << 20))
        {
            Debug.DrawLine(transform.position, info.point, Color.red, 0.5f);

            tempForward = info.point - GameManager.current.gameInfo.playerPositionVar.Value;

            GameObject newBullet = GameManager.current.projectileBuilder.GetObject().gameObject;
            newBullet.transform.position = GameManager.current.gameInfo.playerPositionVar.Value;

            newBullet.transform.forward = tempForward;
            if (bulletSpreadMax > 0 && bulletSpread > 0)
            {
                newBullet.transform.Rotate(Vector3.up, Random.Range(-bulletSpread / 2, bulletSpread / 2));
            }
            newBullet.transform.GetComponent<Projectile>().ProjectileSetup(statBlock.projDamage.Value(),
                                                                           statBlock.projDamageRadius.Value(),
                                                                           statBlock.projSpeed.Value(),
                                                                           statBlock.projCritChance.Value(),
                                                                           statBlock.projCritMultiplier.Value(),
                                                                           statBlock.projPenetration.ValueInt(),
                                                                           statBlock.projUseDistance,
                                                                           statBlock.projMaxDistance.Value(),
                                                                           statBlock.projMaxLifetime.Value(),
                                                                           statBlock.knockback.Value(),
                                                                           statBlock.projArmingDistance.Value(),
                                                                           statBlock.projArmingLifetime.Value(),
                                                                           statBlock.explodeImmediately,
                                                                           statBlock.projectileSprite,
                                                                           statBlock.projectileSpriteColor,
                                                                           statBlock.hitSpriteColor,
                                                                           statBlock.hitSprite);

            bulletsLeft--;
        }

        while (bulletsLeft > 0)
        {
            GameObject newBullet = GameManager.current.projectileBuilder.GetObject().gameObject;
            newBullet.transform.position = GameManager.current.gameInfo.playerPositionVar.Value;

            newBullet.transform.forward = tempForward;
            newBullet.transform.Rotate(Vector3.up, Random.Range(-bulletPerShotSpread / 2, bulletPerShotSpread / 2));
            newBullet.transform.GetComponent<Projectile>().ProjectileSetup(statBlock.projDamage.Value(),
                                                                           statBlock.projDamageRadius.Value(),
                                                                           statBlock.projSpeed.Value(),
                                                                           statBlock.projCritChance.Value(),
                                                                           statBlock.projCritMultiplier.Value(),
                                                                           statBlock.projPenetration.ValueInt(),
                                                                           statBlock.projUseDistance,
                                                                           statBlock.projMaxDistance.Value(),
                                                                           statBlock.projMaxLifetime.Value(),
                                                                           statBlock.knockback.Value(),
                                                                           statBlock.projArmingDistance.Value(),
                                                                           statBlock.projArmingLifetime.Value(),
                                                                           statBlock.explodeImmediately,
                                                                           statBlock.projectileSprite,
                                                                           statBlock.projectileSpriteColor,
                                                                           statBlock.hitSpriteColor,
                                                                           statBlock.hitSprite);

            bulletsLeft--;
        }
    }
}
