using UnityEngine;
using Reclaimers.UpdateSystem;
using Reclaimers.Systems;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _sr;

    protected float useThreshold = 1f;

    public bool used = false;
    protected bool destroyOnUse = false;

    Timer timerUse;

    public string IText = "";
    public string IName = "Name";
    public string IVerb = "Verb";

    public void Use()
    {
        if (used || timerUse) return;

        if (useThreshold > 0f)
        {
            GameManager.Instance.eventSystem.UpdateInteractFill(0, 1, true);
            timerUse = GameManager.Instance.timerSystem.SetTimer(useThreshold, OnUse, 0.01f, UsePartial, "", true);
        } else
        {
            OnUse();
        }
    }

    public void UsePartial()
    {
        GameManager.Instance.eventSystem.UpdateInteractFill(timerUse.durationElapsed / useThreshold, 1, useThreshold > 0f ? true : false);
    }

    public void ResetUse()
    {
        GameManager.Instance.eventSystem.UpdateInteractFill(0, 1, false);
        timerUse?.Cancel();
    }

    public void OnUse()
    {
        used = true;
        GameManager.Instance.eventSystem.InteractableUsed();
        Effect();
    }

    protected virtual void Effect()
    {
        if (destroyOnUse) Destroy(gameObject);
    }

    public virtual void Reset()
    {
        used = false;
    }

    public virtual void SetMaterial(Material newMat)
    {
        if (_sr != null) _sr.material = newMat;
    }

}
