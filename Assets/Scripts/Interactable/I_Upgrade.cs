using Unity.VisualScripting;
using UnityEngine;

public class I_Upgrade : Interactable
{
    [SerializeField] protected Upgrade u;

    private void Awake()
    {
        destroyOnUse = true;
        useThreshold = 0.2f;

        u = GameManager.Instance.GetRandomUpgrade(false);

        switch (u.uCat)
        {
            case UpgradeCategory.Pawn:
                _sr.sprite = GameManager.Instance.gameInfo.upgradePawn;
                break;
            case UpgradeCategory.Weapon:
                _sr.sprite = GameManager.Instance.gameInfo.upgradeWeapon;
                break;
            case UpgradeCategory.Ability:
                _sr.sprite = GameManager.Instance.gameInfo.upgradeAbility;
                break;
        }

        IName = u.modName;
        IVerb = "Get";

        IText = $"{IVerb} {IName}";
    }

    protected override void Effect()
    {
        GameManager.Instance.playerPawn.AddUpgrade(u);

        base.Effect();
    }
}
