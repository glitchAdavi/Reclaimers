using Reclaimers.Systems;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Pawn p;

    [SerializeField] int currentBullets = 0;
    [SerializeField] float bulletSpreadCurrent = 0f;
    [SerializeField] int targetLayer = 11;

    Timer bulletSpreadResetTimer;
    Timer reloadTimer;
    Timer fireRateTimer;
    Timer burstTimer;

    [SerializeField] bool canShoot = true;
    [SerializeField] bool isReloading = false;

    Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, 0.25f, 0f));

    [SerializeField] protected Vector3? overrideForward = null;

    public void Shoot(Vector3? overrideForward = null)
    {
        if (!canShoot) return;

        this.overrideForward = overrideForward;

        if (reloadTimer != null && currentBullets > 0)
        {
            p.CancelTimer(ref reloadTimer);
            isReloading = false;
        }

        if (currentBullets <= 0)
        {
            if (!isReloading) Reload();
            return;
        }

        canShoot = false;

        ShootEffect();
        
        if (p.statBlock.burstAmount.ValueInt() > 1)
        {
            float burstCount = (p.statBlock.burstAmount.ValueInt() - 0.5f) * p.statBlock.burstDelay.Value();

            if (p.statBlock.burstAmount.ValueInt() * p.statBlock.bulletsPerShotCost.Value() > currentBullets)
            {
                burstCount = Mathf.FloorToInt(currentBullets / p.statBlock.bulletsPerShotCost.Value()) * p.statBlock.burstDelay.Value();
            }
            burstTimer = GameManager.Instance.timerSystem.SetTimer(burstCount,
                     () => {
                         fireRateTimer = GameManager.Instance.timerSystem.SetTimer(p.statBlock.fireRate.Value(), ShootEnd, "WEAPON_FireRate");
                     },
                     p.statBlock.burstDelay.Value(),
                     ShootEffect,
                     "WEAPON_Burst");
        } else
        {
            fireRateTimer = GameManager.Instance.timerSystem.SetTimer(p.statBlock.fireRate.Value(), ShootEnd, "WEAPON_FireRate");
        }
    }

    public void ShootEffect()
    {
        currentBullets -= p.statBlock.bulletsPerShotCost.ValueInt();
        if (currentBullets < 0) currentBullets = 0;
        if (p.isPlayer) GameManager.Instance.eventSystem.UpdateWeapon(currentBullets, p.statBlock.magSize.ValueInt());

        int bulletsToFire = p.statBlock.bulletsPerShot.ValueInt();

        Vector3 tempForward = Vector3.zero;
        if (overrideForward == null)
        {
            Ray ray = GameManager.Instance.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            if (aimPlane.Raycast(ray, out float enterDistance))
            {
                tempForward = ray.GetPoint(enterDistance) - p.transform.position;

                InstantiateProjectile(tempForward, Random.Range(-bulletSpreadCurrent / 2, bulletSpreadCurrent / 2));

                bulletsToFire--;
            }
        } else
        {
            tempForward = (Vector3)overrideForward;

            InstantiateProjectile(tempForward, Random.Range(-bulletSpreadCurrent / 2, bulletSpreadCurrent / 2));

            bulletsToFire--;
        }


        while (bulletsToFire > 0)
        {
            InstantiateProjectile(tempForward, Random.Range(-p.statBlock.bulletsPerShotSpread.Value() / 2, p.statBlock.bulletsPerShotSpread.Value() / 2));

            bulletsToFire--;
        }

        if (p.statBlock.bulletSpread.Value() > 0)
        {
            if (bulletSpreadResetTimer != null) p.CancelTimer(ref bulletSpreadResetTimer);
            bulletSpreadCurrent += p.statBlock.bulletSpreadGain.Value();
            if (bulletSpreadCurrent > p.statBlock.bulletSpread.Value()) bulletSpreadCurrent = p.statBlock.bulletSpread.Value();
            bulletSpreadResetTimer = GameManager.Instance.timerSystem.SetTimer(0.5f, () => bulletSpreadCurrent = 0f, "WEAPON_BulletSpreadReset");
        }

        overrideForward = null;
    }

    public void ShootEnd()
    {
        canShoot = true;
    }




    public void Reload()
    {
        if (isReloading) return;
        if (currentBullets >= p.statBlock.magSize.ValueInt()) return;

        isReloading = true;
        reloadTimer = GameManager.Instance.timerSystem.SetTimer(p.statBlock.reloadTime.Value(), ReloadEnd, "WEAPON_Reload");

    }

    public void ReloadEffect()
    {

    }

    public void ReloadEnd()
    {
        isReloading = false;
        currentBullets = p.statBlock.magSize.ValueInt();
        GameManager.Instance.eventSystem.UpdateWeapon(currentBullets, p.statBlock.magSize.ValueInt());
    }



    public void InstantiateProjectile(Vector3 forw, float spread)
    {
        Projectile proj = p.projectileBuilder.GetObject();
        proj.transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
        proj.transform.forward = forw;
        proj.transform.Rotate(Vector3.up, spread);
        proj.SetTargetLayer(targetLayer);
        proj.SetShooter(p);
    }

    public void SetOwner(Pawn p)
    {
        this.p = p;
        currentBullets = this.p.statBlock.magSize.ValueInt();
        if (p.isPlayer)
        {
            GameManager.Instance.eventSystem.UpdateWeaponName(p.statBlock.weaponName);
            GameManager.Instance.eventSystem.UpdateWeapon(currentBullets, p.statBlock.magSize.ValueInt());
        }
    }

    public void SetTargetLayer(int targetLayer)
    {
        this.targetLayer = targetLayer;
    }
}
