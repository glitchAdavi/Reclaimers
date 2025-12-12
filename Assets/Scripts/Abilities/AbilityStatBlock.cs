using UnityEngine;

[CreateAssetMenu(fileName = "AbilityStatBlock", menuName = "Stat Block/AbilityStatBlock")]
public class AbilityStatBlock : ScriptableObject
{
    public Rarity rarity = Rarity.Common;

    public string internalName = "ability";

    public string abilityName = "Ability";
    public string abilityDescription = "An ability.";
    public string abilityType = "A_grenade";

    public Modifier abilityModifier;

    public Stat abilityCooldown = new Stat(1f, 0f, 1f);

    public Stat abilityDamage = new Stat(1f, 0f, 1f);
    public Stat abilityRadius = new Stat(0f, 0f, 1f);

    public Stat abilityCharges = new Stat(1f, 0f, 1f);


    public void CopyValues(AbilityStatBlock asb)
    {
        rarity = asb.rarity;

        internalName = asb.internalName;

        abilityName = asb.abilityName;
        abilityDescription = asb.abilityDescription;
        abilityType = asb.abilityType;

        abilityModifier = asb.abilityModifier;

        abilityCooldown.SetValues(asb.abilityCooldown);

        abilityDamage.SetValues(asb.abilityDamage);
        abilityRadius.SetValues(asb.abilityRadius);

        abilityCharges.SetValues(asb.abilityCharges);
    }
}
