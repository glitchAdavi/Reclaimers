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
    public event Action<bool> onRequestUIUseMainMenu;
    public void RequestUIUseMainMenu(bool useMainMenu) => onRequestUIUseMainMenu?.Invoke(useMainMenu);

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


    public event Action<string, bool, bool> onRequestUIUpdateInteractText;
    public void RequestUIUpdateInteractText(string t, bool enable, bool hasKey = true) => onRequestUIUpdateInteractText?.Invoke(t, enable, hasKey);


    public event Action<float, float, bool> onRequestUIUpdateInteractFill;
    public void RequestUIUpdateInteractFill(float current, float max, bool enable) => onRequestUIUpdateInteractFill?.Invoke(current, max, enable);


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

    public event Action<Vector3, string, Color, float, float> onRequestUISpawnFloatingText;
    public void RequestUISpawnFloatingText(Vector3 pos, string text, Color c, float driftRange, float duration) => onRequestUISpawnFloatingText?.Invoke(pos, text, c, driftRange, duration);


    #endregion

    #region XP
    public event Action<Vector3, float> onSpawnXp;
    public void SpawnXp(Vector3 pos, float v) => onSpawnXp?.Invoke(pos, v);

    public event Action<float> onGivePlayerXp;
    public void GivePlayerXp(float v) => onGivePlayerXp?.Invoke(v);

    public event Action<int> onGivePlayerLevel;
    public void GivePlayerLevel(int levels) => onGivePlayerLevel?.Invoke(levels);
    #endregion

    #region Material
    public event Action<Vector3, float> onSpawnMaterial;
    public void SpawnMaterial(Vector3 pos, float v) => onSpawnMaterial?.Invoke(pos, v);

    public event Action<float> onGivePlayerMaterial;
    public void GivePlayerMaterial(float v) => onGivePlayerMaterial?.Invoke(v);
    #endregion


    #region Upgrades
    public event Action<Vector3> onSpawnPawnUpgrade;
    public void SpawnPawnUpgrade(Vector3 pos) => onSpawnPawnUpgrade?.Invoke(pos);

    public event Action<Vector3> onSpawnWeaponUpgrade;
    public void SpawnWeaponUpgrade(Vector3 pos) => onSpawnWeaponUpgrade?.Invoke(pos);

    public event Action<Vector3> onSpawnRandomUpgrade;
    public void SpawnRandomUpgrade(Vector3 pos) => onSpawnRandomUpgrade?.Invoke(pos);



    #endregion


    #region Enemy
    public event Action<bool> onPawnServiceActive;
    public void SetPawnServiceActive(bool active) => onPawnServiceActive?.Invoke(active);

    public event Action<bool> onPawnServiceIdle;
    public void SetPawnServiceIdle(bool idle) => onPawnServiceIdle?.Invoke(idle);

    public event Action<EnemyPawn> onEnemyDeath;
    public void EnemyDeath(EnemyPawn ep) => onEnemyDeath?.Invoke(ep);

    #endregion
}
