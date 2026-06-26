using System;
using UnityEngine;

namespace Reclaimers.Systems
{
    public class EventSystem : MonoBehaviour
    {
        public static EventSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }


        #region UI
        public event Action<Vector3, string, Color, float> onSpawnFloatingText;
        public void SpawnFloatingText(Vector3 pos, string text, Color c, float driftRange = 0f) => onSpawnFloatingText?.Invoke(pos, text, c, driftRange);



        #endregion


        #region Player
        public event Action<float> onGivePlayerXp;
        public void GivePlayerXp(float v) => onGivePlayerXp?.Invoke(v);

        public event Action<int, bool> onGivePlayerLevel;
        public void GivePlayerLevel(int v, bool silent = false) => onGivePlayerLevel?.Invoke(v, silent);

        public event Action<float> onGivePlayerMoney;
        public void GivePlayerMoney(float v) => onGivePlayerMoney?.Invoke(v);
        #endregion


        #region Enemy
        public event Action<Pawn> onEnemyDeath;
        public void EnemyDeath(Pawn enemy) => onEnemyDeath?.Invoke(enemy);


        #endregion
    }
}

