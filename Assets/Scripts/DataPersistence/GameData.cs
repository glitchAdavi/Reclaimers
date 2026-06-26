using UnityEngine;

namespace Reclaimers.DataPersistence
{
    [System.Serializable]
    public class GameData
    {
        public int accountLevel;
        public float accountXp;


        //---PlayerStatBlack---
        public string pName;
        public string pDesc;

        public Stat xpTotal;
        public Stat xpCur;
        public Stat xpGain;

        public Stat level;

        public Stat money;
        public Stat materialGain;

        public Stat speed;

        public Stat hp;

        public Stat hpRegen;
        public Stat hpRegenDelay;
        public Stat hpRegenTickTime;

        public Stat iFrameDuration;

        public Stat damageModifier;
        public Stat healingModifier;

        public Stat meleeDamage;
        public Stat meleeRange;
        public Stat meleeCooldown;

        public Stat pickUpRange;
        public Stat interactionRange;


        public GameData()
        {
            accountLevel = 0;
            accountXp = 0f;


            //---PlayerStatBlock---
            pName = GameManager.Instance.gameInfo.defaultPlayerStatBlock.pName;
            pDesc = GameManager.Instance.gameInfo.defaultPlayerStatBlock.pDesc;

            xpTotal = GameManager.Instance.gameInfo.defaultPlayerStatBlock.xpTotal;
            xpCur = GameManager.Instance.gameInfo.defaultPlayerStatBlock.xpCur;
            xpGain = GameManager.Instance.gameInfo.defaultPlayerStatBlock.xpGain;

            level = GameManager.Instance.gameInfo.defaultPlayerStatBlock.level;

            money = GameManager.Instance.gameInfo.defaultPlayerStatBlock.money;
            materialGain = GameManager.Instance.gameInfo.defaultPlayerStatBlock.moneyGain;

            speed = GameManager.Instance.gameInfo.defaultPlayerStatBlock.speed;

            hp = GameManager.Instance.gameInfo.defaultPlayerStatBlock.hp;

            hpRegen = GameManager.Instance.gameInfo.defaultPlayerStatBlock.hpRegen;
            hpRegenDelay = GameManager.Instance.gameInfo.defaultPlayerStatBlock.hpRegenDelay;
            hpRegenTickTime = GameManager.Instance.gameInfo.defaultPlayerStatBlock.hpRegenTickTime;

            iFrameDuration = GameManager.Instance.gameInfo.defaultPlayerStatBlock.iFrameDuration;

            damageModifier = GameManager.Instance.gameInfo.defaultPlayerStatBlock.damageModifier;
            healingModifier = GameManager.Instance.gameInfo.defaultPlayerStatBlock.healingModifier;

            meleeDamage = GameManager.Instance.gameInfo.defaultPlayerStatBlock.meleeDamage;
            meleeRange = GameManager.Instance.gameInfo.defaultPlayerStatBlock.meleeRange;
            meleeCooldown = GameManager.Instance.gameInfo.defaultPlayerStatBlock.meleeCooldown;

            pickUpRange = GameManager.Instance.gameInfo.defaultPlayerStatBlock.pickUpRange;
            interactionRange = GameManager.Instance.gameInfo.defaultPlayerStatBlock.interactionRange;
    }
    }
}
