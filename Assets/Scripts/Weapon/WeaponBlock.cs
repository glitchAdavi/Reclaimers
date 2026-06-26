using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBlock")]
public class WeaponBlock : ScriptableObject, ICloneable
{
    public string weaponName = "";

    public WeaponCollection wepType;
    public ProjectileCollection projType;

    public Stat magSize = new Stat(1f, 1f, 1f);

    public Stat bulletsPerShot = new Stat(1f, 1f, 1f);
    public Stat bulletsPerShotCost = new Stat(1f, 1f, 1f);
    public Stat bulletsPerShotSpread = new Stat(30f, 1f, 0f);

    public Stat burstAmount = new Stat(1f, 1f, 0f);
    public Stat burstDelay = new Stat(0f, 1f, 0f);

    public Stat bulletSpread = new Stat(5f, 1f, 0f);
    public Stat bulletSpreadGain = new Stat(1f, 1f, 0f);

    public Stat fireRate = new Stat(1f, 1f, 0f);

    public Stat reloadTime = new Stat(1f, 1f, 0f);

    public Stat projDamage = new Stat(1f, 1f, 0f);
    public Stat projDamageRadius = new Stat(0f, 0f, 0f);

    public Stat projSpeed = new Stat(1f, 1f, 0f, 50f);

    public Stat projPenetration = new Stat(0f, 1f, 0f);

    public Stat projStun = new Stat(0f, 1f, 0f);

    public object Clone()
    {
        WeaponBlock wb = CreateInstance<WeaponBlock>();
        Utilities.CopyValues(this, wb);
        return wb;
    }
}
