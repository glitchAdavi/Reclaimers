using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IUpdate, IPause
{
    public PlayablePawn owner;

    public WeaponStatBlock baseStatBlock;
    public WeaponStatBlock statBlock;

    public int maxClipSize = 7;
    public int currentClipSize = 0;
    public int bulletsPerShot = 1;
    public int bulletsPerShotCost = 1;
    public float bulletPerShotSpread = 0f;
    public float bulletSpreadMax = 0f;
    public float bulletSpread = 0f;
    public float bulletSpreadGain = 0f;
    Timer timerBulletSpreadReset;

    public float reloadTime = 3f;
    Timer timerReload;

    public float fireRate = 0.1f;
    Timer timerFireRate;

    public bool automatic = false;

    public bool isReloading = false;

    public bool canShoot = true;

    protected bool isPaused = false;

    protected virtual void OnEnable()
    {
        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
    }

    public void ExecuteUpdate()
    {
        if (isPaused) return;

        
    }

    public void Pause(bool paused)
    {
        timerReload?.Pause(paused);
        timerFireRate?.Pause(paused);
    }

    protected virtual void OnDisable()
    {
        GameManager.current.updateService.UnregisterUpdate(this);
        GameManager.current.updateService.UnregisterPause(this);
    }

    public virtual void Shoot()
    {
        if (!canShoot) return;

        if (timerReload != null && currentClipSize > 0) {
            timerReload.Cancel();
            timerReload = null;
            isReloading = false;
        }

        if (currentClipSize <= 0) {
            if (!isReloading) Reload();
            return;
        }

        currentClipSize -= bulletsPerShotCost;
        GameManager.current.eventService.RequestUIUpdateWeaponAmmo(currentClipSize, maxClipSize);
        GameManager.current.eventService.RequestUIUpdateWeaponSlider(currentClipSize, maxClipSize);

        canShoot = false;
        owner.MuzzleFlash();
        timerFireRate = GameManager.current.timerService.StartTimer(fireRate, ShootEndTimer);

        if (bulletSpreadMax > 0)
        {
            if (timerBulletSpreadReset != null) timerBulletSpreadReset.Cancel();
            bulletSpread += bulletSpreadGain;
            if (bulletSpread > bulletSpreadMax) bulletSpread = bulletSpreadMax;
            timerBulletSpreadReset = GameManager.current.timerService.StartTimer(0.5f, () => bulletSpread = 0f);
        }

        ShootEffect();
    }

    protected virtual void ShootEffect() { }

    public virtual void ShootEndTimer()
    {
        canShoot = true;
    }

    public virtual void Reload()
    {
        if (isReloading) return;

        isReloading = true;

        GameManager.current.eventService.RequestUIUpdateWeaponSlider(0f, maxClipSize);
        timerReload = GameManager.current.timerService.StartTimer(reloadTime, ReloadEffect, Time.fixedDeltaTime, ReloadPartial);

        GameManager.current.eventService.RequestUISpawnFloatingText(transform.position,
                                                                    "Reloading!",
                                                                    Color.green,
                                                                    0f,
                                                                    1f);
    }

    protected virtual void ReloadPartial()
    {
        GameManager.current.eventService.RequestUIUpdateWeaponSlider((timerReload.lifeTime / reloadTime) * maxClipSize, maxClipSize);
    }

    protected virtual void ReloadEffect()
    {
        isReloading = false;
        currentClipSize = maxClipSize;

        GameManager.current.eventService.RequestUIUpdateWeaponAmmo(currentClipSize, maxClipSize);
        GameManager.current.eventService.RequestUIUpdateWeaponSlider(currentClipSize, maxClipSize);
    }


    #region ApplyValues
    public virtual void FirstStatApplication()
    {
        statBlock = ScriptableObject.CreateInstance<WeaponStatBlock>();
        statBlock.CopyValues(baseStatBlock);

        ApplyWeaponUpgrades();

        ApplyClipSize();
        ApplyBulletsPerShot();
        ApplyBulletsPerShotCost();
        ApplyBulletsPerShotSpread();
        ApplyBulletSpread();
        ApplyBulletSpreadGain();
        ApplyReloadTime();
        ApplyFireRate();
        ApplyAutomatic();

        GameManager.current.SetNewProjectile(statBlock.projectilePrefab);

        currentClipSize = maxClipSize;

        GameManager.current.eventService.RequestUIWeaponShow(true);
        GameManager.current.eventService.RequestUIUpdateWeaponName(statBlock.weaponName);
        GameManager.current.eventService.RequestUIUpdateWeaponAmmo(currentClipSize, maxClipSize);
        GameManager.current.eventService.RequestUIUpdateWeaponSlider(currentClipSize, maxClipSize);
    }

    public void ApplyClipSize()
    {
        maxClipSize = statBlock.clipSize.ValueInt();
        GameManager.current.eventService.RequestUIUpdateWeaponAmmo(currentClipSize, maxClipSize);
    }

    public void ApplyBulletsPerShot()
    {
        bulletsPerShot = statBlock.bulletPerShot.ValueInt();
    }

    public void ApplyBulletsPerShotCost()
    {
        bulletsPerShotCost = statBlock.bulletPerShotCost.ValueInt();
    }

    public void ApplyBulletsPerShotSpread()
    {
        bulletPerShotSpread = statBlock.bulletPerShotSpread.Value();
    }

    public void ApplyBulletSpread()
    {
        bulletSpreadMax = statBlock.bulletSpread.Value();
    }

    public void ApplyBulletSpreadGain()
    {
        bulletSpreadGain = statBlock.bulletSpreadGain.Value();
    }

    public void ApplyReloadTime()
    {
        reloadTime = statBlock.reloadTime.Value();
        GameManager.current.eventService.RequestUIUpdateWeaponSlider(currentClipSize, maxClipSize);
    }

    public void ApplyFireRate()
    {
        fireRate = statBlock.fireRate.Value();
    }

    public void ApplyAutomatic()
    {
        automatic = statBlock.automatic;
    }

    public void ApplyWeaponUpgrades()
    {
        PlayablePawn owner = GetComponent<PlayablePawn>();

        foreach (KeyValuePair<string, int> kvp in owner.appliedUpgrades)
        {
            if (owner.GetUpgradeByName(kvp.Key) is WeaponUpgrade)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    owner.GetUpgradeByName(kvp.Key).Apply(owner);
                }
            }
        }
    }

    #endregion
}
