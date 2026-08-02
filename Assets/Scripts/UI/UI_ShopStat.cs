using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShopStat : MonoBehaviour
{
    public UIMenu_Upgrade myMenu;

    public string upgradeName;
    public Upgrade upgrade;

    public Button buy;
    public TMP_Text costText;
    public TMP_Text statName;
    public TMP_Text statMod;

    public float cost = 0f;
    public int bought = 0;
    public bool percentage = true;

    public void Init()
    {
        Init(upgradeName);
    }
    public void Init(string u)
    {
        upgrade = GameManager.Instance.GetModifier(u) as Upgrade;
        statName.text = upgrade.modName;

        int index = GameManager.Instance.permanentUpgrade.FindIndex(su => su.modId == upgrade.modId);
        if (index > -1) bought = GameManager.Instance.permanentUpgrade[index].modStacks;

        if (percentage) statMod.text = $"{(upgrade.statVarsToModify[0].valueM * bought * 100):0.##}% -> {(upgrade.statVarsToModify[0].valueM * (bought + 1) * 100):0.##}%";
        else statMod.text = $"{(upgrade.statVarsToModify[0].valueB * bought):0.##}% -> {(upgrade.statVarsToModify[0].valueB * (bought + 1)):0.##}%";

        cost = upgrade.buyCost * (bought + 1);
        costText.text = $"{cost:#.##}";

        if (GameManager.Instance.playerPawn.statBlock.money.Value() < cost) buy.interactable = false;
        else buy.interactable = true;
    }

    public void Buy()
    {
        int index = GameManager.Instance.permanentUpgrade.FindIndex(su => su.modId == upgrade.modId);
        if (index > -1) GameManager.Instance.permanentUpgrade[index].modStacks++;
        else GameManager.Instance.permanentUpgrade.Add(new StatUpgrade(upgrade.modId, 1));

        GameManager.Instance.playerPawn.RemoveMoney(cost);
        GameManager.Instance.playerPawn.AddUpgrade(upgrade, 1);

        bought++;

        cost = upgrade.buyCost * (bought + 1);
        costText.text = $"{cost:#.##}";

        if (percentage) statMod.text = $"{(upgrade.statVarsToModify[0].valueM * bought * 100):0.##}% -> {(upgrade.statVarsToModify[0].valueM * (bought + 1) * 100):0.##}%";
        else statMod.text = $"{(upgrade.statVarsToModify[0].valueB * bought):0.##}% -> {(upgrade.statVarsToModify[0].valueB * (bought + 1)):0.##}%";

        myMenu.UpdateMoney();

        if (GameManager.Instance.playerPawn.statBlock.money.Value() < cost) buy.interactable = false;
        else buy.interactable = true;
    }
}
