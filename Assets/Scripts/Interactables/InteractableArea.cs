using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableArea : MonoBehaviour, IPause
{
    public float useThreshold = 3f;

    public bool used = false;
    public bool useOnEnter = false;

    public string IAText = "Doing something";

    Timer timerUse;
    Timer timerUseAutomatic;


    public void Pause(bool paused)
    {
        timerUse?.Pause(paused);
        timerUseAutomatic?.Pause(paused);
    }

    public virtual void Use()
    {
        if (useOnEnter) return;
        if (used) return;
        if (timerUse != null) return;

        timerUse = GameManager.current.timerService.StartTimer(useThreshold,
                                                               OnFinishEffect,
                                                               0.01f,
                                                               () => GameManager.current.eventService.RequestUIUpdateInteractFill(timerUse.GetLifeTime(), useThreshold, true));
    }

    public virtual void UseReset()
    {
        if (useOnEnter) return;
        timerUse?.Cancel();
        timerUse = null;
        GameManager.current.eventService.RequestUIUpdateInteractFill(0f, 0f, false);
    }

    protected virtual void OnFinish()
    {
        GameManager.current.eventService.RequestUIUpdateInteractFill(0f, 0f, false);
        GameManager.current.eventService.RequestUIUpdateInteractText("", false);
        used = true;
        OnFinishEffect();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            GameManager.current.levelService.SetPlayerInsideAnArea(true, this);
            GameManager.current.eventService.RequestUIUpdateInteractText($"{IAText}", true, !useOnEnter);
            if (useOnEnter && timerUseAutomatic == null)
            {
                timerUseAutomatic = GameManager.current.timerService.StartTimer(useThreshold,
                                                                                            OnFinishEffect,
                                                                                            0.01f,
                                                                                            () => GameManager.current.eventService.RequestUIUpdateInteractFill(timerUseAutomatic.GetLifeTime(), useThreshold, true));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            timerUseAutomatic?.Cancel();
            timerUseAutomatic = null;
            GameManager.current.eventService.RequestUIUpdateInteractText("", false);
            GameManager.current.eventService.RequestUIUpdateInteractFill(0f, 0f, false);
            GameManager.current.levelService.SetPlayerInsideAnArea(false, null);
        }
    }

    protected abstract void OnFinishEffect();
}
