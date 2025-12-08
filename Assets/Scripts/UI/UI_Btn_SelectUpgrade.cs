using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Btn_SelectUpgrade : MonoBehaviour
{
    Upgrade currentUpgrade;

    private void OnEnable()
    {
        currentUpgrade = GameManager.current.GetRandomUpgrade();
    }

    public void SelectUpgrade()
    {
        if (currentUpgrade != null)
        {
            GameManager.current.playerPawn.AddUpgrade(currentUpgrade);
        }
    }

    private void OnDisable()
    {
        currentUpgrade = null;
    }
}
