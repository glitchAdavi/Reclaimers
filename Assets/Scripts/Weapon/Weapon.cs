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

    Timer timerBulletSpreadReset;
    Timer timerReload;
    Timer timerFireRate;
    Timer timerBurst;

    [SerializeField] bool canShoot = true;
    [SerializeField] bool isReloading = false;

    Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, 0.25f, 0f));

    public void Shoot()
    {
        if (!canShoot) return;

        if (timerReload != null && currentBullets > 0)
        {
            owner.CancelTimer(ref timerReload);
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
            timerBurst = GameManager.Instance.timerSystem.SetTimer(burstCount,
                     () => {
                         timerFireRate = GameManager.Instance.timerSystem.SetTimer(owner.weaponBlock.fireRate.Value(), ShootEnd);
                     },
                     owner.weaponBlock.burstDelay.Value(),
                     ShootEffect);
        } else
        {
            timerFireRate = GameManager.Instance.timerSystem.SetTimer(owner.weaponBlock.fireRate.Value(), ShootEnd);
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

            Projectile proj = owner.projectileBuilder.GetObject();
            proj.transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
            proj.transform.forward = tempForward;
            proj.transform.Rotate(Vector3.up, Random.Range(-bulletSpreadCurrent / 2, bulletSpreadCurrent / 2));
            proj.SetShooter(owner);

            bulletsToFire--;
        }

        while(bulletsToFire > 0)
        {
            Projectile proj = owner.projectileBuilder.GetObject();
            proj.transform.position = new Vector3(transform.position.x, 0.25f, transform.position.z);
            proj.transform.forward = tempForward;
            proj.transform.Rotate(Vector3.up, Random.Range(-owner.weaponBlock.bulletsPerShotSpread.Value() / 2, owner.weaponBlock.bulletsPerShotSpread.Value() / 2));
            proj.SetShooter(owner);

            bulletsToFire--;
        }

        if (owner.weaponBlock.bulletSpread.Value() > 0)
        {
            if (timerBulletSpreadReset != null) owner.CancelTimer(ref timerBulletSpreadReset);
            bulletSpreadCurrent += owner.weaponBlock.bulletSpreadGain.Value();
            if (bulletSpreadCurrent > owner.weaponBlock.bulletSpread.Value()) bulletSpreadCurrent = owner.weaponBlock.bulletSpread.Value();
            timerBulletSpreadReset = GameManager.Instance.timerSystem.SetTimer(0.5f, () => bulletSpreadCurrent = 0f);
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
        timerReload = GameManager.Instance.timerSystem.SetTimer(owner.weaponBlock.reloadTime.Value(), ReloadEnd);

    }

    public void ReloadEffect()
    {

    }

    public void ReloadEnd()
    {
        isReloading = false;
        currentBullets = owner.weaponBlock.magSize.ValueInt();
    }





    public void SetOwner(Pawn p)
    {
        owner = p;
        currentBullets = owner.weaponBlock.magSize.ValueInt();
    }
}
