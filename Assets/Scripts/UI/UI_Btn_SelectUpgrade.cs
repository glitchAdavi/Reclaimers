using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Btn_SelectUpgrade : MonoBehaviour
{
    [SerializeField] Upgrade currentUpgrade;

    [SerializeField] Button button;
    [SerializeField] TMP_Text upgradeName;
    [SerializeField] TMP_Text upgradeDesc;
    [SerializeField] Image upgradeRarity;
    [SerializeField] Image rarityFade;

    Timer timerRarityFade;

    private void OnEnable()
    {
        currentUpgrade = GameManager.current.GetRandomUpgrade();
        upgradeName.text = currentUpgrade.upgradeName;
        upgradeDesc.text = currentUpgrade.upgradeDesc;
        upgradeRarity.color = GameManager.current.GetRarityColor(currentUpgrade.rarity);

        ColorBlock cb = button.colors;
        cb.selectedColor = rarityFade.color;
        button.colors = cb;

        rarityFade.color = upgradeRarity.color;
        timerRarityFade = GameManager.current.timerService.StartTimer(0.5f, EnableSelect, 0.01f, Fade);
    }

    public void EnableSelect()
    {
        rarityFade.enabled = false;
        button.interactable = true;
    }

    public void Fade()
    {
        Color temp = rarityFade.color;
        temp.a -= 0.02f;
        rarityFade.color = temp;
    }

    public void SelectUpgrade()
    {
        GameManager.current.audioService.PlaySound(GameManager.current.gameInfo.acButtonPress);

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
        rarityFade.enabled = true;
        button.interactable = false;
        currentUpgrade = null;
    }
}
