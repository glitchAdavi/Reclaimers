using UnityEngine;

public class A_StatusEffect : Ability
{
    StatusEffect toApply;

    protected override void Effect()
    {
        p.AddStatusEffect(toApply);
    }
}
