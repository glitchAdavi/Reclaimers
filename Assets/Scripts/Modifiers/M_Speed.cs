using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "M_Speed", menuName = "Modifier/Speed")]
public class M_Speed : Modifier
{
    public override void OnModifierStart()
    {
        target.statBlock.speed.SetMult(target.statBlock.speed.Mult * value);
        target.ApplySpeed();
    }

    public override void OnModifierEnd()
    {
        target.statBlock.speed.SetMult(target.statBlock.speed.Mult / value);
        target.ApplySpeed();

        base.OnModifierEnd();
    }
}
