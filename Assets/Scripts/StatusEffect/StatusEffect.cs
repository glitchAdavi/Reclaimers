using Reclaimers.DataPersistence;
using Reclaimers.Systems;
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

    public float valueB;
    public float valueM;
}

public class StatusEffect : ScriptableObject
{
    protected Pawn target;

    public List<StatVarStruct> statVarsToModify = new List<StatVarStruct>();
    public List<WeaponVarStruct> weaponVarsToModify = new List<WeaponVarStruct>();
    public List<AbilityVarStruct> abilityVarsToModify = new List<AbilityVarStruct>();

    public Sprite statusEffectSprite;

    public string seName;
    public string id;
    public float duration;
    public float tickTime;
    public bool executePartialOnEnd;

    public float value;

    Timer timerStatus;

    public virtual void ApplyStatusEffect(Pawn target)
    {
        this.target = target;
        OnStatusEffectStart();
        timerStatus = GameManager.Instance.timerSystem.SetTimer(duration, OnStatusEffectEnd, tickTime, OnStatusEffectTick, id, executePartialOnEnd);
    }

    public virtual void OnStatusEffectStart() { }

    public virtual void OnStatusEffectTick() { }

    public virtual void OnStatusEffectEnd()
    {
        target.RemoveStatusEffect(id);
    }

    protected void ModifyAllStats(bool remove = false)
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
                aux.ModB(-stat.valueB);
                aux.ModMult(-stat.valueM);
            } else
            {
                aux.ModB(stat.valueB);
                aux.ModMult(stat.valueM);
            }
            param.SetValue(target.statBlock, aux);
        }
    }

    public virtual void ResetStatusEffect()
    {
        timerStatus.Reset();
    }
}
