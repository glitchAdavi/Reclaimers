using UnityEngine;
using UnityEngine.UI;
using Reclaimers.UpdateSystem;
using System;
using TMPro;

namespace Reclaimers.Systems
{
    public class UISystem : MonoBehaviour, IPauseable
    {
        public ObjectBuilder<FloatingText> floatingTextBuilder;

        public CustomList<UIMenu> activeMenues = new CustomList<UIMenu>();

        public GameObject fadeObject;
        public Image fadeImage;
        public UIMenu_Fade fadeMenu;

        public UIMenu pauseMenu;

        public UIMenu missionMenu;

        public UIMenu customizationMenu;

        public UIMenu upgradeMenu;

        public GameObject statMenu;
        public TMP_Text statMenuText;

        public GameObject uiTop;
        public GameObject uiBot;

        public UIFillableBar objectiveBar;
        public TMP_Text objectiveText;
        public TMP_Text currentObjective;
        public GameObject stage1;
        public GameObject stage2;
        public GameObject stage3;

        public TMP_Text killCount;
        public TMP_Text xpGained;
        public TMP_Text moneyGained;

        public TMP_Text healthText;
        public UIFillableBar healthBar;

        public TMP_Text levelText;
        public UIFillableBar xpBar;

        public TMP_Text weaponAmmo;
        public TMP_Text weaponName;
        public UIFillableBar weaponBar;

        public TMP_Text abilityName;
        public TMP_Text abilityAmmo;
        public UIFillableBar abilityBar;

        public GameObject interact;
        public TMP_Text interactText;
        public UIFillableBar interactFill;

        public UIMenu levelUpMenu;


        Timer timerFade;
        bool isPaused = false;

        private void OnEnable()
        {
            this.Register();

            GameObject textParent = new GameObject();
            textParent.name = "FloatingTextParent";
            floatingTextBuilder = new ObjectBuilder<FloatingText>();
            floatingTextBuilder.BuildPool(FloatingTextFactory, 100, textParent.transform);

            Material fMaterial = fadeImage.material;
            fadeImage.material = Instantiate(fMaterial);




            GameManager.Instance.eventSystem.onSpawnFloatingText += SpawnFloatingText;

            GameManager.Instance.eventSystem.onOpenCloseMenues += OpenCloseMenues;

            GameManager.Instance.eventSystem.onEnableUITop += EnableUITop;

            GameManager.Instance.eventSystem.onQueueLevelUp += LevelUpMenu;
            GameManager.Instance.eventSystem.onLevelUpFinish += CloseLevelUpMenu;

            GameManager.Instance.eventSystem.onOpenMissionMenu += MissionMenu;
            GameManager.Instance.eventSystem.onOpenUpgradeMenu += UpgradeMenu;
            GameManager.Instance.eventSystem.onOpenCustomizationMenu += CustomizationMenu;

            GameManager.Instance.eventSystem.onUpdateHp += UpdateHp;
            GameManager.Instance.eventSystem.onUpdateXp += UpdateXp;
            GameManager.Instance.eventSystem.onUpdateLevel += UpdateLevel;
            GameManager.Instance.eventSystem.onUpdateWeapon += UpdateWeapon;
            GameManager.Instance.eventSystem.onUpdateWeaponName += UpdateWeaponName;
            GameManager.Instance.eventSystem.onUpdateAbility += UpdateAbility;
            GameManager.Instance.eventSystem.onUpdateAbilityName += UpdateAbilityName;
            GameManager.Instance.eventSystem.onUpdateKillCount += UpdateKillCount;
            GameManager.Instance.eventSystem.onUpdateXpGained += UpdateXpGained;
            GameManager.Instance.eventSystem.onUpdateMoneyGained += UpdateMoneyGained;
            GameManager.Instance.eventSystem.onUpdateObjective += UpdateObjective;
            GameManager.Instance.eventSystem.onUpdateObjectiveBar += UpdateObjectiveBar;
            GameManager.Instance.eventSystem.onUpdateInteractText += UpdateInteractText;
            GameManager.Instance.eventSystem.onUpdateInteractFill += UpdateInteractFill;

            GameManager.Instance.eventSystem.onTriggerWin += SetFadeWin;
            GameManager.Instance.eventSystem.onTriggerLose += SetFadeLose;
        }

