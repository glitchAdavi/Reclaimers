using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatBlock", menuName = "Stat Block/WeaponStatBlock")]
public class WeaponStatBlock : ScriptableObject
{
    public Rarity rarity = Rarity.Common;

    public string internalName = "weapon";

    public string weaponName = "Weapon";
    public string weaponDescription = "A weapon.";
    public string weaponType = "WepProjectile";

    public Sprite weaponSprite;
    public Color weaponSpriteColor = new Color(255, 255, 255, 255);

    public Sprite projectileSprite;
    public Color projectileSpriteColor = new Color(255, 255, 255, 255);

    public Sprite hitSprite;
    public Color hitSpriteColor = new Color(255, 255, 255, 255);

    public GameObject projectilePrefab;

    public Stat projScale = new Stat(1f, 0f, 1f);

    public bool automatic = false;

    public Stat clipSize = new Stat(1f, 0f, 1f);
    public Stat bulletPerShot = new Stat(1f, 0f, 1f);
    public Stat bulletPerShotCost = new Stat(1f, 0f, 1f);
    public Stat bulletPerShotSpread = new Stat(30f, 0f, 1f);
    public Stat bulletSpread = new Stat(5f, 0f, 1f);
    public Stat bulletSpreadGain = new Stat(1f, 0f, 1f);
    public Stat fireRate = new Stat(1f, 0f, 1f);

    public Stat reloadTime = new Stat(1f, 0, 1f);

    public Stat projDamage = new Stat(1f, 0f, 1f);
    public Stat projDamageRadius = new Stat(0f, 0f, 1f);
    public Stat projSpeed = new Stat(1f, 0f, 1f);
    public Stat projPenetration = new Stat(1f, 0f, 1f);


    public bool projUseDistance = false;
    public Stat projMaxLifetime = new Stat(1f, 0f, 1f);
    public Stat projArmingLifetime = new Stat(0f, 0f, 1f);
    public bool explodeImmediately = false;

    public Stat projCritChance = new Stat(1f, 0f, 1f);
    public Stat projCritMultiplier = new Stat(2f, 0f, 1f);

    public Stat knockback = new Stat(0f, 0f, 1f);

    public void CopyValues(WeaponStatBlock wsb)
    {
        rarity = wsb.rarity;

        internalName = wsb.internalName;

        weaponName = wsb.weaponName;
        weaponDescription = wsb.weaponDescription;
        weaponType = wsb.weaponType;

        weaponSprite = wsb.weaponSprite;
        weaponSpriteColor = wsb.weaponSpriteColor;

        projectileSprite = wsb.projectileSprite;
        projectileSpriteColor = wsb.projectileSpriteColor;

        hitSprite = wsb.hitSprite;
        hitSpriteColor = wsb.hitSpriteColor;

        projectilePrefab = wsb.projectilePrefab;

        projScale.SetValues(wsb.projScale);

        automatic = wsb.automatic;

        clipSize.SetValues(wsb.clipSize);
        bulletPerShot.SetValues(wsb.bulletPerShot);
        bulletPerShotCost.SetValues(wsb.bulletPerShotCost);
        bulletPerShotSpread.SetValues(wsb.bulletPerShotSpread);
        bulletSpread.SetValues(wsb.bulletSpread);
        bulletSpreadGain.SetValues(wsb.bulletSpreadGain);
        fireRate.SetValues(wsb.fireRate);

        reloadTime.SetValues(wsb.reloadTime);

        projDamage.SetValues(wsb.projDamage);
        projDamageRadius.SetValues(wsb.projDamageRadius);
        projSpeed.SetValues(wsb.projSpeed);
        projPenetration.SetValues(wsb.projPenetration);


        projUseDistance = wsb.projUseDistance;
        projMaxLifetime.SetValues(wsb.projMaxLifetime);
        projArmingLifetime.SetValues(wsb.projArmingLifetime);
        explodeImmediately = wsb.explodeImmediately;

        projCritChance.SetValues(wsb.projCritChance);
        projCritMultiplier.SetValues(wsb.projCritMultiplier);

        knockback.SetValues(wsb.knockback);
    }
}
