using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_AbilityUpgradePickup : InteractableObject
{
    [SerializeField] protected AbilityUpgrade upgrade;


    public override void Start()
    {
        base.Start();

        useThreshold = 1f;
        if (upgrade != null) IOName = upgrade.GetUpgradeName();
    }

    public void SetUpgrade(AbilityUpgrade au)
    {
        upgrade = au;
        IOName = upgrade.GetUpgradeName();
    }

    protected override void OnFinishEffect()
    {
        GameManager.current.playerPawn.AddUpgrade(upgrade);

        GameManager.current.eventService.RequestUISpawnFloatingText(GameManager.current.gameInfo.playerPositionVar.Value,
                                                                    $"+1 {upgrade.GetUpgradeName()}",
                                                                    Color.green,
                                                                    0f,
                                                                    1f);


        base.OnFinishEffect();
    }
}
