using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_WeaponPickup : InteractableObject
{
    [SerializeField] protected WeaponStatBlock stats;



    public override void Start()
    {
        base.Start();

        IOVerb = "Switch to ";
        useThreshold = 0.3f;
    }

    protected override void OnFinishEffect()
    {
        GameManager.current.playerPawn.ChangeWeapon(stats);
    }
}
