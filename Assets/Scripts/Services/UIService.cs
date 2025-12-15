using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UIService : MonoBehaviour, IPause
{
    public FloatingTextBuilder floatingTextBuilder;

    public Image fade;

    public bool useMainMenu = true;
    public GameObject mainMenu;
    public GameObject pauseMenu;

    public GameObject statsMenu;
    public TMP_Text statsMenuText;

    public GameObject uiMapProgression;
    public Slider uiMapProgressionFill;

    public GameObject stage1On;
    public GameObject stage2On;
    public GameObject stage3On;

    public TMP_Text killCount;
    public TMP_Text xpGained;
    public TMP_Text materialsGained;

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
    public TMP_Text levelUpPending;

    public GameObject shopMenu;
    public TMP_Text currentMaterials;
    public UI_ShopStat ssLife;
    public UI_ShopStat ssXpGain;
    public UI_ShopStat ssMaterialGain;
    public UI_ShopStat ssSpeed;
    public UI_ShopStat ssDamageMultiplier;
    public UI_ShopStat ssHealingMultiplier;
    public UI_ShopStat ssProjDamage;
    public UI_ShopStat ssPickUpRange;

    Timer timerFadeIn;
    Timer timerFadeOut;

    public GameObject fadeMenu;


    private void OnEnable()
    {
        GameManager.current.updateService.RegisterPause(this);
        floatingTextBuilder = GameManager.current.CreateService<FloatingTextBuilder>();

        UpdateUIKillCount(0);
        UpdateUIXpGained(0f);
        UpdateUIMaterialsGained(0f);

        GameManager.current.eventService.onRequestUISpawnFloatingText += SpawnFloatingText;
        GameManager.current.eventService.onRequestUIUseMainMenu += (x) => useMainMenu = x;
        GameManager.current.eventService.onRequestUITogglePauseMenu += TogglePauseMenu;
        GameManager.current.eventService.onRequestUIMapProgressionEnable += UpdateUIMapProgressionEnabled;
        GameManager.current.eventService.onRequestUIMapProgression += UpdateUIMapProgression;

        GameManager.current.eventService.onRequestUIMapProgressionSetup += UpdateUIMapProgressionSetup;
        GameManager.current.eventService.onRequestUIMapStage1 += UpdateUIMapStage1;
        GameManager.current.eventService.onRequestUIMapStage2 += UpdateUIMapStage2;
        GameManager.current.eventService.onRequestUIMapStage3 += UpdateUIMapStage3;
        GameManager.current.eventService.onRequestUIUpdateKillCount += UpdateUIKillCount;
        GameManager.current.eventService.onRequestUIUpdateXpGained += UpdateUIXpGained;
        GameManager.current.eventService.onRequestUIUpdateMaterialsGained += UpdateUIMaterialsGained;

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
        GameManager.current.eventService.onRequestUIUpdateAbilityCharges += UpdateUIAbilityCharges;
        GameManager.current.eventService.onRequestUIUpdateAbilitySlider += UpdateUIAbilitySlider;

        GameManager.current.eventService.onQueueLevelUp += LevelUpMenuOpen;
        GameManager.current.eventService.onLevelUpFinish += LevelUpMenuClose;

        GameManager.current.eventService.onRequestUIOpenShopMenu += ShopOpen;
        GameManager.current.eventService.onRequestUICloseShopMenu += ShopClose;
    }

    public void Pause(bool paused)
    {
        timerFadeIn?.Pause(paused);
        timerFadeOut?.Pause(paused);
    }

    public void TogglePauseMenu(bool paused)
    {
        UpdateStats();
        statsMenu.SetActive(paused);
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

    public void UpdateUIMapStage1()
    {
        stage1On.SetActive(true);
        stage2On.SetActive(false);
        stage3On.SetActive(false);
    }

    public void UpdateUIMapStage2()
    {
        stage1On.SetActive(false);
        stage2On.SetActive(true);
        stage3On.SetActive(false);
    }

    public void UpdateUIMapStage3()
    {
        stage1On.SetActive(false);
        stage2On.SetActive(false);
        stage3On.SetActive(true);
    }

    public void UpdateUIKillCount(int kc)
    {
        killCount.text = $"{kc}";
    }

    public void UpdateUIXpGained(float xp)
    {
        xpGained.text = $"{xp.ToString("#.##")}";
    }

    public void UpdateUIMaterialsGained(float materials)
    {
        materialsGained.text = $"{materials.ToString("#.##")}";
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

    public void UpdateUIAbilityCharges(int charges)
    {
        if (charges < 0)
        {
            uiAbilityCharges.enabled = false;
        } else
        {
            uiAbilityCharges.enabled = true;
            uiAbilityCharges.text = charges.ToString();
        }
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
        if (GameManager.current.playerPawn.pendingLevelUps > 0)
        {
            levelUpPending.enabled = true;
            levelUpPending.text = $"({GameManager.current.playerPawn.pendingLevelUps} pending)";
        }
    }

    public void LevelUpMenuClose()
    {
        levelUpPending.enabled = false;
        levelUpMenu.SetActive(false);
        GameManager.current.eventService.RequestEnableControlAll(true);
        GameManager.current.eventService.RequestTogglePause();
    }




    public void FadeIn(Action callback, float time = 1f)
    {
        GameManager.current.eventService.RequestEnableControlAll(false);
        GameManager.current.eventService.RequestEnableControlPlayer(false);

        timerFadeIn = GameManager.current.timerService.StartTimer(time, callback, 0.01f, () => {
            Color temp = fade.color;
            time -= 0.01f;
            temp.a = time;
            fade.color = temp;
            if (temp.a <= 0.01f) fade.enabled = false;
        });
    }

    public void FadeOut(Action callback, float time = 1f)
    {
        GameManager.current.eventService.RequestEnableControlAll(false);
        GameManager.current.eventService.RequestEnableControlPlayer(false);

        fade.enabled = true;

        timerFadeOut = GameManager.current.timerService.StartTimer(time, callback, 0.01f, () => {
            Color temp = fade.color;
            temp.a += 0.01f;
            fade.color = temp;
        });
    }

    public void EnableFadeMenu()
    {
        fadeMenu.SetActive(true);
    }





    public void UpdateStats()
    {
        statsMenuText.text = $"Player:\n" +
            $"Speed - {GameManager.current.playerPawn.statBlock.speed.ValuesAsString()}\n" +
            $"Lifepoints - {GameManager.current.playerPawn.statBlock.lifepoints.ValuesAsString()}";
    }




    public void ShopOpen()
    {
        GameManager.current.eventService.RequestEnableControlPlayer(false);
        currentMaterials.text = GameManager.current.playerPawn.statBlock.materials.Value().ToString("#.##");
        ssLife.Init("pawn_permanent_lifepoints");
        ssXpGain.Init("pawn_permanent_xpgain");
        ssMaterialGain.Init("pawn_permanent_materialgain");
        ssSpeed.Init("pawn_permanent_speed");
        ssDamageMultiplier.Init("pawn_permanent_damagemultiplier");
        ssHealingMultiplier.Init("pawn_permanent_healingmultiplier");
        ssProjDamage.Init("weapon_permanent_projdamage");
        ssPickUpRange.Init("pawn_permanent_pickuprange");
        shopMenu.SetActive(true);
    }

    public void ShopClose()
    {
        GameManager.current.eventService.RequestEnableControlPlayer(true);
        shopMenu.SetActive(false);
    }
}
