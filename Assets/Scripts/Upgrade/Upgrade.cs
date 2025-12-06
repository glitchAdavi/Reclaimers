using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : ScriptableObject
{
    public Rarity rarity = Rarity.Common;

    public string internalName = "upgrade";

    public string upgradeName = "Upgrade";
    public string upgradeDesc = "Description.";

    public virtual void Apply(PlayablePawn p)
    {

    }

    public virtual void Remove(PlayablePawn p)
    {

    }
}
