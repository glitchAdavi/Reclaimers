using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IPause
{
    public WeaponStatBlock statBlock;

    public int maxClipSize = 7;
    public int currentClipSize = 0;
    public int bulletsPerShot = 1;

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

        currentClipSize -= bulletsPerShot;
        GameManager.current.eventService.RequestUIUpdateWeaponAmmo(currentClipSize, maxClipSize);

        canShoot = false;
        timerFireRate = GameManager.current.timerService.StartTimer(fireRate, ShootEndTimer);

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
        ApplyReloadTime();
        ApplyFireRate();



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

    public void ApplyReloadTime()
    {
        reloadTime = statBlock.reloadTime.Value();
        GameManager.current.eventService.RequestUIUpdateWeaponReloadSetMax(reloadTime);
    }

    public void ApplyFireRate()
    {
        fireRate = statBlock.fireRate.Value();
    }

    #endregion
}
