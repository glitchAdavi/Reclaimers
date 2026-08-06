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

public abstract class Modifier : ScriptableObject, ICloneable
{
    public Quality quality = Quality.Common;

    public bool qualityChangesValues = true;

    public string modName = "";
    [TextArea] public string modDesc = "";
    public string modId = "";

    public Sprite modImage;

    public List<StatVarStruct> statVarsToModify = new List<StatVarStruct>();

    protected Pawn target;

    public bool permanent = false;
    public float buyCost = 0;

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
                aux.ModB(-stat.valueB * (qualityChangesValues ? (int)quality : 1));
                aux.ModMult(-stat.valueM * (qualityChangesValues ? (int)quality : 1));
                aux.ModAddTotal(-stat.valueATotal * (qualityChangesValues ? (int)quality : 1));
                aux.ModMultTotal(-stat.valueMTotal * (qualityChangesValues ? (int)quality : 1));
            }
            else
            {
                aux.ModB(stat.valueB * (qualityChangesValues ? (int)quality : 1));
                aux.ModMult(stat.valueM * (qualityChangesValues ? (int)quality : 1));
                aux.ModAddTotal(stat.valueATotal * (qualityChangesValues ? (int)quality : 1));
                aux.ModMultTotal(stat.valueMTotal * (qualityChangesValues ? (int)quality : 1));
            }
            param.SetValue(target.statBlock, aux);
        }
    }

    public virtual string GetDesc()
    {
        return modDesc;
    }

    public object Clone()
    {
        Modifier mod = Instantiate(this);
        return mod;
    }
}
