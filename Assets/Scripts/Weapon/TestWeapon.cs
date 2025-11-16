using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeapon : Weapon
{
    protected override void ShootEffect()
    {
        Debug.Log("Fire!");
    }
}
