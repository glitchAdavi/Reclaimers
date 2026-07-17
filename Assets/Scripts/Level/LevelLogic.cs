using UnityEngine;
using Reclaimers.Systems;
using Reclaimers.UpdateSystem;

public abstract class LevelLogic : MonoBehaviour, IUpdateable, IPauseable
{
    public bool isActive = false;
    public bool isPaused = false;
    public bool ended = false;

    public virtual void Begin()
    {
        this.Register();
        isActive = true;

        GameManager.Instance.eventSystem.EnablePlayerControls(true);
    }

    public void ManagedUpdate()
    {
        if (isPaused) return;

        LogicUpdate();
    }

    public void ManagedPause(bool paused)
    {
        isPaused = paused;

        LogicPause(isPaused);
    }

    protected virtual void LogicUpdate() { }

    protected virtual void LogicPause(bool paused) { }

    protected virtual void Win()
    {
        if (ended) return;
        ended = true;

        GameManager.Instance.uiSystem.FadeOut(null, 3f, true);
    }

    protected virtual void Lose()
    {
        if (ended) return;
        ended = true;

        GameManager.Instance.uiSystem.FadeOut(null, 3f, true);
    }
}
