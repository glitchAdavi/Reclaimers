using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "WeaponUpgrade", menuName = "Upgrades/WeaponUpgrade")]
public class WeaponUpgrade : Upgrade
{
    public Sprite projectileSprite;
    public Color projectileSpriteColor = new Color(255, 255, 255, 255);
    public Sprite projectileSpriteAux;
    public Color projectileSpriteColorAux = new Color(255, 255, 255, 255);

    public GameObject projectilePrefab;

    public Stat projScale = new Stat(0f, 0f, 0f);

    public bool automatic = false;

    public Stat clipSize = new Stat(0f, 0f, 0f);
    public Stat bulletPerShot = new Stat(0f, 0f, 0f);
    public Stat bulletPerShotCost = new Stat(0f, 0f, 0f);
    public Stat bulletPerShotSpread = new Stat(0f, 0f, 0f);
    public Stat bulletSpread = new Stat(0f, 0f, 0f);
    public Stat bulletSpreadGain = new Stat(0f, 0f, 0f);
    public Stat fireRate = new Stat(0f, 0f, 0f);

    public Stat reloadTime = new Stat(0f, 0, 0f);

    public Stat projDamage = new Stat(0f, 0f, 0f);
    public Stat projDamageRadius = new Stat(0f, 0f, 0f);
    public Stat projSpeed = new Stat(0f, 0f, 0f);
    public Stat projPenetration = new Stat(0f, 0f, 0f);


    public bool projUseDistance = false;
    public Stat projMaxLifetime = new Stat(0f, 0f, 0f);
    public Stat projArmingLifetime = new Stat(0f, 0f, 0f);
    public bool explodeImmediately = false;
    public Stat explosionRadius = new Stat(0f, 0f, 0f);

    public Stat projCritChance = new Stat(0f, 0f, 0f);
    public Stat projCritMultiplier = new Stat(0f, 0f, 0f);

    public Stat knockback = new Stat(0f, 0f, 0f);

    public override void Apply(PlayablePawn p)
    {
        if (projectileSprite != null)
        {
            p.equippedWeapon.projSprite = projectileSprite;
            p.equippedWeapon.projSpriteColor = projectileSpriteColor;
        }

        if (projectileSpriteAux != null)
        {
            p.equippedWeapon.projSpriteAux = projectileSpriteAux;
            p.equippedWeapon.projSpriteAuxColor = projectileSpriteColorAux;
        }

        if (projectilePrefab != null)
        {
            GameManager.current.SetNewProjectile(projectilePrefab);
        }

        if (projScale.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projScale.ModifyValues(clipSize);
        }

        if (clipSize.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.clipSize.ModifyValues(clipSize);
            p.equippedWeapon.ApplyClipSize();
        }

        if (bulletPerShot.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletPerShot.ModifyValues(bulletPerShot);
            p.equippedWeapon.ApplyBulletsPerShot();
        }

        if (bulletPerShotCost.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletPerShotCost.ModifyValues(bulletPerShotCost);
            p.equippedWeapon.ApplyBulletsPerShotCost();
        }

        if (bulletPerShotSpread.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletPerShotSpread.ModifyValues(bulletPerShotSpread);
            p.equippedWeapon.ApplyBulletsPerShotSpread();
        }

        if (bulletSpread.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletSpread.ModifyValues(bulletSpread);
            p.equippedWeapon.ApplyBulletSpread();
        }

        if (bulletSpreadGain.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletSpreadGain.ModifyValues(bulletSpreadGain);
            p.equippedWeapon.ApplyBulletSpreadGain();
        }

        if (fireRate.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.fireRate.ModifyValues(fireRate);
            p.equippedWeapon.ApplyFireRate();
        }

        if (reloadTime.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.reloadTime.ModifyValues(reloadTime);
            p.equippedWeapon.ApplyReloadTime();
        }

        if (projDamage.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projDamage.ModifyValues(projDamage);
        }

        if (projDamageRadius.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projDamageRadius.ModifyValues(projDamage);
        }

        if (projSpeed.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projSpeed.ModifyValues(projSpeed);
        }

        if (projPenetration.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projPenetration.ModifyValues(projPenetration);
        }

        if (projUseDistance)
        {
            p.equippedWeapon.statBlock.projUseDistance = projUseDistance;
        }

        if (projMaxLifetime.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projMaxLifetime.ModifyValues(projMaxLifetime);
        }

        if (projArmingLifetime.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projArmingLifetime.ModifyValues(projArmingLifetime);
        }

        if (explodeImmediately)
        {
            p.equippedWeapon.statBlock.explodeImmediately = explodeImmediately;
        }

        if (explosionRadius.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.explosionRadius.ModifyValues(explosionRadius);
        }

        if (projCritChance.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projCritChance.ModifyValues(projCritChance);
        }

        if (projCritMultiplier.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projCritMultiplier.ModifyValues(projCritMultiplier);
        }

        if (knockback.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.knockback.ModifyValues(knockback);
        }

    }

