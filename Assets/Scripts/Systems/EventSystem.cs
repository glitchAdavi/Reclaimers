using System;
using UnityEngine;

namespace Reclaimers.Systems
{
    public class EventSystem : MonoBehaviour
    {
        public event Action<bool> onSetPause;
        public void SetPause(bool pause) => onSetPause?.Invoke(pause);

        public event Action<float> onGiveAccountXp;
        public void GiveAccountXp(float val) => onGiveAccountXp?.Invoke(val);

        #region UI
        public event Action<Vector3, string, Sprite, Color, float, float> onSpawnFloatingText;
        public void SpawnFloatingText(Vector3 pos, string text, Sprite image, Color c, float driftRange = 0f, float maxSize = 1f) => onSpawnFloatingText?.Invoke(pos, text, image, c, driftRange, maxSize);

        public event Action onOpenCloseMenues;
        public void OpenCloseMenues() => onOpenCloseMenues?.Invoke();

        public event Action<float, float> onUpdateHp;
        public void UpdateHp(float c, float m) => onUpdateHp?.Invoke(c, m);

        public event Action<float, float> onUpdateXp;
        public void UpdateXp(float c, float m) => onUpdateXp?.Invoke(c, m);

        public event Action<int> onUpdateLevel;
        public void UpdateLevel(int l) => onUpdateLevel?.Invoke(l);

        public event Action<float, float> onUpdateWeapon;
        public void UpdateWeapon(float c, float m) => onUpdateWeapon?.Invoke(c, m);

        public event Action<string> onUpdateWeaponName;
        public void UpdateWeaponName(string s) => onUpdateWeaponName?.Invoke(s);

        public event Action<float, float> onUpdateAbility;
        public void UpdateAbility(float c, float m) => onUpdateAbility?.Invoke(c, m);

        public event Action<string> onUpdateAbilityName;
        public void UpdateAbilityName(string s) => onUpdateAbilityName?.Invoke(s);

        public event Action<float> onUpdateKillCount;
        public void UpdateKillCount(float c) => onUpdateKillCount?.Invoke(c);

        public event Action<float> onUpdateXpGained;
        public void UpdateXpGained(float c) => onUpdateXpGained?.Invoke(c);

        public event Action<float> onUpdateMoneyGained;
        public void UpdateMoneyGained(float c) => onUpdateMoneyGained?.Invoke(c);

        public event Action<string> onUpdateObjective;
        public void UpdateObjective(string s) => onUpdateObjective?.Invoke(s);

        public event Action<float, float, bool, bool> onUpdateObjectiveBar;
        public void UpdateObjectiveBar(float c, float m, bool b, bool altC = false) => onUpdateObjectiveBar?.Invoke(c, m, b, altC);

        public event Action onEnableUITop;
        public void EnableUITop() => onEnableUITop?.Invoke();
        #endregion

        #region Level
        public event Action<float, int, int> onSetPawnSpawnVars;
        public void SetPawnSpawnVars(float i, int b, int m) => onSetPawnSpawnVars?.Invoke(i, b, m);

        public event Action onRequestClearPawnSpawns;
        public void RequestClearPawnSpawns() => onRequestClearPawnSpawns?.Invoke();

        public event Action<StatBlock, int> onRequestAddPawnSpawn;
        public void RequestAddPawnSpawn(StatBlock sb, int w) => onRequestAddPawnSpawn?.Invoke(sb, w);

        public event Action onRequestBossSpawn;
        public void RequestBossSpawn() => onRequestBossSpawn?.Invoke();

        public event Action onTriggerEnd;
        public void TriggerEnd() => onTriggerEnd?.Invoke();

        public event Action onTriggerWin;
        public void TriggerWin() => onTriggerWin?.Invoke();

        public event Action onTriggerLose;
        public void TriggerLose() => onTriggerLose?.Invoke();
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

        public event Action onQueueLevelUp;
        public void QueueLevelUp() => onQueueLevelUp?.Invoke();

        public event Action onLevelUpFinish;
        public void LevelUpFinish() => onLevelUpFinish?.Invoke();
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

