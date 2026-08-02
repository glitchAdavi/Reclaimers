using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatBlock")]
public class StatBlock : ScriptableObject, ICloneable
{
    public bool firstTime = true;

    #region Pawn
    [Header("Pawn Stats")]
    [Header("")]
    [PawnVar] public string pName = "Pawn";
    [PawnVar] public string pDesc = "A Pawn.";

    [PawnVar] public List<StatUpgrade> upgrades = new List<StatUpgrade>();
    [PawnVar] public List<ItemUpgrade> items = new List<ItemUpgrade>();

    [PawnVar] public string weaponBlock;
    [PawnVar] public ProjectileType projType;
    [PawnVar] public List<StatOnHitEffect> onHitEffects = new List<StatOnHitEffect>();

    [PawnVar] public string abilityBlock;

    // 0 - Head
    // 1 - Torso
    // 2 - Legs
    // 3 - GeneralDetails
    [PawnVar] public PawnType sprCat = PawnType.Player;
    [PawnVar] public List<string> sprNames = new List<string>();

    [PawnVar] public bool spriteVariableHue = false;
    [PawnVar] public int spriteVariableHueRange = 0;

    [PawnVar] public Sprite hitSprite;

    [PawnVar] public Stat scale = new Stat(0f, 0f, 0.1f);

    [PawnVar] public Stat xpTotal = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat xpCur = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat xpGain = new Stat(0f, 0f, 0f);

    [PawnVar] public Stat level = new Stat(0f, 0f, 0f);

    [PawnVar] public Stat money = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat moneyGain = new Stat(0f, 0f, 0f);

    [PawnVar] public Stat speed = new Stat(0f, 0f);

    [PawnVar] public Stat hp = new Stat(0f, 0f, 1f);

    [PawnVar] public Stat hpRegen = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat hpRegenDelay = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat hpRegenTickTime = new Stat(0f, 0f, 0f);

    [PawnVar] public Stat iFrameDuration = new Stat(0f, 0f);

    [PawnVar] public Stat damageModifier = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat healingModifier = new Stat(0f, 0f, 0f);

    [PawnVar] public Stat meleeDamage = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat meleeRange = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat meleeCooldown = new Stat(0f, 0f, 0f);

    [PawnVar] public Stat critChance = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat critMult = new Stat(0f, 0f, 0f);

    [PawnVar] public Stat pickUpRange = new Stat(0f, 0f, 0f);
    [PawnVar] public Stat interactionRange = new Stat(0f, 0f, 0f);

    // Enemy Exclusive
    [PawnVar] public bool isBoss = false;

    [PawnVar] public bool spawnsIdle = false;
    [PawnVar] public List<Sprite> idleDisguises = new List<Sprite>();

    [PawnVar] public bool immobile = false;

    [PawnVar] public bool xpInstant = false;
    [PawnVar] public Stat xpKillValue = new Stat(0f, 0f, 0f);
    [PawnVar] public bool moneyInstant = false;
    [PawnVar] public Stat moneyKillValue = new Stat(0f, 0f, 0f);

    [PawnVar] public bool onDamageSpawnEnemy;
    [PawnVar] public string onDamageEnemyToSpawn;
    [PawnVar] public Stat onDamageInterval = new Stat(0f, 0f, 1f);
    [PawnVar] public Stat onDamageNumToSpawn = new Stat(0f, 0f, 1f);

    [PawnVar] public bool onDeathSpawnEnemy;
    [PawnVar] public string onDeathEnemyToSpawn;
    [PawnVar] public Stat onDeathNumToSpawn = new Stat(0f, 0f, 1f);
    #endregion

    #region Weapon
    [Header("")]
    [Header("Weapon Stats")]
    [Header("")]
    [WeaponVar] public string weaponName = "";

    [WeaponVar] public Sprite wepSprite;

    [WeaponVar] public WeaponType wepType;

    [WeaponVar] public Stat magSize = new Stat(0f, 0f, 1f);

    [WeaponVar] public Stat bulletsPerShot = new Stat(0f, 0f, 1f);
    [WeaponVar] public Stat bulletsPerShotCost = new Stat(0f, 0f, 1f);
    [WeaponVar] public Stat bulletsPerShotSpread = new Stat(0f, 0f, 0f);
    [WeaponVar] public Stat bulletsPerShotFirstBulletSpread = new Stat(0f, 0f, 0f);

    [WeaponVar] public Stat burstAmount = new Stat(0f, 0f, 0f);
    [WeaponVar] public Stat burstDelay = new Stat(0f, 0f, 0f);

    [WeaponVar] public Stat bulletSpread = new Stat(0f, 0f, 0f);
    [WeaponVar] public Stat bulletSpreadGain = new Stat(0f, 0f, 0f);

    [WeaponVar] public Stat fireRate = new Stat(0f, 0f, 0f);

    [WeaponVar] public Stat reloadTime = new Stat(0f, 0f, 0f);

    [WeaponVar] public Stat projDamage = new Stat(0f, 0f, 0f);
    [WeaponVar] public Stat projDamageRadius = new Stat(0f, 0f, 0f);

    [WeaponVar] public Stat projSpeed = new Stat(0f, 0f, 0f, 50f);

    [WeaponVar] public Stat projPenetration = new Stat(0f, 0f, 0f);

    [WeaponVar] public Stat projStun = new Stat(0f, 0f, 0f);

    #endregion

    #region Ability
    [Header("")]
    [Header("Ability Stats")]
    [Header("")]
    [AbilityVar] public string abilityName = "";
    [AbilityVar] public AbilityScripts abilityScript;

    [AbilityVar] public StatusEffect seToApply;
    [AbilityVar] public GameObject goToInstantiate;

    [AbilityVar] public Stat abilityDuration = new Stat(0f, 0f, 0f);

    [AbilityVar] public Stat abilityCooldown = new Stat(0f, 0f, 0f);

    [AbilityVar] public Stat abilityDamage = new Stat(0f, 0f, 0f);
    [AbilityVar] public Stat abilityRadius = new Stat(0f, 0f, 0f);

    [AbilityVar] public Stat abilityCharges = new Stat(0f, 0f, 0f);

    #endregion

    public object Clone()
    {
        StatBlock sb = Instantiate(this);
        return sb;
    }
}
