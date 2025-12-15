using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_Shop : InteractableObject
{
    Timer timerReset;

    public override void Start()
    {
        base.Start();

        IOName = "Shop";
        IOVerb = "Open ";
        useThreshold = 0f;
    }

    protected override void OnFinish()
    {
        GameManager.current.eventService.RequestUIUpdateInteractFill(0f, 0f, false);
        GameManager.current.eventService.RequestUIUpdateInteractText("", false);
        used = true;
        useTimer = 0;
        OnFinishEffect();
    }

    protected override void OnFinishEffect()
    {
        GameManager.current.eventService.RequestUIOpenShopMenu();

        timerReset = GameManager.current.timerService.StartTimer(0.25f, () => used = false);

        base.OnFinishEffect();
    }
}
