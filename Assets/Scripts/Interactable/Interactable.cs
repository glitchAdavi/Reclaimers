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

        timerUse = GameManager.Instance.timerSystem.SetTimer(useThreshold, OnUse, 0.05f, UsePartial, "", true);
    }

    public void UsePartial()
    {

    }

    public void ResetUse()
    {
        timerUse.Cancel();
    }

    public void OnUse()
    {
        used = true;
        Effect();
    }

    protected virtual void Effect()
    {
        if (destroyOnUse) Destroy(gameObject);
    }

    public virtual void SetMaterial(Material newMat)
    {
        if (_sr != null) _sr.material = newMat;
    }

}
