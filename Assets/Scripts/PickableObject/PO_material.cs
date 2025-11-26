using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PO_material : PickableObject
{
    [SerializeField] protected float matValue = 1;

    public void Init(float xp)
    {
        matValue = xp;
    }

    protected override void Effect()
    {
        if (target != null)
        {
            if (matValue > 0) GameManager.current.eventService.GivePlayerMaterial(matValue);
        }
    }

    protected override void Die()
    {
        //GameManager.current.levelService.RemoveXpFromList(this);
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

    public float GetMatValue()
    {
        return matValue;
    }
}
