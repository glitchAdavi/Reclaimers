using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="UP_pawnSpeed", menuName="Upgrades/PawnSpeed")]
public class UP_pawnSpeed : PawnUpgrade
{
    public override void Apply(PlayablePawn p)
    {
        p.statBlock.speed.ModifyValues(stat1);
        p.ApplySpeed();
    }

    public override void Remove(PlayablePawn p)
    {
        p.statBlock.speed.ModifyValues(-stat1);
        p.ApplySpeed();
    }
}
