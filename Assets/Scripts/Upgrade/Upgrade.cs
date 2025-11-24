using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    [SerializeField] protected string upgradeName = "Upgrade";
    [SerializeField] protected string upgradeDesc = "Description.";

    public string GetUpgradeName()
    {
        return upgradeName;
    }

    public string GetUpgradeDesc()
    {
        return upgradeDesc;
    }

    public virtual void Apply(PlayablePawn p)
    {

    }

    public virtual void Remove(PlayablePawn p)
    {

    }
}
