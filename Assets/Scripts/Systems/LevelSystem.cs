using Reclaimers.DeveloperConsole;
using Reclaimers.UpdateSystem;
using System.Linq;
using UnityEngine;

namespace Reclaimers.Systems
{
    public class LevelSystem : MonoBehaviour
    {
        [SerializeField] CustomList<XpOrb> xpOrbsInScene = new CustomList<XpOrb>();
        [SerializeField] CustomList<XpOrb> visibleXpOrbs = new CustomList<XpOrb>();
        [SerializeField] private float accumulatedXp = 0f;
        private int maxXpOrbsInScene = 150;
        private int maxXpOrbsVisible = 100;

        [SerializeField] CustomList<MoneyOrb> moneyOrbsInScene = new CustomList<MoneyOrb>();
        [SerializeField] CustomList<MoneyOrb> visibleMoneyOrbs = new CustomList<MoneyOrb>();
        [SerializeField] private float accumulatedMoney = 0f;
        private int maxMoneyOrbsInScene = 150;
        private int maxMoneyOrbsVisible = 100;

        [SerializeField] LevelLogic currentLevelLogic;




        private void OnEnable()
        {
            currentLevelLogic = GameObject.Find("Grid").GetComponent<LevelLogic>();

            EventSystem.Instance.onSpawnXp += SpawnXpOrb;
            EventSystem.Instance.onXpOrbDeath += RemoveXpOrb;
            EventSystem.Instance.onXpOrbDecay += OnXpOrbDecay;
            EventSystem.Instance.onXpOrbUpdateVisibility += AddRemoveXpOrbVisible;

            EventSystem.Instance.onSpawnMoney += SpawnMoneyOrb;
            EventSystem.Instance.onMoneyOrbDeath += RemoveMoneyOrb;
            EventSystem.Instance.onMoneyOrbDecay += OnMoneyOrbDecay;
            EventSystem.Instance.onMoneyOrbUpdateVisibility += AddRemoveMoneyOrbVisible;

        }

        public void StartLevel()
        {

            if (currentLevelLogic == null)
            {
                LogSystem.Log("LevelSystem: Missing Level Logic");
                return;
            }

            if (currentLevelLogic.isActive) return;

            currentLevelLogic.Begin();
        }








        #region XP
        public void SpawnXpOrb(Vector3 pos, float val)
        {
            if (xpOrbsInScene.Count >= maxXpOrbsInScene)
            {
                XpOrb notVis = null;
                foreach (XpOrb xpOrb in xpOrbsInScene)
                {
                    if (!visibleXpOrbs.Contains(xpOrb))
                    {
                        notVis = xpOrb;
                        break;
                    }
                }

                if (notVis != null) notVis.ForceDecay();
            }

            if (visibleXpOrbs.Count >= maxXpOrbsVisible)
            {
                xpOrbsInScene[^1].SetValue(xpOrbsInScene[^1].GetValue() + val + accumulatedXp);
                accumulatedXp = 0;
                return;
            }

            Vector3 newPos = new Vector3(pos.x, 0.25f, pos.z);
            XpOrb newOrb = Instantiate(GameManager.Instance.gameInfo.xpOrbPrefab, newPos, Quaternion.identity).GetComponent<XpOrb>();
            newOrb.Init(val + accumulatedXp);
            accumulatedXp = 0;
            xpOrbsInScene.Add(newOrb);
        }


        public void RemoveXpOrb(XpOrb toRemove)
        {
            xpOrbsInScene.Remove(toRemove);
        }

        public void AddRemoveXpOrbVisible(XpOrb xpOrb, bool visible)
        {
            if (visible)
            {
                if (!visibleXpOrbs.Contains(xpOrb)) visibleXpOrbs.Add(xpOrb);
            }
            else
            {
                if (visibleXpOrbs.Contains(xpOrb)) visibleXpOrbs.Remove(xpOrb);
            }
        }

        public void OnXpOrbDecay(XpOrb xpOrb)
        {
            accumulatedXp += xpOrb.GetValue();
        }
        #endregion

        #region Money
        public void SpawnMoneyOrb(Vector3 pos, float val)
        {
            if (moneyOrbsInScene.Count >= maxMoneyOrbsInScene)
            {
                MoneyOrb notVis = null;
                foreach (MoneyOrb moneyOrb in moneyOrbsInScene)
                {
                    if (!visibleMoneyOrbs.Contains(moneyOrb))
                    {
                        notVis = moneyOrb;
                        break;
                    }
                }

                if (notVis != null) notVis.ForceDecay();
            }

            if (visibleMoneyOrbs.Count >= maxMoneyOrbsVisible)
            {
                moneyOrbsInScene[^1].SetValue(moneyOrbsInScene[^1].GetValue() + val + accumulatedMoney);
                accumulatedMoney = 0;
                return;
            }

            Vector3 newPos = new Vector3(pos.x, 0.25f, pos.z);
            MoneyOrb newOrb = Instantiate(GameManager.Instance.gameInfo.moneyOrbPrefab, newPos, Quaternion.identity).GetComponent<MoneyOrb>();
            newOrb.Init(val + accumulatedMoney);
            accumulatedMoney = 0;
            moneyOrbsInScene.Add(newOrb);
        }


        public void RemoveMoneyOrb(MoneyOrb toRemove)
        {
            moneyOrbsInScene.Remove(toRemove);
        }

        public void AddRemoveMoneyOrbVisible(MoneyOrb moneyOrb, bool visible)
        {
            if (visible)
            {
                if (!visibleMoneyOrbs.Contains(moneyOrb)) visibleMoneyOrbs.Add(moneyOrb);
            }
            else
            {
                if (visibleMoneyOrbs.Contains(moneyOrb)) visibleMoneyOrbs.Remove(moneyOrb);
            }
        }

        public void OnMoneyOrbDecay(MoneyOrb moneyOrb)
        {
            accumulatedMoney += moneyOrb.GetValue();
        }
        #endregion
    }
}

