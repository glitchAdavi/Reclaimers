using Reclaimers.Systems;
using UnityEngine;

public abstract class StatusEffect : Modifier
{
    public Sprite statusEffectSprite;

    public float duration;
    public float tickTime;
    public bool executePartialOnEnd;

    public float value;

    Timer timerStatus;

    public override void Apply(Pawn target)
    {
        base.Apply(target);
        OnStatusEffectStart();
        timerStatus = GameManager.Instance.timerSystem.SetTimer(duration, OnStatusEffectEnd, tickTime, OnStatusEffectTick, modId, executePartialOnEnd);
    }

    public virtual void OnStatusEffectStart() { }

    public virtual void OnStatusEffectTick() { }

    public virtual void OnStatusEffectEnd()
    {
        target.RemoveStatusEffect(modId);
    }

    public virtual void ResetStatusEffect()
    {
        timerStatus.Reset();
    }

    public void SetDuration(float dur)
    {
        duration = dur;
    }
}
