using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_WeaponUpgradePickup : InteractableObject
{
    [SerializeField] protected WeaponUpgrade upgrade;


    public override void Start()
    {
        base.Start();

        useThreshold = 1f;
        if (upgrade != null) IOName = upgrade.upgradeName;
    }

    public void SetUpgrade(WeaponUpgrade wu)
    {
        upgrade = wu;
        IOName = upgrade.upgradeName;
    }

    protected override void OnFinishEffect()
    {
        GameManager.current.playerPawn.AddUpgrade(upgrade);
        GameManager.current.statService.AddWeaponUpgradeGained();

        GameManager.current.eventService.RequestUISpawnFloatingText(GameManager.current.gameInfo.playerPositionVar.Value,
                                                                    $"+1 {upgrade.upgradeName}",
                                                                    Color.green,
                                                                    0f,
                                                                    1f);


        base.OnFinishEffect();
    }
}
