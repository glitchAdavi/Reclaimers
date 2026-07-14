using UnityEngine;
using UnityEngine.UI;
using Reclaimers.UpdateSystem;
using UnityEditor.MPE;
using System;
using System.Collections.Generic;
using TMPro;

namespace Reclaimers.Systems
{
    public class UISystem : MonoBehaviour, IPauseable
    {
        public ObjectBuilder<FloatingText> floatingTextBuilder;

        public CustomList<UIMenu> activeMenues = new CustomList<UIMenu>();

        public GameObject fadeObject;
        public Image fadeImage;
        public GameObject fadeMenu;

        public UIMenu pauseMenu;

        public GameObject statMenu;
        public TMP_Text statMenuText;

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






            EventSystem.Instance.onSpawnFloatingText += SpawnFloatingText;

            EventSystem.Instance.onOpenCloseMenues += OpenCloseMenues;

            EventSystem.Instance.onQueueLevelUp += LevelUpMenu;
            EventSystem.Instance.onLevelUpFinish += CloseLevelUpMenu;

            EventSystem.Instance.onUpdateHp += UpdateHp;
            EventSystem.Instance.onUpdateXp += UpdateXp;
            EventSystem.Instance.onUpdateLevel += UpdateLevel;
            EventSystem.Instance.onUpdateWeapon += UpdateWeapon;
            EventSystem.Instance.onUpdateWeaponName += UpdateWeaponName;
            EventSystem.Instance.onUpdateAbility += UpdateAbility;
            EventSystem.Instance.onUpdateAbilityName += UpdateAbilityName;
            EventSystem.Instance.onUpdateKillCount += UpdateKillCount;
            EventSystem.Instance.onUpdateXpGained += UpdateXpGained;
            EventSystem.Instance.onUpdateMoneyGained += UpdateMoneyGained;
        }

        public void ManagedPause(bool paused)
        {
            isPaused = paused;
            timerFade?.Pause(paused);
        }


        public void OpenCloseMenues()
        {
            if (activeMenues.Count > 0)
            {
                UIMenu m = activeMenues[^1];
                activeMenues.Remove(m);
                m.BeforeClose();
                if (isPaused) EventSystem.Instance.SetPause(false);
            } else
            {
                OpenPauseMenu();
            }

        }

        public void OpenPauseMenu()
        {
            activeMenues.Add(pauseMenu);
            pauseMenu.BeforeOpen();

            EventSystem.Instance.SetPause(true);
        }

        public void LevelUpMenu()
        {
            activeMenues.Add(levelUpMenu);
            levelUpMenu.BeforeOpen();

            EventSystem.Instance.SetPause(true);
        }

        public void CloseLevelUpMenu()
        {
            activeMenues.Remove(levelUpMenu);
            EventSystem.Instance.SetPause(false);
        }

        public void FadeIn(Action callback, float time = 1f)
        {
            EventSystem.Instance.EnableAllControls(false);
            EventSystem.Instance.EnablePlayerControls(false);

            float stepTime = time / 100;

            timerFade = GameManager.Instance.timerSystem.SetTimer(time, callback, stepTime, () =>
            {
                Color temp = fadeImage.color;
                time -= 0.01f;
                temp.a = time;
                fadeImage.color = temp;
                if (temp.a <= 0.01f) fadeImage.enabled = false;
            });
        }

        public void FadeOut(Action callback, float time = 1f, bool menu = false)
        {
            EventSystem.Instance.EnableAllControls(false);
            EventSystem.Instance.EnablePlayerControls(false);

            float stepTime = time / 100;
            fadeImage.enabled = true;

            timerFade = GameManager.Instance.timerSystem.SetTimer(time, () =>
            {
                Cursor.visible = true;

                callback();
                fadeMenu.SetActive(menu);
            }, stepTime, () =>
            {
                Color temp = fadeImage.color;
                temp.a += 0.01f;
                fadeImage.color = temp;
            });
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
            abilityAmmo.text = $"{Mathf.FloorToInt(current)}, {max}";
        }

        public void UpdateAbilityName(string aName)
        {
            abilityName.text = aName;
        }

        public void UpdateKillCount(float current)
        {

        }

        public void UpdateXpGained(float current)
        {

        }

        public void UpdateMoneyGained(float current)
        {

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

