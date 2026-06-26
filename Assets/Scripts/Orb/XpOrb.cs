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
            if (value > 0) EventSystem.Instance.GivePlayerXp(value);
        }
    }

    public float GetValue()
    {
        return value;
    }
}
