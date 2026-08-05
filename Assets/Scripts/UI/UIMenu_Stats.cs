using TMPro;
using UnityEngine;

public class UIMenu_Stats : UIMenu
{
    [SerializeField] TMP_Text playerStats;
    [SerializeField] TMP_Text weaponStats;
    [SerializeField] TMP_Text abilityStats;

    protected override void OnOpen()
    {
        base.OnOpen();

        SetStats();
    }

    protected void SetStats()
    {
        StatBlock psb = GameManager.Instance.playerPawn.statBlock;

        playerStats.text = $"XP Gain: {psb.xpGain.ValueString(false, 1)}" +
            $"\nMaterial Gain: {psb.moneyGain.ValueString(false, 1)}" +
            $"\nSpeed: {psb.speed.ValueString(false, 1)}" +
            $"\nHealth: {psb.hp.ValueString(false, 1)}" +
            $"\nHp Regen: {psb.hpRegen.ValueString(false, 1)}" +
            $"\nDamage Mod: {psb.damageModifier.ValueString(false, 1)}" +
            $"\nHealing Mod: {psb.healingModifier.ValueString(false, 1)}" +
            $"\nCrit Chance: {psb.critChance.ValueString(false, 1)}" +
            $"\nCrit Mult: {psb.critMult.ValueString(false, 1)}" +
            $"\nPickUp Range: {psb.pickUpRange.ValueString(false, 1)}" +
            $"\nInteractionRange: {psb.interactionRange.ValueString(false, 1)}";

        weaponStats.text = $"{psb.weaponName}" +
            $"\n\nMagazine Size: {psb.magSize.ValueString(true, 1)}" +
            $"\nBullets p'\'shot: {psb.bulletsPerShot.ValueString(true, 1)}" +
            $"\nBullets p'\'shot Cost: {psb.bulletsPerShotCost.ValueString(false, 1)}" +
            $"\nBullets p'\'shot Spread: {psb.bulletsPerShotSpread.ValueString(false, 1)}" +
            $"\nBurst Amount: {psb.burstAmount.ValueString(true, 1)}" +
            $"\nBurst Delay: {psb.burstDelay.ValueString(false, 1)}" +
            $"\nBullet Spread: {psb.bulletSpread.ValueString(false, 1)}" +
            $"\nFire Rate: {psb.fireRate.ValueString(false, 1)}" +
            $"\nReload: {psb.reloadTime.ValueString(false, 1)}" +
            $"\nDamage: {psb.projDamage.ValueString(false, 1)}" +
            $"\nDamage Radius: {psb.projDamageRadius.ValueString(false, 1)}" +
            $"\nSpeed: {psb.projSpeed.ValueString(false, 1)}" +
            $"\nPenetration: {psb.projPenetration.ValueString(true, 1)}" +
            $"\nKnockback: {psb.projStun.ValueString(false, 1)}";

        abilityStats.text = $"{psb.abilityName}" +
            $"\n\nDuration: {psb.abilityDuration.ValueString(false, 1)}" +
            $"\nCooldown: {psb.abilityCooldown.ValueString(false, 1)}" +
            $"\nDamage: {psb.abilityDamage.ValueString(false, 1)}" +
            $"\nRadius: {psb.abilityRadius.ValueString(false, 1)}" +
            $"\nCharges: {psb.abilityCharges.ValueString(false, 1)}";
    }
}
