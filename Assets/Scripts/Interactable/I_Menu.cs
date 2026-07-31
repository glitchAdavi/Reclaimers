using Unity.VisualScripting;
using UnityEngine;

public class I_Menu : Interactable
{
    public int menuToUse = 0;

    private void Awake()
    {
        useThreshold = 0f;

        IText = $"{IVerb} {IName}";
    }

    protected override void Effect()
    {
        used = false;

        switch (menuToUse)
        {
            case 0:
                GameManager.Instance.eventSystem.OpenMissionMenu();
                break;
            case 1:
                GameManager.Instance.eventSystem.OpenUpgradeMenu();
                break;
            case 2:
                GameManager.Instance.eventSystem.OpenCustomizationMenu();
                break;
            default:
                break;
        }
    }
}
