using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPause
{
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
        GameManager.current.updateService.RegisterPause(this);
    }

    public void Pause(bool paused)
    {
        timerReload?.Pause(paused);
        timerFireRate?.Pause(paused);
    }

    protected virtual void OnDisable()
    {
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

        canShoot = false;
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
        if (isReloading)
        {
            timerReload.Cancel();
            GameManager.current.eventService.RequestUIUpdateWeaponReloadReset();
        }

        isReloading = true;
        GameManager.current.eventService.RequestUIUpdateWeaponReloadReset();
        GameManager.current.eventService.RequestUIUpdateWeaponReloadTimer();
        timerReload = GameManager.current.timerService.StartTimer(reloadTime, ReloadEffect, Time.fixedDeltaTime, GameManager.current.eventService.RequestUIUpdateWeaponReloadTimer);
    }

    protected virtual void ReloadEffect()
    {
        isReloading = false;
        currentClipSize = maxClipSize;

        GameManager.current.eventService.RequestUIUpdateWeaponAmmo(currentClipSize, maxClipSize);
        GameManager.current.eventService.RequestUIUpdateWeaponReloadEnd();
    }


    #region ApplyValues
    public virtual void FirstStatApplication()
    {
        ApplyClipSize();
        ApplyBulletsPerShot();
        ApplyBulletsPerShotCost();
        ApplyBulletsPerShotSpread();
        ApplyBulletSpread();
        ApplyBulletSpreadGain();
        ApplyReloadTime();
        ApplyFireRate();
        ApplyAutomatic();


        currentClipSize = maxClipSize;
        GameManager.current.eventService.RequestUIUpdateWeaponAmmo(currentClipSize, maxClipSize);
    }

    public void ApplyClipSize()
    {
        maxClipSize = statBlock.clipSize.ValueInt();
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
        GameManager.current.eventService.RequestUIUpdateWeaponReloadSetMax(reloadTime);
    }

    public void ApplyFireRate()
    {
        fireRate = statBlock.fireRate.Value();
    }

    public void ApplyAutomatic()
    {
        automatic = statBlock.automatic;
    }

    #endregion
}
