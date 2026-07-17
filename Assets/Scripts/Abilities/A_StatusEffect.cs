using UnityEngine;

public class A_StatusEffect : Ability
{
    [SerializeField] StatusEffect seToApply;

    public override void SetOwner(Pawn p)
    {
        base.SetOwner(p);

        seToApply = _p.statBlock.seToApply.Clone() as StatusEffect;
    }

    protected override void Effect()
    {
        seToApply.duration = _p.statBlock.abilityDuration.Value();
        _p.AddStatusEffect(seToApply);
    }
}
