using UnityEngine;

[CreateAssetMenu(fileName = "AbilityStatBlock", menuName = "Stat Block/AbilityStatBlock")]
public class AbilityStatBlock : ScriptableObject
{
    public Rarity rarity = Rarity.Common;

    public string abilityName = "Ability";
    public string abilityDescription = "An ability.";
    public string abilityType = "A_grenade";

    public Stat abilityCooldown = new Stat(1f, 0f, 1f);

    public Stat abilityDamage = new Stat(1f, 0f, 1f);

    public Stat abilityCharges = new Stat(1f, 0f, 1f);


    public void CopyValues(AbilityStatBlock asb)
    {
        rarity = asb.rarity;

        abilityName = asb.abilityName;
        abilityDescription = asb.abilityDescription;
        abilityType = asb.abilityType;

        abilityCooldown.SetValues(asb.abilityCooldown);

        abilityDamage.SetValues(asb.abilityDamage);

        abilityCharges.SetValues(asb.abilityCharges);
    }
}
