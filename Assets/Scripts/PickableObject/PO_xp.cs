using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PO_xp : PickableObject
{
    [SerializeField] protected float xpValue = 1;

    public void Init(float xp)
    {
        xpValue = xp;
    }

    protected override void Effect()
    {
        if (target != null)
        {
            if (xpValue > 0) GameManager.current.eventService.GivePlayerXp(xpValue);
        }
    }

    protected override void Die()
    {
        GameManager.current.levelService.RemoveXpFromList(this);
        base.Die();
    }

    protected override void ManageTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            Effect();
            Die();
        }
    }

    public float GetXpValue()
    {
        return xpValue;
    }
}
