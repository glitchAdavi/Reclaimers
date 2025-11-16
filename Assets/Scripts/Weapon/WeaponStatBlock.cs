using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStatBlock", menuName = "Stat Block/WeaponStatBlock")]
public class WeaponStatBlock : ScriptableObject
{
    public string weaponName = "";
    public string weaponType = "WepProjectile";

    public Stat clipSize = new Stat(1f, 0, 1f);
    public Stat bulletPerShot = new Stat(1f, 0f, 1f);
    public Stat fireRate = new Stat(1f, 0f, 1f);

    public Stat reloadTime = new Stat(1f, 0, 1f);

    public Stat projDamage = new Stat(1f, 0f, 1f);
    public Stat projSpeed = new Stat(1f, 0f, 1f);
    public Stat projPenetration = new Stat(1f, 0f, 1f);

    public bool projUseDistance = false;
    public Stat projMaxDistance = new Stat(1f, 0f, 1f);
    public Stat projMaxLifetime = new Stat(1f, 0f, 1f);

    public Stat knockback = new Stat(0f, 0f, 1f);

    public void CopyValues(WeaponStatBlock wsb)
    {
        weaponName = wsb.weaponName;
        weaponType = wsb.weaponType;

        clipSize.SetValues(wsb.clipSize);
        bulletPerShot.SetValues(wsb.bulletPerShot);
        fireRate.SetValues(wsb.fireRate);

        reloadTime.SetValues(wsb.reloadTime);

        projDamage.SetValues(wsb.projDamage);
        projSpeed.SetValues(wsb.projSpeed);
        projPenetration.SetValues(wsb.projPenetration);

        projUseDistance = wsb.projUseDistance;
        projMaxDistance.SetValues(wsb.projMaxDistance);
        projMaxLifetime.SetValues(wsb.projMaxLifetime);

        knockback.SetValues(wsb.knockback);
    }
}
