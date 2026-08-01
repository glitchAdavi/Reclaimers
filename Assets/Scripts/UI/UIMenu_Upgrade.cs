using UnityEngine;
using System.Collections.Generic;

public class UIMenu_Upgrade : UIMenu
{
    protected override void OnOpen()
    {
        base.OnOpen();

        foreach (GameObject o in objects)
        {
            o.TryGetComponent(out UI_ShopStat ss);
            if (ss != null) ss.Init();
        }
    }
}
