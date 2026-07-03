using UnityEngine;
using Reclaimers.Systems;

public class MoneyOrb : Orb
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
            if (value > 0) EventSystem.Instance.GivePlayerMoney(value);
        }
    }

    public float GetValue()
    {
        return value;
    }

    protected override void Die()
    {
        base.Die();
        EventSystem.Instance.MoneyOrbDeath(this);
    }

    protected override void Decay()
    {
        EventSystem.Instance.MoneyOrbDecay(this);
        base.Decay();
    }

    protected override void UpdateVisibility()
    {
        base.UpdateVisibility();
        EventSystem.Instance.MoneyOrbUpdateVisibility(this, isVisible);
    }
}
