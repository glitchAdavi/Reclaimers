using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    protected float useThreshold = 1f;
    protected float useTimer = 0f;

    public bool used = false;
    public bool destroyOnUse = false;

    public virtual void Use()
    {

        if (used) return;

        useTimer += Time.deltaTime;

        if (useTimer >= useThreshold) OnFinish();
    }

    public virtual void UseReset()
    {
        useTimer = 0;

    }

    protected virtual void OnFinish()
    {
        used = true;
        useTimer = 0;
        OnFinishEffect();
    }

    protected abstract void OnFinishEffect();
}