    public override void Remove(PlayablePawn p)
    {
        if (projectileSprite != null)
        {
            p.equippedWeapon.ApplyProjectileSprite();
        }

        if (projectileSpriteAux != null)
        {
            p.equippedWeapon.ApplyProjectileSprite();
        }

        if (projectilePrefab != null)
        {
            GameManager.current.SetNewProjectile(projectilePrefab);
        }

        if (projScale.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projScale.ModifyValues(-clipSize);
        }

        if (clipSize.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.clipSize.ModifyValues(-clipSize);
            p.equippedWeapon.ApplyClipSize();
        }

        if (bulletPerShot.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletPerShot.ModifyValues(-bulletPerShot);
            p.equippedWeapon.ApplyBulletsPerShot();
        }

        if (bulletPerShotCost.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletPerShotCost.ModifyValues(-bulletPerShotCost);
            p.equippedWeapon.ApplyBulletsPerShotCost();
        }

        if (bulletPerShotSpread.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletPerShotSpread.ModifyValues(-bulletPerShotSpread);
            p.equippedWeapon.ApplyBulletsPerShotSpread();
        }

        if (bulletSpread.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletSpread.ModifyValues(-bulletSpread);
            p.equippedWeapon.ApplyBulletSpread();
        }

        if (bulletSpreadGain.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.bulletSpreadGain.ModifyValues(-bulletSpreadGain);
            p.equippedWeapon.ApplyBulletSpreadGain();
        }

        if (fireRate.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.fireRate.ModifyValues(-fireRate);
            p.equippedWeapon.ApplyFireRate();
        }

        if (reloadTime.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.reloadTime.ModifyValues(-reloadTime);
            p.equippedWeapon.ApplyReloadTime();
        }

        if (projDamage.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projDamage.ModifyValues(-projDamage);
        }

        if (projDamageRadius.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projDamageRadius.ModifyValues(-projDamage);
        }

        if (projSpeed.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projSpeed.ModifyValues(-projSpeed);
        }

        if (projPenetration.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projPenetration.ModifyValues(-projPenetration);
        }

        if (projUseDistance)
        {
            p.equippedWeapon.statBlock.projUseDistance = !projUseDistance;
        }

        if (projMaxLifetime.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projMaxLifetime.ModifyValues(-projMaxLifetime);
        }

        if (projArmingLifetime.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projArmingLifetime.ModifyValues(-projArmingLifetime);
        }

        if (explodeImmediately)
        {
            p.equippedWeapon.statBlock.explodeImmediately = !explodeImmediately;
        }

        if (explosionRadius.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.explosionRadius.ModifyValues(-explosionRadius);
        }

        if (projCritChance.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projCritChance.ModifyValues(-projCritChance);
        }

        if (projCritMultiplier.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.projCritMultiplier.ModifyValues(-projCritMultiplier);
        }

        if (knockback.IsNotEmpty())
        {
            p.equippedWeapon.statBlock.knockback.ModifyValues(-knockback);
        }
    }
}
