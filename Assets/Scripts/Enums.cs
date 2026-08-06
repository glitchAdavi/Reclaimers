using System;
using UnityEngine;

[Serializable]
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

[Serializable]
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

[Serializable]
public class ItemUpgrade
{
    public string modId;
    public int modStacks;

    public ItemUpgrade(string id, int stacks)
    {
        modId = id;
        modStacks = stacks;
    }
}


public enum Difficulty
{
    Normal = 10,
    Hard = 15,
    Suicide = 20
}

public enum Quality
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5
}

public enum WeaponType
{
    Projectile,
    Enemy
}

public enum ProjectileType
{
    P_Basic,
    P_Bounce,
    P_Split,
    P_Enemy
}

public enum AbilityScripts
{
    A_StatusEffect,
    A_Building,
    A_Throwable
}

public enum UpgradeCategory
{
    Pawn,
    Weapon,
    Ability
}

public enum SpritePartType
{
    Head,
    Torso,
    Legs,
    Details
}

public enum PawnType
{
    Player,
    Slime,
    Zombie,
    Bug,
    StaticPlant,
    Rat,
    Mimic,
    Amalgam,
    AmalgamPart,
    BossSlime,
    BossAmalgam
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
    abilityDuration,
    abilityCooldown,
    abilityDamage,
    abilityRadius,
    abilityCharges
}