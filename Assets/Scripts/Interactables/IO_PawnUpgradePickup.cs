using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_PawnUpgradePickup : InteractableObject
{
    [SerializeField] protected PawnUpgrade upgrade;


    public override void OnEnable()
    {
        IOVerb = $"Get {upgrade.GetUpgradeName()}";
        useThreshold = 1f;
    }

    protected override void OnFinishEffect()
    {
        GameManager.current.playerPawn.AddUpgrade(upgrade);

        GameManager.current.eventService.RequestUISpawnFloatingText(GameManager.current.gameInfo.playerPositionVar.Value,
                                                                    $"+1 {upgrade.GetUpgradeName()}",
                                                                    Color.green,
                                                                    0f,
                                                                    1f);
    }
}
