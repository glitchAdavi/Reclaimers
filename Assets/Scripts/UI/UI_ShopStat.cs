using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopStat : MonoBehaviour
{
    public Upgrade upgrade;

    public Button buy;
    public TMP_Text buttonText;
    public TMP_Text statName;
    public TMP_Text statMod;

    public float cost = 0f;
    public bool percentage = true;

    public void Init(string u)
    {
        upgrade = GameManager.current.GetUpgradeByName(u);
        statName.text = upgrade.upgradeName;

        int bought = 0;
        if (GameManager.current.permanentUpgrades.ContainsKey(u))
        {
            bought = GameManager.current.permanentUpgrades[u];
        }

        if (percentage) statMod.text = $"Next: 1% (Total: {bought.ToString("#")}%)";
        else statMod.text = $"Next: 1 (Total: {bought.ToString("#")})";

        cost = 5 + (bought * 5);

        if (GameManager.current.playerPawn.statBlock.materials.Value() < cost)
        {
            buy.interactable = false;
            buttonText.text = $"Unavailable";
        } else
        {
            buy.interactable = true;
            buttonText.text = $"Buy for {cost.ToString("#.##")}";
        }
    }

    public void Buy()
    {
        GameManager.current.audioService.PlaySound(GameManager.current.gameInfo.acButtonPress);

        if (GameManager.current.permanentUpgrades.ContainsKey(upgrade.internalName))
        {
            GameManager.current.permanentUpgrades[upgrade.internalName]++;
        } else
        {
            GameManager.current.permanentUpgrades[upgrade.internalName] = 1;
        }

        GameManager.current.playerPawn.RemoveMaterials(cost);
        GameManager.current.playerPawn.AddUpgrade(upgrade);
        GameManager.current.eventService.RequestUIOpenShopMenu();
    }
}
