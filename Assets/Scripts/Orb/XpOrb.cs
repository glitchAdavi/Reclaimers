using Reclaimers.Systems;
using UnityEngine;

public class XpOrb : Orb
{
    [SerializeField] protected float value = 1;

    public void Init(float v)
    {
        SetValue(v);
    }

    public void SetValue(float v)
    {
        value = v;

        // Change color depending value
    }

    protected override void Effect()
    {
        if (target != null)
        {
            if (value > 0) GameManager.Instance.eventSystem.GivePlayerXp(value);
        }
    }

    public float GetValue()
    {
        return value;
    }

    protected override void Die()
    {
        base.Die();
        GameManager.Instance.eventSystem.XpOrbDeath(this);
    }

    protected override void Decay()
    {
        GameManager.Instance.eventSystem.XpOrbDecay(this);
        base.Decay();
    }

    protected override void UpdateVisibility()
    {
        base.UpdateVisibility();
        GameManager.Instance.eventSystem.XpOrbUpdateVisibility(this, isVisible);
    }
}
