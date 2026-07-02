using Reclaimers.Systems;
using UnityEngine;

public enum WeaponCollection
{
    TEST
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private Pawn owner;

    [SerializeField] int currentBullets = 0;
    [SerializeField] float bulletSpreadCurrent = 0f;

    Timer bulletSpreadResetTimer;
    Timer reloadTimer;
    Timer fireRateTimer;
    Timer burstTimer;

    [SerializeField] bool canShoot = true;
    [SerializeField] bool isReloading = false;

    Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, 0.25f, 0f));

    public void Shoot()
    {
        if (!canShoot) return;

        if (reloadTimer != null && currentBullets > 0)
        {
            owner.CancelTimer(ref reloadTimer);
            isReloading = false;
        }

        if (currentBullets <= 0)
        {
            if (!isReloading) Reload();
            return;
        }

        canShoot = false;

        ShootEffect();
        
        if (owner.weaponBlock.burstAmount.ValueInt() > 1)
        {
            float burstCount = (owner.weaponBlock.burstAmount.ValueInt() - 0.5f) * owner.weaponBlock.burstDelay.Value();

            if (owner.weaponBlock.burstAmount.ValueInt() * owner.weaponBlock.bulletsPerShotCost.Value() > currentBullets)
            {
                burstCount = Mathf.FloorToInt(currentBullets / owner.weaponBlock.bulletsPerShotCost.Value()) * owner.weaponBlock.burstDelay.Value();
            }
            burstTimer = GameManager.Instance.timerSystem.SetTimer(burstCount,
                     () => {
                         fireRateTimer = GameManager.Instance.timerSystem.SetTimer(owner.weaponBlock.fireRate.Value(), ShootEnd, "WEAPON_FireRate");
                     },
                     owner.weaponBlock.burstDelay.Value(),
                     ShootEffect,
                     "WEAPON_Burst");
        } else
        {
            fireRateTimer = GameManager.Instance.timerSystem.SetTimer(owner.weaponBlock.fireRate.Value(), ShootEnd, "WEAPON_FireRate");
        }
    }

    public void ShootEffect()
    {
        currentBullets -= owner.weaponBlock.bulletsPerShotCost.ValueInt();

        if (currentBullets < 0) currentBullets = 0;

        int bulletsToFire = owner.weaponBlock.bulletsPerShot.ValueInt();

        Vector3 tempForward = Vector3.zero;
        Ray ray = GameManager.Instance.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (aimPlane.Raycast(ray, out float enterDistance))
        {
            tempForward = ray.GetPoint(enterDistance) - owner.transform.position;

            InstantiateProjectile(tempForward, Random.Range(-bulletSpreadCurrent / 2, bulletSpreadCurrent / 2));

            bulletsToFire--;
        }

        while(bulletsToFire > 0)
        {
            InstantiateProjectile(tempForward, Random.Range(-owner.weaponBlock.bulletsPerShotSpread.Value() / 2, owner.weaponBlock.bulletsPerShotSpread.Value() / 2));

            bulletsToFire--;
        }

        if (owner.weaponBlock.bulletSpread.Value() > 0)
        {
            if (bulletSpreadResetTimer != null) owner.CancelTimer(ref bulletSpreadResetTimer);
            bulletSpreadCurrent += owner.weaponBlock.bulletSpreadGain.Value();
            if (bulletSpreadCurrent > owner.weaponBlock.bulletSpread.Value()) bulletSpreadCurrent = owner.weaponBlock.bulletSpread.Value();
            bulletSpreadResetTimer = GameManager.Instance.timerSystem.SetTimer(0.5f, () => bulletSpreadCurrent = 0f, "WEAPON_BulletSpreadReset");
        }
    }

    public void ShootEnd()
    {
        canShoot = true;
    }




    public void Reload()
    {
        if (isReloading) return;
        if (currentBullets >= owner.weaponBlock.magSize.ValueInt()) return;

        isReloading = true;
        reloadTimer = GameManager.Instance.timerSystem.SetTimer(owner.weaponBlock.reloadTime.Value(), ReloadEnd, "WEAPON_Reload");

    }

    public void ReloadEffect()
    {

    }

    public void ReloadEnd()
    {
        isReloading = false;
        currentBullets = owner.weaponBlock.magSize.ValueInt();
    }



    public void InstantiateProjectile(Vector3 forw, float spread)
    {
        Projectile proj = owner.projectileBuilder.GetObject();
        proj.transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
        proj.transform.forward = forw;
        proj.transform.Rotate(Vector3.up, spread);
        proj.SetShooter(owner);
    }

    public void SetOwner(Pawn p)
    {
        owner = p;
        currentBullets = owner.weaponBlock.magSize.ValueInt();
    }
}
