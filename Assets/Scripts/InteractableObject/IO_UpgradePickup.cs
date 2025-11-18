using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_UpgradePickup : InteractableObject
{
    public override void OnEnable()
    {
        IOVerb = "Get ";
        useThreshold = 1f;
    }

    protected override void OnFinishEffect()
    {
        GameManager.current.eventService.RequestUISpawnFloatingText(GameManager.current.gameInfo.playerPositionVar.Value,
                                                                    $"+1 Upgrade",
                                                                    Color.green,
                                                                    0f,
                                                                    1f);
    }
}
