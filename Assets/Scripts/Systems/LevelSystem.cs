using Reclaimers.DeveloperConsole;
using Reclaimers.UpdateSystem;
using UnityEngine;

namespace Reclaimers.Systems
{
    public class LevelSystem : MonoBehaviour
    {
        [SerializeField] CustomList<XpOrb> xpOrbsInScene = new CustomList<XpOrb>();
        [SerializeField] CustomList<MoneyOrb> moneyOrbsInScene = new CustomList<MoneyOrb>();

        [SerializeField] LevelLogic currentLevelLogic;



        private void OnEnable()
        {
            currentLevelLogic = GameObject.Find("Grid").GetComponent<LevelLogic>();
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
    }
}

