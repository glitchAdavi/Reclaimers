using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatBlock", menuName = "Stat Block/WeaponStatBlock")]
public class WeaponStatBlock : ScriptableObject
{
    public string weaponName = "";
    public string weaponType = "WepProjectile";

    public Sprite weaponSprite;
    public Color weaponSpriteColor;

    public Sprite projectileSprite;
    public Color projectileSpriteColor;

    public Sprite hitSprite;
    public Color hitSpriteColor;

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
    public Stat projMaxDistance = new Stat(1f, 0f, 1f);
    public Stat projArmingDistance = new Stat(0f, 0f, 1f);
    public Stat projMaxLifetime = new Stat(1f, 0f, 1f);
    public Stat projArmingLifetime = new Stat(0f, 0f, 1f);
    public bool explodeImmediately = false;

    public Stat projCritChance = new Stat(1f, 0f, 1f);
    public Stat projCritMultiplier = new Stat(2f, 0f, 1f);

    public Stat knockback = new Stat(0f, 0f, 1f);

    public void CopyValues(WeaponStatBlock wsb)
    {
        weaponName = wsb.weaponName;
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
        projMaxDistance.SetValues(wsb.projMaxDistance);
        projArmingDistance.SetValues(wsb.projArmingDistance);
        projMaxLifetime.SetValues(wsb.projMaxLifetime);
        projArmingLifetime.SetValues(wsb.projArmingLifetime);
        explodeImmediately = wsb.explodeImmediately;

        projCritChance.SetValues(wsb.projCritChance);
        projCritMultiplier.SetValues(wsb.projCritMultiplier);

        knockback.SetValues(wsb.knockback);
    }
}
