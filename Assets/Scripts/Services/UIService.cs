using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public GameObject uiWeapon;
    public TMP_Text uiWeaponName;
    public TMP_Text uiWeaponAmmo;
    public Slider uiWeaponReloadSlider;

    public GameObject uiAbility;
    public TMP_Text uiAbilityName;
    public TMP_Text uiAbilityCharges;
    public Slider uiAbilityCooldownSlider;

    public GameObject levelUpMenu;


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

        GameManager.current.eventService.onRequestUIWeaponShow += (x) => uiWeapon.SetActive(x);
        GameManager.current.eventService.onRequestUIUpdateWeaponName += (x) => uiWeaponName.text = x;
        GameManager.current.eventService.onRequestUIUpdateWeaponAmmo += UpdateUIWeaponAmmo;
        GameManager.current.eventService.onRequestUIUpdateWeaponSlider += UpdateUIWeaponSlider;

        GameManager.current.eventService.onRequestUIUpdateInteractText += UpdateUIInteractText;
        GameManager.current.eventService.onRequestUIUpdateInteractFill += UpdateUIInteractFill;

        GameManager.current.eventService.onRequestUIAbilityShow += (x) => uiAbility.SetActive(x);
        GameManager.current.eventService.onRequestUIUpdateAbilityName += (x) => uiAbilityName.text = x;
        GameManager.current.eventService.onRequestUIUpdateAbilityCharges += (x) => uiAbilityCharges.text = x.ToString();
        GameManager.current.eventService.onRequestUIUpdateAbilitySlider += UpdateUIAbilitySlider;

        GameManager.current.eventService.onQueueLevelUp += LevelUpMenuOpen;
        GameManager.current.eventService.onLevelUpFinish += LevelUpMenuClose;
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

    public void UpdateUIMapProgressionSetup(float max, Color? newColor = null)
    {
        uiMapProgressionFill.maxValue = max;
        if (newColor != null) uiMapProgressionFill.image.color = (Color)newColor;
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

    public void UpdateUIWeaponSlider(float current, float max)
    {
        uiWeaponReloadSlider.maxValue = max;
        uiWeaponReloadSlider.value = current;
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

    public void UpdateUIAbilitySlider(float current, float max)
    {
        uiAbilityCooldownSlider.maxValue = max;
        uiAbilityCooldownSlider.value = current;
    }




    public void SpawnFloatingText(Vector3 pos, string text, Color c, float driftRange, float duration)
    {
        UIFloatingText newFloatingText = floatingTextBuilder.GetObject();
        newFloatingText.transform.position = new Vector3(pos.x, 1f, pos.z);
        newFloatingText.Init(text, c, driftRange, duration);

    }

    public void LevelUpMenuOpen()
    {
        GameManager.current.eventService.RequestTogglePause();
        GameManager.current.eventService.RequestEnableControlAll(false);
        levelUpMenu.SetActive(true);
    }

    public void LevelUpMenuClose()
    {
        levelUpMenu.SetActive(false);
        GameManager.current.eventService.RequestEnableControlAll(true);
        GameManager.current.eventService.RequestTogglePause();
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
