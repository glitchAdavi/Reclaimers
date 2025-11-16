using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_WeaponPickup : InteractableObject
{
    [SerializeField] protected WeaponStatBlock stats;

    private void OnEnable()
    {
        useThreshold = 0.3f;
    }

    protected override void OnFinishEffect()
    {
        GameManager.current.playerPawn.ChangeWeapon(stats);
    }
}
