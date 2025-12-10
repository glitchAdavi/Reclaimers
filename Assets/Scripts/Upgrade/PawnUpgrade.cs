using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PawnUpgrade", menuName = "Upgrades/PawnUpgrade")]
public class PawnUpgrade : Upgrade
{
    public Stat scale = new Stat(0f, 0f, 0f);

    public Stat materialGain = new Stat(0f, 0f, 0f);

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

    public Stat knockBackResist = new Stat(0f, 0f, 0f);

    public override void Apply(PlayablePawn p)
    {
        if (scale.IsNotEmpty())
        {
            p.statBlock.scale.ModifyValues(scale);
            p.ApplyScale();
        }

        if (materialGain.IsNotEmpty())
        {
            p.statBlock.materialGain.ModifyValues(materialGain);
            p.ApplyMaterialGain();
        }

        if (xpGain.IsNotEmpty())
        {
            p.statBlock.xpGain.ModifyValues(xpGain);
            p.ApplyXpGain();
        }

        if (speed.IsNotEmpty())
        {
            p.statBlock.speed.ModifyValues(speed);
            p.ApplySpeed();
        }

        if (lifepoints.IsNotEmpty())
        {
            p.statBlock.lifepoints.ModifyValues(lifepoints);
            p.ApplyLifepoints();
        }

        if (lpRegen.IsNotEmpty())
        {
            p.statBlock.lpRegen.ModifyValues(lpRegen);
            p.ApplyLpRegen();
        }

        if (lpRegenDelay.IsNotEmpty())
        {
            p.statBlock.lpRegenDelay.ModifyValues(lpRegenDelay);
            p.ApplyLpRegenDelay();
        }

        if (lpRegenTickTime.IsNotEmpty())
        {
            p.statBlock.lpRegenTickTime.ModifyValues(lpRegenTickTime);
            p.ApplyLpRegenTickTime();
        }

        if (damageMultiplier.IsNotEmpty())
        {
            p.statBlock.damageMultiplier.ModifyValues(damageMultiplier);
            p.ApplyDamageMultiplier();
        }

        if (healingMultiplier.IsNotEmpty())
        {
            p.statBlock.healingMultiplier.ModifyValues(healingMultiplier);
            p.ApplyHealingMultiplier();
        }

        if (meleeDamage.IsNotEmpty())
        {
            p.statBlock.meleeDamage.ModifyValues(meleeDamage);
            p.ApplyMeleeDamage();
        }

        if (meleeCooldown.IsNotEmpty())
        {
            p.statBlock.meleeCooldown.ModifyValues(meleeCooldown);
            p.ApplyMeleeCooldown();
        }

        if (pickUpRange.IsNotEmpty())
        {
            p.statBlock.pickUpRange.ModifyValues(pickUpRange);
            p.ApplyPickUpRange();
        }

        if (interactionRange.IsNotEmpty())
        {
            p.statBlock.interactionRange.ModifyValues(interactionRange);
            p.ApplyInteractionRange();
        }

        if (iFrameDuration.IsNotEmpty())
        {
            p.statBlock.iFrameDuration.ModifyValues(iFrameDuration);
            p.ApplyIFrameDuration();
        }

        if (knockBackResist.IsNotEmpty())
        {
            p.statBlock.knockBackResist.ModifyValues(knockBackResist);
            p.ApplyKnockbackResist();
        }

    }

    public override void Remove(PlayablePawn p)
    {
        if (scale.IsNotEmpty())
        {
            p.statBlock.scale.ModifyValues(-scale);
            p.ApplyScale();
        }

        if (materialGain.IsNotEmpty())
        {
            p.statBlock.materialGain.ModifyValues(-materialGain);
            p.ApplyMaterialGain();
        }

        if (xpGain.IsNotEmpty())
        {
            p.statBlock.xpGain.ModifyValues(-xpGain);
            p.ApplyXpGain();
        }

        if (speed.IsNotEmpty())
        {
            p.statBlock.speed.ModifyValues(-speed);
            p.ApplySpeed();
        }

        if (lifepoints.IsNotEmpty())
        {
            p.statBlock.lifepoints.ModifyValues(-lifepoints);
            p.ApplyLifepoints();
        }

        if (lpRegen.IsNotEmpty())
        {
            p.statBlock.lpRegen.ModifyValues(-lpRegen);
            p.ApplyLpRegen();
        }

        if (lpRegenDelay.IsNotEmpty())
        {
            p.statBlock.lpRegenDelay.ModifyValues(-lpRegenDelay);
            p.ApplyLpRegenDelay();
        }

        if (lpRegenTickTime.IsNotEmpty())
        {
            p.statBlock.lpRegenTickTime.ModifyValues(-lpRegenTickTime);
            p.ApplyLpRegenTickTime();
        }

        if (damageMultiplier.IsNotEmpty())
        {
            p.statBlock.damageMultiplier.ModifyValues(-damageMultiplier);
            p.ApplyDamageMultiplier();
        }

        if (healingMultiplier.IsNotEmpty())
        {
            p.statBlock.healingMultiplier.ModifyValues(-healingMultiplier);
            p.ApplyHealingMultiplier();
        }

        if (meleeDamage.IsNotEmpty())
        {
            p.statBlock.meleeDamage.ModifyValues(-meleeDamage);
            p.ApplyMeleeDamage();
        }

        if (meleeCooldown.IsNotEmpty())
        {
            p.statBlock.meleeCooldown.ModifyValues(-meleeCooldown);
            p.ApplyMeleeCooldown();
        }

        if (pickUpRange.IsNotEmpty())
        {
            p.statBlock.pickUpRange.ModifyValues(-pickUpRange);
            p.ApplyPickUpRange();
        }

        if (interactionRange.IsNotEmpty())
        {
            p.statBlock.interactionRange.ModifyValues(-interactionRange);
            p.ApplyInteractionRange();
        }

        if (iFrameDuration.IsNotEmpty())
        {
            p.statBlock.iFrameDuration.ModifyValues(-iFrameDuration);
            p.ApplyIFrameDuration();
        }

        if (knockBackResist.IsNotEmpty())
        {
            p.statBlock.knockBackResist.ModifyValues(-knockBackResist);
            p.ApplyKnockbackResist();
        }
    }
}
