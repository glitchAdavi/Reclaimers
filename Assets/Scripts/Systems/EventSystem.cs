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


        public event Action<bool> onSetPause;
        public void SetPause(bool pause) => onSetPause?.Invoke(pause);

        #region UI
        public event Action<Vector3, string, Color, float, float> onSpawnFloatingText;
        public void SpawnFloatingText(Vector3 pos, string text, Color c, float driftRange = 0f, float maxSize = 6f) => onSpawnFloatingText?.Invoke(pos, text, c, driftRange, maxSize);



        #endregion


        #region Player
        public event Action<bool> onEnableAllControls;
        public void EnableAllControls(bool active) => onEnableAllControls?.Invoke(active);

        public event Action<bool> onEnablePlayerControls;
        public void EnablePlayerControls(bool active) => onEnablePlayerControls?.Invoke(active);

        public event Action<float> onGivePlayerXp;
        public void GivePlayerXp(float v) => onGivePlayerXp?.Invoke(v);

        public event Action<int, bool> onGivePlayerLevel;
        public void GivePlayerLevel(int v, bool silent = false) => onGivePlayerLevel?.Invoke(v, silent);

        public event Action<float> onGivePlayerMoney;
        public void GivePlayerMoney(float v) => onGivePlayerMoney?.Invoke(v);

        public event Action onPlayerDeath;
        public void PlayerDeath() => onPlayerDeath?.Invoke();
        #endregion


        #region Enemy
        public event Action<Pawn> onEnemyDeath;
        public void EnemyDeath(Pawn enemy) => onEnemyDeath?.Invoke(enemy);


        #endregion

        #region XpOrb
        public event Action<Vector3, float> onSpawnXp;
        public void SpawnXp(Vector3 pos, float val) => onSpawnXp?.Invoke(pos, val);

        public event Action<XpOrb, bool> onXpOrbUpdateVisibility;
        public void XpOrbUpdateVisibility(XpOrb xpOrb, bool visible) => onXpOrbUpdateVisibility?.Invoke(xpOrb, visible);

        public event Action<XpOrb> onXpOrbDeath;
        public void XpOrbDeath(XpOrb xpOrb) => onXpOrbDeath?.Invoke(xpOrb);

        public event Action<XpOrb> onXpOrbDecay;
        public void XpOrbDecay(XpOrb xpOrb) => onXpOrbDecay?.Invoke(xpOrb);
        #endregion

        #region MoneyOrb
        public event Action<Vector3, float> onSpawnMoney;
        public void SpawnMoney(Vector3 pos, float val) => onSpawnMoney?.Invoke(pos, val);

        public event Action<MoneyOrb, bool> onMoneyOrbUpdateVisibility;
        public void MoneyOrbUpdateVisibility(MoneyOrb MoneyOrb, bool visible) => onMoneyOrbUpdateVisibility?.Invoke(MoneyOrb, visible);

        public event Action<MoneyOrb> onMoneyOrbDeath;
        public void MoneyOrbDeath(MoneyOrb moneyOrb) => onMoneyOrbDeath?.Invoke(moneyOrb);

        public event Action<MoneyOrb> onMoneyOrbDecay;
        public void MoneyOrbDecay(MoneyOrb moneyOrb) => onMoneyOrbDecay?.Invoke(moneyOrb);
        #endregion
    }
}

