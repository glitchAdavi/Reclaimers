using Reclaimers.DeveloperConsole;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;

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
        public List<string> sprNames = new List<string>();

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
        public Stat bulletsPerShotFirstBulletSpread;

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

            Utilities.CopyValues(defaultSB, this);
        }
    }
}