        public void ManagedPause(bool paused)
        {
            isPaused = paused;
            timerFade?.Pause(paused);
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.N)) MissionMenu();
            //if (Input.GetKeyDown(KeyCode.M)) UpgradeMenu();
            //if (Input.GetKeyDown(KeyCode.B)) CustomizationMenu();
        }


        public void OpenCloseMenues()
        {
            if (activeMenues.Count > 0)
            {
                Cursor.visible = false;
                UIMenu m = activeMenues[^1];
                activeMenues.Remove(m);
                m.BeforeClose();
                if (isPaused) GameManager.Instance.eventSystem.SetPause(false);
            } else
            {
                OpenPauseMenu();
            }

        }

        public void OpenPauseMenu()
        {
            Cursor.visible = true;
            activeMenues.Add(pauseMenu);
            pauseMenu.BeforeOpen();

            GameManager.Instance.eventSystem.SetPause(true);
        }

        public void MissionMenu()
        {
            Cursor.visible = true;
            activeMenues.Add(missionMenu);
            missionMenu.BeforeOpen();

            GameManager.Instance.eventSystem.SetPause(true);
        }

        public void CustomizationMenu()
        {
            Cursor.visible = true;
            activeMenues.Add(customizationMenu);
            customizationMenu.BeforeOpen();

            GameManager.Instance.eventSystem.SetPause(true);
        }

        public void UpgradeMenu()
        {
            Cursor.visible = true;
            activeMenues.Add(upgradeMenu);
            upgradeMenu.BeforeOpen();

            GameManager.Instance.eventSystem.SetPause(true);
        }

        public void LevelUpMenu()
        {
            Cursor.visible = true;
            activeMenues.Add(levelUpMenu);
            levelUpMenu.BeforeOpen();

            GameManager.Instance.eventSystem.SetPause(true);
        }

        public void CloseLevelUpMenu()
        {
            Cursor.visible = false;
            activeMenues.Remove(levelUpMenu);
            GameManager.Instance.eventSystem.SetPause(false);
        }

        public void EnableUITop()
        {
            uiTop.SetActive(true);
        }

        public void FadeIn(Action callback, float time = 3f)
        {
            GameManager.Instance.eventSystem.EnableAllControls(false);
            GameManager.Instance.eventSystem.EnablePlayerControls(false);

            float stepTime = time / 100;

            timerFade = GameManager.Instance.timerSystem.SetTimer(time, () =>
            {
                fadeImage.material.SetFloat("_AlphaClip", 1);
                fadeImage.enabled = false;
                if (callback != null) callback();
            }, stepTime, () =>
            {
                fadeImage.material.SetFloat("_AlphaClip", (timerFade.durationElapsed / time));
            }, "UI_FadeIn", false, false);
        }

        public void FadeOut(Action callback, float time = 3f, bool menu = false)
        {
            Cursor.visible = false;
            GameManager.Instance.eventSystem.EnableAllControls(false);
            GameManager.Instance.eventSystem.EnablePlayerControls(false);

            float stepTime = time / 100;
            fadeImage.enabled = true;

            timerFade = GameManager.Instance.timerSystem.SetTimer(time, () =>
            {
                if (callback != null) callback();
                if (menu) OpenFadeMenu();
            }, stepTime, () =>
            {
                fadeImage.material.SetFloat("_AlphaClip", 1 - (timerFade.durationElapsed / time));
            }, "UI_FadeOut", false, false);
        }

        public void OpenFadeMenu()
        {
            Cursor.visible = true;
            fadeMenu.BeforeOpen();
        }

        public void SetFadeWin()
        {
            fadeMenu.SetText("You Survived!");
            fadeMenu.SetStats("STATS");
        }

        public void SetFadeLose()
        {
            fadeMenu.SetText("You Died.");
            fadeMenu.SetStats("");
        }

        #region UpdateUIValues
        public void UpdateHp(float current, float max)
        {
            healthBar.UpdateBar(current / max, 1);
            healthText.text = $"{current.ToString("#.##")}/{max.ToString("#.##")}";
        }

        public void UpdateXp(float current, float max)
        {
            xpBar.UpdateBar(current / max, 1);
        }

        public void UpdateLevel(int lvl)
        {
            levelText.text = $"<size=26>Lvl.<size=34> {lvl.ToString()}";
        }

        public void UpdateWeapon(float current, float max)
        {
            if (max > 49)
            {
                weaponBar.UpdateBar(current / max, 1);
            } else
            {
                weaponBar.UpdateBar(current, max);
            }
            weaponAmmo.text = $"{current}/{max}";
        }

        public void UpdateWeaponName(string wName)
        {
            weaponName.text = wName;
        }

        public void UpdateAbility(float current, float max)
        {
            abilityBar.UpdateBar(current, max);
            abilityAmmo.text = $"{Mathf.FloorToInt(current)}/{max}";
        }

        public void UpdateAbilityName(string aName)
        {
            abilityName.text = aName;
        }

        public void UpdateKillCount(float current)
        {
            killCount.text = current.ToString("#.##");
        }

        public void UpdateXpGained(float current)
        {
            xpGained.text = current.ToString("#.##");
        }

        public void UpdateMoneyGained(float current)
        {
            moneyGained.text = current.ToString("#.##");
        }

        public void UpdateObjective(string current)
        {
            currentObjective.text = $"<size=26>Current objective: <size=36>{current}";
        }

        public void UpdateObjectiveBar(float current, float max, bool updateText, bool altColor = false)
        {
            objectiveBar.UseAltColor(altColor);
            objectiveBar.UpdateBar(current / max, 1);
            if (updateText) objectiveText.text = $"{current:#}<size=26>s";
            else objectiveText.text = "";
        }

        public void UpdateInteractText(string t, bool enable, bool hasKey = true)
        {
            interact.SetActive(enable);
            if (hasKey) interactText.text = $"[F] {t}";
            else interactText.text = $"{t}";
        }

        public void UpdateInteractFill(float current, float max, bool enable)
        {
            interactFill.gameObject.SetActive(enable);
            interactFill.UpdateBar(current, max);
        }
        #endregion


        #region ButtonFuncs





        #endregion

        #region FloatingText
        public void SpawnFloatingText(Vector3 pos, string text, Sprite image, Color c, float driftRange, float maxSize)
        {
            FloatingText newFT = floatingTextBuilder.GetObject();
            newFT.transform.position = new Vector3(pos.x, 1f, pos.z + 0.25f);
            newFT.Init(text, c, image, driftRange, maxSize);
        }

        public FloatingText FloatingTextFactory(Transform parent = null)
        {
            FloatingText ft = Instantiate(GameManager.Instance.gameInfo.floatingTextPrefab,
                                          Vector3.zero,
                                          GameManager.Instance.gameInfo.floatingTextPrefab.transform.rotation,
                                          parent)
                                          .GetComponent<FloatingText>();

            return ft;
        }
        #endregion

    }
}

