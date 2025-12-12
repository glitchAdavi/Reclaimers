using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "M_DoT", menuName = "Modifier/DoT")]
public class M_DoT : Modifier
{
    public override void OnModifierTick()
    {
        target.GetHit(value, false);
    }

    public override void OnModifierEnd()
    {
        base.OnModifierEnd();
    }
}
