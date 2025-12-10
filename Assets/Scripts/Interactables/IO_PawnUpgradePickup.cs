using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_PawnUpgradePickup : InteractableObject
{
    [SerializeField] protected PawnUpgrade upgrade;


    public override void Start()
    {
        base.Start();

        useThreshold = 1f;
        if (upgrade != null) IOVerb = upgrade.upgradeName;
    }

    public void SetUpgrade(PawnUpgrade pu)
    {
        upgrade = pu;
        IOVerb = upgrade.upgradeName;
    }

    protected override void OnFinishEffect()
    {
        GameManager.current.playerPawn.AddUpgrade(upgrade);
        GameManager.current.statService.AddPawnUpgradeGained();

        GameManager.current.eventService.RequestUISpawnFloatingText(GameManager.current.gameInfo.playerPositionVar.Value,
                                                                    $"+1 {upgrade.upgradeName}",
                                                                    Color.green,
                                                                    0f,
                                                                    1f);


        base.OnFinishEffect();
    }
}
