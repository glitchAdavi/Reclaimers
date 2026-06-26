using System;
using UnityEngine;

public enum AbilityBlockVars
{
    abilityCooldown,
    abilityDamage,
    abilityRadius,
    abilityCharges
}

[CreateAssetMenu(fileName = "AbilityBlock")]
public class AbilityBlock : ScriptableObject, ICloneable
{
    public string abilityName = "";
    public AbilityType abilityType;

    public Stat abilityCooldown = new Stat(1f, 1f, 0f);

    public Stat abilityDamage = new Stat(1f, 1f, 0f);
    public Stat abilityRadius = new Stat(1f, 1f, 0f);

    public Stat abilityCharges = new Stat(1f, 1f, 0f);

    public object Clone()
    {
        AbilityBlock ab = CreateInstance<AbilityBlock>();
        Utilities.CopyValues(this, ab);
        return ab;
    }
}
