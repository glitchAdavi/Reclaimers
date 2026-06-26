using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public enum StatBlockVars
{
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
    interactionRange
}

[CreateAssetMenu(fileName = "StatBlock")]
public class StatBlock : ScriptableObject, ICloneable
{
    public string pName = "Pawn";
    public string pDesc = "A Pawn.";

    public WeaponBlock weaponBlock;

    // 0 - Head
    // 1 - HeadDetails
    // 2 - Torso
    // 3 - TorsoDetails
    // 4 - Legs
    // 5 - LegsDetails
    // 6 - GeneralDetails
    public string[] sprNames = new string[7];
    public Color[] sprFrameColor = new Color[7];

    public Stat xpTotal = new Stat(0f, 1f, 0f);
    public Stat xpCur = new Stat(0f, 1f, 0f);
    public Stat xpGain = new Stat(1f, 1f, 0f);

    public Stat level = new Stat(0f, 1f, 0f);

    public Stat money = new Stat(0f, 1f, 0f);
    public Stat moneyGain = new Stat(1f, 1f, 0f);

    public Stat speed = new Stat(0f, 1f);

    public Stat hp = new Stat(100f, 1f, 1f);

    public Stat hpRegen = new Stat(0f, 1f, 0f);
    public Stat hpRegenDelay = new Stat(0f, 1f, 0f);
    public Stat hpRegenTickTime = new Stat(0f, 1f, 0f);

    public Stat iFrameDuration = new Stat(0.1f, 1f);

    public Stat damageModifier = new Stat(1f, 1f, 0f);
    public Stat healingModifier = new Stat(1f, 1f, 0f);

    public Stat meleeDamage = new Stat(0f, 1f, 0f);
    public Stat meleeRange = new Stat(0f, 1f, 0f);
    public Stat meleeCooldown = new Stat(0f, 1f, 0f);

    public Stat critChance = new Stat(0f, 1f, 0f);
    public Stat critMult = new Stat(1f, 2f, 0f);

    public Stat pickUpRange = new Stat(5f, 1f, 0f);
    public Stat interactionRange = new Stat(3f, 1f, 0f);

    public object Clone()
    {
        StatBlock sb = Instantiate(this);
        return sb;
    }
}
