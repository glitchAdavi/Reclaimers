using UnityEngine;
using System.Collections.Generic;

namespace Reclaimers.DataPersistence
{
    [System.Serializable]
    public class GameData
    {
        public int accountLevel;
        public float accountXp;

        public List<StatUpgrade> permanentUpgrade;

        //---PlayerStatBlack---
        public bool firstTime;

        public string pName;
        public string pDesc;

        public List<StatUpgrade> upgrades;
        public List<ItemUpgrade> items;

        public string weaponBlock;
        public ProjectileType projType;

        public string abilityBlock;

        public PawnType sprCat;
        public string[] sprNames = new string[4];

        public bool spriteVariableHue;
        public int spriteVariableHueRange;

        public Stat scale;

        public Stat xpTotal;
        public Stat xpCur;
        public Stat xpGain;

        public Stat level;

        public Stat money;
        public Stat moneyGain;

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

        public Stat critChance;
        public Stat critMult;

        public Stat pickUpRange;
        public Stat interactionRange;


        public string weaponName;

        public WeaponType wepType;

        public Stat magSize;

        public Stat bulletsPerShot;
        public Stat bulletsPerShotCost;
        public Stat bulletsPerShotSpread;

        public Stat burstAmount;
        public Stat burstDelay;

        public Stat bulletSpread;
        public Stat bulletSpreadGain;

        public Stat fireRate;

        public Stat reloadTime;

        public Stat projDamage;
        public Stat projDamageRadius;

        public Stat projSpeed;

        public Stat projPenetration;

        public Stat projStun;



        public string abilityName;
        public AbilityScripts abilityScript;

        public Stat abilityDuration;

        public Stat abilityCooldown;

        public Stat abilityDamage;
        public Stat abilityRadius;

        public Stat abilityCharges;


        public GameData()
        {
            accountLevel = 0;
            accountXp = 0f;

            permanentUpgrade = new List<StatUpgrade>();

            StatBlock defaultSB = GameManager.Instance.gameInfo.defaultPlayerStatBlock;

            //---PlayerStatBlock---
            firstTime = defaultSB.firstTime;

            pName = defaultSB.pName;
            pDesc = defaultSB.pDesc;

            upgrades = defaultSB.upgrades;
            items = defaultSB.items;

            weaponBlock = defaultSB.weaponBlock;

            projType = defaultSB.projType;

            abilityBlock = defaultSB.abilityBlock;

            sprCat = defaultSB.sprCat;
            sprNames = defaultSB.sprNames;

            spriteVariableHue = defaultSB.spriteVariableHue;
            spriteVariableHueRange = defaultSB.spriteVariableHueRange;

            scale = defaultSB.scale;

            xpTotal = defaultSB.xpTotal;
            xpCur = defaultSB.xpCur;
            xpGain = defaultSB.xpGain;

            level = defaultSB.level;

            money = defaultSB.money;
            moneyGain = defaultSB.moneyGain;

            speed = defaultSB.speed;

            hp = defaultSB.hp;

            hpRegen = defaultSB.hpRegen;
            hpRegenDelay = defaultSB.hpRegenDelay;
            hpRegenTickTime = defaultSB.hpRegenTickTime;

            iFrameDuration = defaultSB.iFrameDuration;

            damageModifier = defaultSB.damageModifier;
            healingModifier = defaultSB.healingModifier;

            meleeDamage = defaultSB.meleeDamage;
            meleeRange = defaultSB.meleeRange;
            meleeCooldown = defaultSB.meleeCooldown;

            critChance = defaultSB.critChance;
            critMult = defaultSB.critMult;

            pickUpRange = defaultSB.pickUpRange;
            interactionRange = defaultSB.interactionRange;


            weaponName = defaultSB.weaponName;

            wepType = defaultSB.wepType;

            magSize = defaultSB.magSize;

            bulletsPerShot = defaultSB.bulletsPerShot;
            bulletsPerShotCost = defaultSB.bulletsPerShotCost;
            bulletsPerShotSpread = defaultSB.bulletsPerShotSpread;

            burstAmount = defaultSB.burstAmount;
            burstDelay = defaultSB.burstDelay;

            bulletSpread = defaultSB.bulletSpread;
            bulletSpreadGain = defaultSB.bulletSpreadGain;

            fireRate = defaultSB.fireRate;

            reloadTime = defaultSB.reloadTime;

            projDamage = defaultSB.projDamage;
            projDamageRadius = defaultSB.projDamageRadius;

            projSpeed = defaultSB.projSpeed;

            projPenetration = defaultSB.projPenetration;

            projStun = defaultSB.projStun;

            abilityScript = defaultSB.abilityScript;

            abilityName = defaultSB.abilityName;

            abilityDuration = defaultSB.abilityDuration;

            abilityCooldown = defaultSB.abilityCooldown;

            abilityDamage = defaultSB.abilityDamage;
            abilityRadius = defaultSB.abilityRadius;

            abilityCharges = defaultSB.abilityCharges;
        }
    }
}
