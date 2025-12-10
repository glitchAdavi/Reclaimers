using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityUpgrade", menuName = "Upgrades/AbilityUpgrade")]
public class AbilityUpgrade : Upgrade
{
    public Stat abilityCooldown = new Stat(0f, 0f, 0f);

    public Stat abilityDamage = new Stat(0f, 0f, 0f);
    public Stat abilityRadius = new Stat(0f, 0f, 0f);

    public Stat abilityCharges = new Stat(0f, 0f, 0f);

    public override void Apply(PlayablePawn p)
    {
        if (abilityCooldown.IsNotEmpty())
        {
            p.equippedAbility.statBlock.abilityCooldown.ModifyValues(abilityCooldown);
            p.equippedAbility.ApplyCooldown();
        }

        if (abilityDamage.IsNotEmpty())
        {
            p.equippedAbility.statBlock.abilityDamage.ModifyValues(abilityDamage);
            p.equippedAbility.ApplyDamage();
        }

        if (abilityRadius.IsNotEmpty())
        {
            p.equippedAbility.statBlock.abilityRadius.ModifyValues(abilityRadius);
            p.equippedAbility.ApplyRadius();
        }

        if (abilityCharges.IsNotEmpty())
        {
            p.equippedAbility.statBlock.abilityCharges.ModifyValues(abilityCharges);
            p.equippedAbility.ApplyCharges();
        }
    }

    public override void Remove(PlayablePawn p)
    {
        if (abilityCooldown.IsNotEmpty())
        {
            p.equippedAbility.statBlock.abilityCooldown.ModifyValues(-abilityCooldown);
            p.equippedAbility.ApplyCooldown();
        }

        if (abilityDamage.IsNotEmpty())
        {
            p.equippedAbility.statBlock.abilityDamage.ModifyValues(-abilityDamage);
            p.equippedAbility.ApplyDamage();
        }

        if (abilityRadius.IsNotEmpty())
        {
            p.equippedAbility.statBlock.abilityRadius.ModifyValues(-abilityRadius);
            p.equippedAbility.ApplyRadius();
        }

        if (abilityCharges.IsNotEmpty())
        {
            p.equippedAbility.statBlock.abilityCharges.ModifyValues(-abilityCharges);
            p.equippedAbility.ApplyCharges();
        }
    }
}
