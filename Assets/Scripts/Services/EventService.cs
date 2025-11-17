using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventService : MonoBehaviour
{
    public static EventService current { get; private set; }

    private void Awake()
    {
        if (current == null) current = this;
    }









    #region UI
    public event Action<bool> onRequestUITogglePauseMenu;
    public void RequestUITogglePauseMenu(bool paused) => onRequestUITogglePauseMenu?.Invoke(paused);

    public event Action<float, float> onRequestUIUpdateHealth;
    public void RequestUIUpdateHealth(float hp, float max) => onRequestUIUpdateHealth?.Invoke(hp, max);


    public event Action<int> onRequestUIUpdateKillCount;
    public void RequestUIUpdateKillCount(int kc) => onRequestUIUpdateKillCount?.Invoke(kc);


    public event Action<float, int> onRequestUIUpdateXpBar;
    public void RequestUIUpdateXpBar(float xp, int threshold) => onRequestUIUpdateXpBar?.Invoke(xp, threshold);


    public event Action<int> onRequestUIUpdateLevelCounter;
    public void RequestUIUpdateLevelCounter(int level) => onRequestUIUpdateLevelCounter?.Invoke(level);


    public event Action<string> onRequestUIUpdateInteractText;
    public void RequestUIUpdateInteractText(string t) => onRequestUIUpdateInteractText?.Invoke(t);


    public event Action onRequestUIUpdateInteractFill;
    public void RequestUIUpdateInteractFill() => onRequestUIUpdateInteractFill?.Invoke();


    public event Action<int, int> onRequestUIUpdateWeaponAmmo;
    public void RequestUIUpdateWeaponAmmo(int current, int max) => onRequestUIUpdateWeaponAmmo?.Invoke(current, max);

    public event Action<float> onRequestUIUpdateWeaponReloadSetMax;
    public void RequestUIUpdateWeaponReloadSetMax(float max) => onRequestUIUpdateWeaponReloadSetMax?.Invoke(max);

    public event Action onRequestUIUpdateWeaponReloadReset;
    public void RequestUIUpdateWeaponReloadReset() => onRequestUIUpdateWeaponReloadReset?.Invoke();

    public event Action onRequestUIUpdateWeaponReloadTimer;
    public void RequestUIUpdateWeaponReloadTimer() => onRequestUIUpdateWeaponReloadTimer?.Invoke();

    public event Action onRequestUIUpdateWeaponReloadEnd;
    public void RequestUIUpdateWeaponReloadEnd() => onRequestUIUpdateWeaponReloadEnd?.Invoke();


    #endregion

    #region XP
    public event Action<Vector3, float> onSpawnXp;
    public void SpawnXp(Vector3 pos, float v) => onSpawnXp?.Invoke(pos, v);

    public event Action<float> onGivePlayerXp;
    public void GivePlayerXp(float v) => onGivePlayerXp?.Invoke(v);

    public event Action<int> onGivePlayerLevel;
    public void GivePlayerLevel(int levels) => onGivePlayerLevel?.Invoke(levels);


    #endregion


    #region Enemy
    public event Action<bool> onPawnServiceActive;
    public void SetPawnServiceActive(bool active) => onPawnServiceActive?.Invoke(active);

    public event Action<EnemyPawn> onEnemyDeath;
    public void EnemyDeath(EnemyPawn ep) => onEnemyDeath?.Invoke(ep);

    public event Action<Vector3, float, bool> onEnemyHurt;
    public void EnemyHurt(Vector3 pos, float damage, bool isCrit) => onEnemyHurt?.Invoke(pos, damage, isCrit);


    #endregion
}
