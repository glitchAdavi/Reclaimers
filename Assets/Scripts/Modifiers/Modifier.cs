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
    public float valueATotal;
    public float valueMTotal;
}

public enum Quality
{
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legendary = 5
}

public abstract class Modifier : ScriptableObject, ICloneable
{
    public Quality quality = Quality.Common;

    public string modName = "";
    public string modDesc = "";
    public string modId = "";

    public Sprite modImage;

    public List<StatVarStruct> statVarsToModify = new List<StatVarStruct>();

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
                aux.ModAddTotal(-stat.valueATotal * (int)quality);
                aux.ModMultTotal(-stat.valueMTotal * (int)quality);
            }
            else
            {
                aux.ModB(stat.valueB * (int)quality);
                aux.ModMult(stat.valueM * (int)quality);
                aux.ModAddTotal(stat.valueATotal * (int)quality);
                aux.ModMultTotal(stat.valueMTotal * (int)quality);
            }
            param.SetValue(target.statBlock, aux);
        }
    }

    public object Clone()
    {
        Modifier mod = Instantiate(this);
        return mod;
    }
}
