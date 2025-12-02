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


    public event Action<bool> onRequestUIMapProgressionEnable;
    public void RequestUIMapProgressionEnable(bool enabled) => onRequestUIMapProgressionEnable?.Invoke(enabled);


    public event Action<float> onRequestUIMapProgression;
    public void RequestUIMapProgression(float current) => onRequestUIMapProgression?.Invoke(current);


    public event Action<float> onRequestUIMapProgressionSetup;
    public void RequestUIMapProgressionSetup(float max) => onRequestUIMapProgressionSetup?.Invoke(max);


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


    public event Action<bool> onRequestUIWeaponShow;
    public void RequestUIWeaponShow(bool show) => onRequestUIWeaponShow?.Invoke(show);


    public event Action<string> onRequestUIUpdateWeaponName;
    public void RequestUIUpdateWeaponName(string newName) => onRequestUIUpdateWeaponName?.Invoke(newName);


    public event Action<int, int> onRequestUIUpdateWeaponAmmo;
    public void RequestUIUpdateWeaponAmmo(int current, int max) => onRequestUIUpdateWeaponAmmo?.Invoke(current, max);


    public event Action<float, float> onRequestUIUpdateWeaponSlider;
    public void RequestUIUpdateWeaponSlider(float current, float max) => onRequestUIUpdateWeaponSlider?.Invoke(current, max);


    public event Action<bool> onRequestUIAbilityShow;
    public void RequestUIAbilityShow(bool show) => onRequestUIAbilityShow?.Invoke(show);


    public event Action<string> onRequestUIUpdateAbilityName;
    public void RequestUIUpdateAbilityName(string newName) => onRequestUIUpdateAbilityName?.Invoke(newName);


    public event Action<int> onRequestUIUpdateAbilityCharges;
    public void RequestUIUpdateAbilityCharges(int charges) => onRequestUIUpdateAbilityCharges?.Invoke(charges);


    public event Action<float, float> onRequestUIUpdateAbilitySlider;
    public void RequestUIUpdateAbilitySlider(float current, float max) => onRequestUIUpdateAbilitySlider?.Invoke(current, max);


    public event Action<Vector3, string, Color, float, float> onRequestUISpawnFloatingText;
    public void RequestUISpawnFloatingText(Vector3 pos, string text, Color c, float driftRange, float duration) => onRequestUISpawnFloatingText?.Invoke(pos, text, c, driftRange, duration);


    #endregion

    #region Control
    public event Action<bool> onRequestEnableControlAll;
    public void RequestEnableControlAll(bool enable) => onRequestEnableControlAll?.Invoke(enable);

    public event Action<bool> onRequestEnableControlPlayer;
    public void RequestEnableControlPlayer(bool enable) => onRequestEnableControlPlayer?.Invoke(enable);



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

    public event Action<bool> onPawnServiceSpawnIdle;
    public void SetPawnServiceIdle(bool spawnIdle) => onPawnServiceSpawnIdle?.Invoke(spawnIdle);

    public event Action<bool> onPawnServiceSpawnAlert;
    public void SetPawnServiceAlert(bool spawnAlert) => onPawnServiceSpawnAlert?.Invoke(spawnAlert); 

    public event Action<EnemyPawn> onEnemyDeath;
    public void EnemyDeath(EnemyPawn ep) => onEnemyDeath?.Invoke(ep);

    #endregion
}
