using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.LowLevel;

public class UIService : MonoBehaviour
{
    public FloatingTextBuilder floatingTextBuilder;

    public Image fade;

    public GameObject mainMenu;
    public GameObject pauseMenu;
    public bool useMainMenu = true;

    public GameObject uiMapProgression;
    public Slider uiMapProgressionFill;

    public GameObject uiInteract;
    public TMP_Text uiInteractText;
    public Slider uiInteractFill;

    public Slider healthSlider;

    public Slider xpBarSlider;
    public TMP_Text levelCounter;

    public TMP_Text uiWeaponAmmo;
    public Slider uiWeaponReloadTimer;



    Timer timerFadeIn;
    Timer timerFadeOut;


    private void OnEnable()
    {
        floatingTextBuilder = GameManager.current.CreateService<FloatingTextBuilder>();



        GameManager.current.eventService.onRequestUISpawnFloatingText += SpawnFloatingText;
        GameManager.current.eventService.onRequestUIUseMainMenu += (x) => useMainMenu = x;
        GameManager.current.eventService.onRequestUITogglePauseMenu += TogglePauseMenu;
        GameManager.current.eventService.onRequestUIMapProgressionEnable += UpdateUIMapProgressionEnabled;
        GameManager.current.eventService.onRequestUIMapProgression += UpdateUIMapProgression;
        GameManager.current.eventService.onRequestUIMapProgressionSetup += UpdateUIMapProgressionSetup;
        GameManager.current.eventService.onRequestUIUpdateHealth += UpdateUIHealth;
        GameManager.current.eventService.onRequestUIUpdateXpBar += UpdateUIXpBar;
        GameManager.current.eventService.onRequestUIUpdateLevelCounter += UpdateUILevelCounter;
        GameManager.current.eventService.onRequestUIUpdateWeaponAmmo += UpdateUIWeaponAmmo;
        GameManager.current.eventService.onRequestUIUpdateWeaponReloadSetMax += UpdateUIWeaponReloadSetMax;
        GameManager.current.eventService.onRequestUIUpdateWeaponReloadReset += UpdateUIWeaponReloadReset;
        GameManager.current.eventService.onRequestUIUpdateWeaponReloadTimer += UpdateUIWeaponReloadTimer;
        GameManager.current.eventService.onRequestUIUpdateWeaponReloadEnd += UpdateUIWeaponReloadEnd;
        GameManager.current.eventService.onRequestUIUpdateInteractText += UpdateUIInteractText;
        GameManager.current.eventService.onRequestUIUpdateInteractFill += UpdateUIInteractFill;
    }

    public void TogglePauseMenu(bool paused)
    {
        if (useMainMenu) mainMenu.SetActive(paused);
        else pauseMenu.SetActive(paused);
    }

    public void UpdateUIMapProgressionEnabled(bool enabled)
    {
        uiMapProgression.SetActive(enabled);
    }

    public void UpdateUIMapProgression(float current)
    {
        uiMapProgressionFill.value = current;
    }

    public void UpdateUIMapProgressionSetup(float max)
    {
        uiMapProgressionFill.maxValue = max;
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

    public void UpdateUIInteractText(string t, bool enable, bool hasKey = true)
    {
        uiInteract.SetActive(enable);
        if (hasKey) uiInteractText.text = $"[F] {t}";
        else uiInteractText.text = $"{t}";
    }

    public void UpdateUIInteractFill(float current, float max, bool enable)
    {
        uiInteractFill.maxValue = max;
        uiInteractFill.value = current;
    }


    public void SpawnFloatingText(Vector3 pos, string text, Color c, float driftRange, float duration)
    {
        UIFloatingText newFloatingText = floatingTextBuilder.GetObject();
        newFloatingText.transform.position = new Vector3(pos.x, 1f, pos.z);
        newFloatingText.Init(text, c, driftRange, duration);

    }






    public void FadeIn(Action callback)
    {
        GameManager.current.eventService.RequestEnableControlAll(false);
        GameManager.current.eventService.RequestEnableControlPlayer(false);

        timerFadeIn = GameManager.current.timerService.StartTimer(1f, callback, 0.01f, () => {
            Color temp = fade.color;
            temp.a -= 0.01f;
            fade.color = temp;
            if (temp.a <= 0.01f) fade.enabled = false;
        });
    }

    public void FadeOut(Action callback)
    {
        GameManager.current.eventService.RequestEnableControlAll(false);
        GameManager.current.eventService.RequestEnableControlPlayer(false);

        fade.enabled = true;

        timerFadeOut = GameManager.current.timerService.StartTimer(1f, callback, 0.01f, () => {
            Color temp = fade.color;
            temp.a += 0.01f;
            fade.color = temp;
        });
    }
}
