using UnityEngine;

[CreateAssetMenu(fileName = "PawnStatBlock", menuName = "Stat Block/PawnStatBlock")]
public class PawnStatBlock : ScriptableObject
{
    public string pawnName = "John Doe";
    public WeaponStatBlock equippedWeapon;

    public Stat scale = new Stat(1f, 0f, 1f);

    public Stat materials = new Stat(0f, 0f, 1f);
    public Stat materialGain = new Stat(1f, 0f, 1f);

    public Stat totalXp = new Stat(0f, 0f, 1f);
    public Stat xp = new Stat(0f, 0f, 1f);
    public Stat xpGain = new Stat(1f, 0f, 1f);
    public Stat level = new Stat(0f, 0f, 1f);



    public Stat speed = new Stat(1f, 0f, 1f);


    public Stat lifepoints = new Stat(100f, 0f, 1f);

    public Stat lpRegen = new Stat(0f, 0f, 1f);
    public Stat lpRegenDelay = new Stat(1f, 0f, 1f);
    public Stat lpRegenTickTime = new Stat(1f, 0f, 1f);

    public Stat damageMultiplier = new Stat(1f, 0f, 1f);
    public Stat healingMultiplier = new Stat(1f, 0f, 1f);


    public Stat meleeDamage = new Stat(1f, 0f, 1f);
    public Stat meleeCooldown = new Stat(0.5f, 0f, 1f);


    public Stat pickUpRange = new Stat(5f, 0f, 1f);
    public Stat interactionRange = new Stat(3f, 0f, 1f);


    public Stat iFrameDuration = new Stat(0.1f, 0f, 0f);

    public Stat xpKillValue = new Stat(1f, 0f, 1f);

    public void CopyValues(PawnStatBlock psb)
    {
        pawnName = psb.pawnName;
        equippedWeapon = psb.equippedWeapon;

        scale.SetValues(psb.scale);

        materials.SetValues(psb.materials);
        materialGain.SetValues(psb.materialGain);

        totalXp.SetValues(psb.totalXp);
        xp.SetValues(psb.xp);
        xpGain.SetValues(psb.xpGain);
        level.SetValues(psb.level);

        speed.SetValues(psb.speed);


        lifepoints.SetValues(psb.lifepoints);

        lpRegen.SetValues(psb.lpRegen);
        lpRegenDelay.SetValues(psb.lpRegenDelay);
        lpRegenTickTime.SetValues(psb.lpRegenTickTime);

        damageMultiplier.SetValues(psb.damageMultiplier);
        healingMultiplier.SetValues(psb.healingMultiplier);


        meleeDamage.SetValues(psb.meleeDamage);
        meleeCooldown.SetValues(psb.meleeCooldown);

        pickUpRange.SetValues(psb.pickUpRange);
        interactionRange.SetValues(psb.interactionRange);

        iFrameDuration.SetValues(psb.iFrameDuration);

        xpKillValue.SetValues(psb.xpKillValue);
    }
}
