using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public struct StatVarStruct
{
    public StatBlockVars statBlockVar;
    public float valueB;
    public float valueM;
}

[Serializable]
public struct WeaponVarStruct
{
    public WeaponBlockVars weaponBlockVar;
    public float valueB;
    public float valueM;
}

[Serializable]
public struct AbilityVarStruct
{
    public AbilityBlockVars abilityBlockVar;
    public float valueB;
    public float valueM;
}

public enum Quality
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5
}

public abstract class Modifier : ScriptableObject
{
    public Quality quality = Quality.Common;

    public string modName = "";
    public string modDesc = "";
    public string modId = "";

    public List<StatVarStruct> statVarsToModify = new List<StatVarStruct>();
    public List<WeaponVarStruct> weaponVarsToModify = new List<WeaponVarStruct>();
    public List<AbilityVarStruct> abilityVarsToModify = new List<AbilityVarStruct>();



    protected Pawn target;

    public virtual void Apply(Pawn target)
    {
        this.target = target;
    }

    public virtual void Remove(Pawn target) { }

    protected void ModifyStats(bool remove = false)
    {
        if (statVarsToModify.Count > 0)
        {
            foreach (StatVarStruct svs in statVarsToModify)
            {
                ModifyStat(svs, remove);
            }
        }

        if (weaponVarsToModify.Count > 0)
        {
            foreach (WeaponVarStruct wvs in weaponVarsToModify)
            {
                ModifyStat(wvs, remove);
            }
        }

        if (abilityVarsToModify.Count > 0)
        {
            foreach (AbilityVarStruct avs in abilityVarsToModify)
            {
                ModifyStat(avs, remove);
            }
        }
    }

    protected void ModifyStat(StatVarStruct stat, bool remove = false)
    {
        FieldInfo param = typeof(StatBlock).GetField(stat.statBlockVar.ToString());
        if (param != null)
        {
            Stat aux = param.GetValue(target.statBlock) as Stat;

            if (remove)
            {
                aux.ModB(-stat.valueB * (int)quality);
                aux.ModMult(-stat.valueM * (int)quality);
            }
            else
            {
                aux.ModB(stat.valueB * (int)quality);
                aux.ModMult(stat.valueM * (int)quality);
            }
            param.SetValue(target.statBlock, aux);
        }
    }
    protected void ModifyStat(WeaponVarStruct stat, bool remove = false)
    {
        FieldInfo param = typeof(StatBlock).GetField(stat.weaponBlockVar.ToString());
        if (param != null)
        {
            Stat aux = param.GetValue(target.weaponBlock) as Stat;

            if (remove)
            {
                aux.ModB(-stat.valueB * (int)quality);
                aux.ModMult(-stat.valueM * (int)quality);
            }
            else
            {
                aux.ModB(stat.valueB * (int)quality);
                aux.ModMult(stat.valueM * (int)quality);
            }
            param.SetValue(target.weaponBlock, aux);
        }
    }
    protected void ModifyStat(AbilityVarStruct stat, bool remove = false)
    {
        FieldInfo param = typeof(StatBlock).GetField(stat.abilityBlockVar.ToString());
        if (param != null)
        {
            Stat aux = param.GetValue(target.abilityBlock) as Stat;

            if (remove)
            {
                aux.ModB(-stat.valueB * (int)quality);
                aux.ModMult(-stat.valueM * (int)quality);
            }
            else
            {
                aux.ModB(stat.valueB * (int)quality);
                aux.ModMult(stat.valueM * (int)quality);
            }
            param.SetValue(target.abilityBlock, aux);
        }
    }
}
