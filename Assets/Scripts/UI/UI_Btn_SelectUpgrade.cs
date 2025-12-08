using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Btn_SelectUpgrade : MonoBehaviour
{
    [SerializeField] Upgrade currentUpgrade;

    [SerializeField] TMP_Text upgradeName;
    [SerializeField] TMP_Text upgradeDesc;

    private void OnEnable()
    {
        currentUpgrade = GameManager.current.GetRandomUpgrade();
        upgradeName.text = currentUpgrade.upgradeName;
        upgradeDesc.text = currentUpgrade.upgradeDesc;
    }

    public void SelectUpgrade()
    {
        if (currentUpgrade != null)
        {
            GameManager.current.playerPawn.AddUpgrade(currentUpgrade);
        } else
        {
            Debug.Log("Upgrade is null");
        }
        GameManager.current.eventService.LevelUpFinish();
    }

    private void OnDisable()
    {
        currentUpgrade = null;
    }
}
