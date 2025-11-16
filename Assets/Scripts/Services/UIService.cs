using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIService : MonoBehaviour
{
    public DamageNumberBuilder damageNumberBuilder;

    public GameObject pauseMenu;

    public TMP_Text uiInteractText;
    public Slider uiInteractFill;

    public Slider healthSlider;

    public Slider xpBarSlider;
    public TMP_Text levelCounter;

    public TMP_Text uiWeaponAmmo;
    public Slider uiWeaponReloadTimer;


    private void OnEnable()
    {
        damageNumberBuilder = GameManager.current.CreateService<DamageNumberBuilder>();



        GameManager.current.eventService.onEnemyHurt += SpawnDamageNumber;
        GameManager.current.eventService.onRequestUITogglePauseMenu += TogglePauseMenu;
        GameManager.current.eventService.onRequestUIUpdateHealth += UpdateUIHealth;
        GameManager.current.eventService.onRequestUIUpdateXpBar += UpdateUIXpBar;
        GameManager.current.eventService.onRequestUIUpdateLevelCounter += UpdateUILevelCounter;
        GameManager.current.eventService.onRequestUIUpdateWeaponAmmo += UpdateUIWeaponAmmo;
        GameManager.current.eventService.onRequestUIUpdateWeaponReloadSetMax += UpdateUIWeaponReloadSetMax;
        GameManager.current.eventService.onRequestUIUpdateWeaponReloadReset += UpdateUIWeaponReloadReset;
        GameManager.current.eventService.onRequestUIUpdateWeaponReloadTimer += UpdateUIWeaponReloadTimer;
        GameManager.current.eventService.onRequestUIUpdateWeaponReloadEnd += UpdateUIWeaponReloadEnd;
    }

    public void TogglePauseMenu(bool paused)
    {
        pauseMenu.SetActive(paused);
    }

    public void UpdateUIHealth(float current, float max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }

    public void UpdateUIXpBar(float xp, int threshold)
    {
        xpBarSlider.maxValue = threshold;
        xpBarSlider.value = xp;
    }

    public void UpdateUILevelCounter(int level)
    {
        levelCounter.text = $"<size=70%>Lvl<size=100%>  {level.ToString()}";
    }

    public void UpdateUIWeaponAmmo(int current, int max)
    {
        uiWeaponAmmo.text = $"{current}/{max}";
    }

    public void UpdateUIWeaponReloadSetMax(float max)
    {
        uiWeaponReloadTimer.maxValue = max;
        uiWeaponReloadTimer.value = max;
    }

    public void UpdateUIWeaponReloadReset()
    {
        uiWeaponReloadTimer.value = 0;
    }

    public void UpdateUIWeaponReloadTimer()
    {
        if (uiWeaponReloadTimer.value > uiWeaponReloadTimer.maxValue) uiWeaponReloadTimer.value = uiWeaponReloadTimer.maxValue;
        else uiWeaponReloadTimer.value += Time.fixedDeltaTime;
    }

    public void UpdateUIWeaponReloadEnd()
    {
        uiWeaponReloadTimer.value = uiWeaponReloadTimer.maxValue;
    }


    public void SpawnDamageNumber(Vector3 pos, float damage, bool isCrit)
    {
        UIDamageNumber newDamageNumber = damageNumberBuilder.GetObject();
        newDamageNumber.transform.position = new Vector3(pos.x, 0.7f, pos.z);
        newDamageNumber.SetText(damage, isCrit);
    }
}
