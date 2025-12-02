using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Modifier", menuName = "Modifier")]
public abstract class Modifier : ScriptableObject
{
    public Pawn target;

    public string modifierName;
    public string id;
    public float durationTotal;
    public float durationTick;
    public bool executePartialOnEnd;

    public float value;

    Timer timerModifier;

    public virtual void ApplyModifier(Pawn target)
    {
        this.target = target;
        OnModifierStart();
        timerModifier = GameManager.current.timerService.StartTimer(durationTotal, OnModifierEnd, durationTick, OnModifierTick, id, executePartialOnEnd);
    }

    public virtual void OnModifierStart() { }

    public virtual void OnModifierTick() { }

    public virtual void OnModifierEnd()
    {
        target.RemoveModifier(id);
    }

    public virtual void Reset()
    {
        timerModifier.Reset();
    }

}
