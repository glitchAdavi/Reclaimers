using UnityEngine;

public enum Difficulty
{
    Normal = 10,
    Hard = 15,
    Suicide = 20
}

public enum WeaponType
{
    Projectile
}

public enum ProjectileType
{
    P_Basic,
    P_Bounce,
    P_Split
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
    FleshGolem,
    FleshGolemSmall
}