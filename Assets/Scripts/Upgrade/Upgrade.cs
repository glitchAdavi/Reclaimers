using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    [SerializeField] protected string upgradeName = "Upgrade";
    [SerializeField] protected string upgradeDesc = "Description.";

    [SerializeField] protected Stat stat1;
    [SerializeField] protected Stat stat2;
    [SerializeField] protected Stat stat3;

    [SerializeField] protected Stat stat1neg;
    [SerializeField] protected Stat stat2neg;
    [SerializeField] protected Stat stat3neg;

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
