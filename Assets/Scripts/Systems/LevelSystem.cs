using Reclaimers.DeveloperConsole;
using Reclaimers.UpdateSystem;
using System.Linq;
using UnityEngine;

namespace Reclaimers.Systems
{
    public class LevelSystem : MonoBehaviour
    {
        GameObject xpOrbParent;
        ObjectBuilder<XpOrb> _xpOrbBuilder;
        [SerializeField] CustomList<XpOrb> xpOrbsInScene = new CustomList<XpOrb>();
        [SerializeField] CustomList<XpOrb> visibleXpOrbs = new CustomList<XpOrb>();
        [SerializeField] private float accumulatedXp = 0f;
        private int maxXpOrbsInScene = 150;
        private int maxXpOrbsVisible = 100;

        GameObject moneyOrbParent;
        ObjectBuilder<MoneyOrb> _moneyOrbBuilder;
        [SerializeField] CustomList<MoneyOrb> moneyOrbsInScene = new CustomList<MoneyOrb>();
        [SerializeField] CustomList<MoneyOrb> visibleMoneyOrbs = new CustomList<MoneyOrb>();
        [SerializeField] private float accumulatedMoney = 0f;
        private int maxMoneyOrbsInScene = 150;
        private int maxMoneyOrbsVisible = 100;

        [SerializeField] CustomList<Interactable> interactablesInScene = new CustomList<Interactable>();

        [SerializeField] LevelLogic currentLevelLogic;




        private void OnEnable()
        {
            currentLevelLogic = GameObject.Find("Grid").GetComponent<LevelLogic>();

            xpOrbParent = new GameObject();
            xpOrbParent.name = "XpOrbParent";
            _xpOrbBuilder = new ObjectBuilder<XpOrb>();
            _xpOrbBuilder.BuildPool(XpOrbFactory, 50, xpOrbParent.transform);
            GameManager.Instance.eventSystem.onSpawnXp += SpawnXpOrb;
            GameManager.Instance.eventSystem.onXpOrbDeath += RemoveXpOrb;
            GameManager.Instance.eventSystem.onXpOrbDecay += OnXpOrbDecay;
            GameManager.Instance.eventSystem.onXpOrbUpdateVisibility += AddRemoveXpOrbVisible;

            
            moneyOrbParent = new GameObject();
            moneyOrbParent.name = "MoneyOrbParent";
            _moneyOrbBuilder = new ObjectBuilder<MoneyOrb>();
            _moneyOrbBuilder.BuildPool(MoneyOrbFactory, 50, moneyOrbParent.transform);
            GameManager.Instance.eventSystem.onSpawnMoney += SpawnMoneyOrb;
            GameManager.Instance.eventSystem.onMoneyOrbDeath += RemoveMoneyOrb;
            GameManager.Instance.eventSystem.onMoneyOrbDecay += OnMoneyOrbDecay;
            GameManager.Instance.eventSystem.onMoneyOrbUpdateVisibility += AddRemoveMoneyOrbVisible;

            GameManager.Instance.eventSystem.onTriggerEnd += OnEnd;
            GameManager.Instance.eventSystem.onTriggerWin += OnWin;
            GameManager.Instance.eventSystem.onTriggerLose += OnLose;

            GameManager.Instance.eventSystem.onSpawnRandomUpgrade += SpawnRandomUpgrade;

            GetInteractablesInScene();
        }

        public void StartLevel()
        {

            if (currentLevelLogic == null)
            {
                LogSystem.Log("LevelSystem: Missing Level Logic");
                return;
            }

            if (currentLevelLogic.isActive) return;


            GameManager.Instance.eventSystem.EnableAllControls(true);
            currentLevelLogic.Begin();
        }


        #region XP
        public XpOrb XpOrbFactory(Transform parent = null)
        {
            return Instantiate(GameManager.Instance.gameInfo.xpOrbPrefab, Vector3.zero, Quaternion.identity, parent).GetComponent<XpOrb>();
        }

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
            XpOrb newOrb = _xpOrbBuilder.GetObject();
            newOrb.transform.position = newPos;
            newOrb.Init(val + accumulatedXp);
            accumulatedXp = 0;
            xpOrbsInScene.Add(newOrb);
        }


        public void RemoveXpOrb(XpOrb toRemove)
        {
            xpOrbsInScene.Remove(toRemove);
            visibleXpOrbs.Remove(toRemove);
            _xpOrbBuilder.ReturnObject(toRemove);
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
        public MoneyOrb MoneyOrbFactory(Transform parent = null)
        {
            return Instantiate(GameManager.Instance.gameInfo.moneyOrbPrefab, Vector3.zero, Quaternion.identity, parent).GetComponent<MoneyOrb>();
        }

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
            MoneyOrb newOrb = _moneyOrbBuilder.GetObject();
            newOrb.transform.position = newPos;
            newOrb.Init(val + accumulatedMoney);
            accumulatedMoney = 0;
            moneyOrbsInScene.Add(newOrb);
        }


        public void RemoveMoneyOrb(MoneyOrb toRemove)
        {
            moneyOrbsInScene.Remove(toRemove);
            visibleMoneyOrbs.Remove(toRemove);
            _moneyOrbBuilder.ReturnObject(toRemove);
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

        public void OnEnd()
        {

        }

        public void OnWin()
        {

        }

        public void OnLose()
        {

        }


        #region Interactables
        public void GetInteractablesInScene()
        {
            Interactable[] interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
            foreach (Interactable i in interactables)
            {
                interactablesInScene.Add(i);
            }
        }

        public Interactable GetClosestInteractable(Vector3 pos, float range)
        {
            if (interactablesInScene.Count < 1) return null;

            Interactable result = null;
            float currentResultDistance = range * 2;

            for (int i = 0; i < interactablesInScene.Count; i++)
            {
                if (interactablesInScene[i].used) continue;
                float currentIODistance = Vector3.Distance(interactablesInScene[i].transform.position, pos);
                if (currentIODistance <= range)
                {
                    if (result != null)
                    {
                        if (currentIODistance < currentResultDistance)
                        {
                            currentResultDistance = currentIODistance;
                            result = interactablesInScene[i];
                        }
                    }
                    else
                    {
                        currentResultDistance = currentIODistance;
                        result = interactablesInScene[i];
                    }
                }
            }

            return result;
        }

        public void RemoveInteractable(Interactable i)
        {
            interactablesInScene.Remove(i);
        }

        public void SpawnRandomUpgrade(Vector3 pos)
        {
            I_Upgrade u = Instantiate(GameManager.Instance.gameInfo.upgradePrefab, pos, Quaternion.identity).GetComponent<I_Upgrade>();
            interactablesInScene.Add(u);
        }
        #endregion
    }
}

