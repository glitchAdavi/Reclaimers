using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr;

    protected float useThreshold = 1f;
    protected float useTimer = 0f;

    public bool used = false;
    public bool destroyOnUse = false;

    public string IOName = "Temp";
    public string IOVerb = "Use";

    public virtual void Start()
    {
        GameManager.current.levelService.AddInteractableObject(this);
    }

    public virtual void Use()
    {

        if (used) return;

        useTimer += Time.deltaTime;

        GameManager.current.eventService.RequestUIUpdateInteractFill(useTimer, useThreshold, true);

        if (useTimer >= useThreshold) OnFinish();
    }

    public virtual void UseReset()
    {
        useTimer = 0;

        GameManager.current.eventService.RequestUIUpdateInteractFill(0f, 0f, false);
    }

    protected virtual void OnFinish()
    {
        GameManager.current.eventService.RequestUIUpdateInteractFill(0f, 0f, false);
        GameManager.current.eventService.RequestUIUpdateInteractText("", false);
        used = true;
        useTimer = 0;
        OnFinishEffect();
        GameManager.current.levelService.RemoveInteractableObject(this);
    }

    protected virtual void OnFinishEffect()
    {
        if (destroyOnUse) Destroy(gameObject);
    }
}
