using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PawnUpgrade", menuName = "Upgrades/PawnUpgrade")]
public class PawnUpgrade : Upgrade
{
    public Stat scale = new Stat(0f, 0f, 0f);

    public Stat totalXp = new Stat(0f, 0f, 0f);
    public Stat xp = new Stat(0f, 0f, 0f);
    public Stat xpGain = new Stat(0f, 0f, 0f);

    public Stat speed = new Stat(0f, 0f, 0f);

    public Stat lifepoints = new Stat(0f, 0f, 0f);

    public Stat lpRegen = new Stat(0f, 0f, 0f);
    public Stat lpRegenDelay = new Stat(0f, 0f, 0f);
    public Stat lpRegenTickTime = new Stat(0f, 0f, 0f);

    public Stat damageMultiplier = new Stat(0f, 0f, 0f);
    public Stat healingMultiplier = new Stat(0f, 0f, 0f);


    public Stat meleeDamage = new Stat(0f, 0f, 0f);
    public Stat meleeCooldown = new Stat(0f, 0f, 0f);


    public Stat pickUpRange = new Stat(0f, 0f, 0f);
    public Stat interactionRange = new Stat(0f, 0f, 0f);


    public Stat iFrameDuration = new Stat(0f, 0f, 0f);

    public Stat xpKillValue = new Stat(0f, 0f, 0f);


    public override void Apply(PlayablePawn p)
    {
        if (scale.Value() != 0)
        {
            p.statBlock.scale.ModifyValues(scale);
            p.ApplyScale();
        }

        if (speed.Value() != 0)
        {
            p.statBlock.speed.ModifyValues(speed);
            p.ApplySpeed();
        }




    }

    public override void Remove(PlayablePawn p)
    {
        if (scale.Value() != 0)
        {
            p.statBlock.scale.ModifyValues(-scale);
            p.ApplyScale();
        }

        if (speed.Value() != 0)
        {
            p.statBlock.speed.ModifyValues(-speed);
            p.ApplySpeed();
        }




    }
}
