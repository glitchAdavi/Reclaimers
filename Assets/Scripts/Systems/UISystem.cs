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

        public bool useMainMenu = true;
        public GameObject mainMenu;
        public GameObject pauseMenu;

        public GameObject statMenu;
        public TMP_Text statMenuText;

        public TMP_Text currentObjective;
        public GameObject stage1;
        public GameObject stage2;
        public GameObject stage3;

        public TMP_Text killCount;
        public TMP_Text xpGained;
        public TMP_Text moneyGained;




        Timer timerFade;

        private void OnEnable()
        {
            this.Register();

            GameObject textParent = new GameObject();
            textParent.name = "FloatingTextParent";
            floatingTextBuilder = new ObjectBuilder<FloatingText>();
            floatingTextBuilder.BuildPool(FloatingTextFactory, 100, textParent.transform);

            EventSystem.Instance.onSpawnFloatingText += SpawnFloatingText;
        }

        public void ManagedPause(bool paused)
        {
            timerFade?.Pause(paused);
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

        #region FloatingText
        public void SpawnFloatingText(Vector3 pos, string text, Color c, float driftRange, float maxSize)
        {
            FloatingText newFT = floatingTextBuilder.GetObject();
            newFT.transform.position = new Vector3(pos.x, 1f, pos.z + 0.25f);
            newFT.Init(text, c, driftRange, maxSize);
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

