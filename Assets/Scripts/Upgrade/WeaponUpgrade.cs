using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "WeaponUpgrade", menuName = "Upgrades/WeaponUpgrade")]
public class WeaponUpgrade : Upgrade
{
    public Stat clipSize = new Stat(0f, 0, 0f);

    public override void Apply(PlayablePawn p)
    {
        if (clipSize.Value() != 0)
        {
            p.equippedWeapon.statBlock.clipSize.ModifyValues(clipSize);
            p.equippedWeapon.ApplyClipSize();
        }




    }

    public override void Remove(PlayablePawn p)
    {
        if (clipSize.Value() != 0)
        {
            p.equippedWeapon.statBlock.clipSize.ModifyValues(-clipSize);
            p.equippedWeapon.ApplyClipSize();
        }




    }
}
