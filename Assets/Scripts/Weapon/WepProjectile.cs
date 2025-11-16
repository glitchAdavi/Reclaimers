using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WepProjectile : Weapon
{
    private Ray myRay;

    protected override void ShootEffect()
    {
        GameObject newBullet = GameManager.current.projectileBuilder.GetObject().gameObject;
        newBullet.transform.position = GameManager.current.gameInfo.playerPositionVar.Value;

        Ray ray = GameManager.current.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit info, 100f, 1 << 20))
        {
            Debug.DrawLine(transform.position, info.point, Color.red, 0.5f);

            newBullet.transform.forward = info.point - GameManager.current.gameInfo.playerPositionVar.Value;
            newBullet.transform.GetComponent<Projectile>().ProjectileSetup(statBlock.projDamage.Value(),
                                                                           statBlock.projSpeed.Value(),
                                                                           statBlock.projPenetration.ValueInt(),
                                                                           statBlock.projUseDistance,
                                                                           statBlock.projMaxDistance.Value(),
                                                                           statBlock.projMaxLifetime.Value(),
                                                                           statBlock.knockback.Value(),
                                                                           Color.white);


            
        }
    }
}
