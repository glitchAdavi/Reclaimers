using System;
using System.Collections.Generic;
using UnityEngine;

public class StatOnHitEffect
{
    public StatusEffect effect;
    public float chance;

    public StatOnHitEffect(StatusEffect se, float chance)
    {
        effect = se;
        this.chance = chance;
    }
}

public class StatUpgrade
{
    public string modId;
    public int modStacks;

    public StatUpgrade(string id, int stacks)
    {
        modId = id;
        modStacks = stacks;
    }
}

public enum StatBlockVars
{
    scale,
    xpTotal,
    xpCur,
    xpGain,
    level,
    money,
    moneyGain,
    speed,
    hp,
    hpRegen,
    hpRegenDelay,
    hpRegenTickTime,
    iFrameDuration,
    damageModifier,
    healingModifier,
    meleeDamage,
    meleeRange,
    meleeCooldown,
    critChance,
    critMult,
    pickUpRange,
    interactionRange,
    magSize,
    bulletsPerShot,
    bulletsPerShotCost,
    bulletsPerShotSpread,
    burstAmount,
    burstDelay,
    bulletSpread,
    bulletSpreadGain,
    fireRate,
    reloadTime,
    projDamage,
    projDamageRadius,
    projSpeed,
    projPenetration,
    projStun,
    abilityCooldown,
    abilityDamage,
    abilityRadius,
    abilityCharges
}

[CreateAssetMenu(fileName = "StatBlock")]
public class StatBlock : ScriptableObject, ICloneable
{
    #region Pawn
    [Header("Pawn Stats")]
    [Header("")]
    [PawnVar] public string pName = "Pawn";
    [PawnVar] public string pDesc = "A Pawn.";

    [PawnVar] public List<StatUpgrade> upgrades = new List<StatUpgrade>();

    [PawnVar] public StatBlock weaponBlock;
    [PawnVar] public ProjectileType projType;
    [PawnVar] public List<StatOnHitEffect> onHitEffects = new List<StatOnHitEffect>();

    [PawnVar] public StatBlock abilityBlock;

    // 0 - Head
    // 1 - HeadDetails
    // 2 - Torso
    // 3 - TorsoDetails
    // 4 - Legs
    // 5 - LegsDetails
    // 6 - GeneralDetails
    [PawnVar] public string[] sprNames = new string[7];
    [PawnVar] public Color[] sprFrameColor = new Color[7];

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

    [PawnVar] public bool xpInstant = false;
    [PawnVar] public Stat xpKillValue = new Stat(0f, 0f, 0f);
    [PawnVar] public bool moneyInstant = false;
    [PawnVar] public Stat moneyKillValue = new Stat(0f, 0f, 0f);

    [PawnVar] public bool onDamageSpawnEnemy;
    [PawnVar] public StatBlock onDamageEnemyToSpawn;
    [PawnVar] public Stat onDamageInterval = new Stat(0f, 0f, 1f);
    [PawnVar] public Stat onDamageNumToSpawn = new Stat(0f, 0f, 1f);

    [PawnVar] public bool onDeathSpawnEnemy;
    [PawnVar] public StatBlock onDeathEnemyToSpawn;
    [PawnVar] public Stat onDeathNumToSpawn = new Stat(0f, 0f, 1f);
    #endregion

    #region Weapon
    [Header("")]
    [Header("Weapon Stats")]
    [Header("")]
    [WeaponVar] public string weaponName = "";

    [WeaponVar] public WeaponType wepType;

    [WeaponVar] public Stat magSize = new Stat(0f, 0f, 1f);

    [WeaponVar] public Stat bulletsPerShot = new Stat(0f, 0f, 1f);
    [WeaponVar] public Stat bulletsPerShotCost = new Stat(0f, 0f, 1f);
    [WeaponVar] public Stat bulletsPerShotSpread = new Stat(0f, 0f, 0f);

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
    [AbilityVar] public AbilityType abilityType;

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
