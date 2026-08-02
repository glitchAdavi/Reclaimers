using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class UIMenu_Upgrade : UIMenu
{
    [SerializeField] TMP_Text money;

    protected override void OnOpen()
    {
        base.OnOpen();

        foreach (GameObject o in objects)
        {
            o.TryGetComponent(out UI_ShopStat ss);
            if (ss != null)
            {
                ss.myMenu = this;
                ss.Init();
            }
        }

        UpdateMoney();
    }

    public void UpdateMoney()
    {
        money.text = GameManager.Instance.playerPawn.statBlock.money.Value().ToString();

        foreach (GameObject o in objects)
        {
            o.TryGetComponent(out UI_ShopStat ss);
            if (ss != null)
            {
                ss.myMenu = this;
                ss.Init();
            }
        }
    }
}
