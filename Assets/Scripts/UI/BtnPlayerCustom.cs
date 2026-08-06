using UnityEngine;
using UnityEngine.UI;

public class BtnPlayerCustom : MonoBehaviour
{
    public UIMenu_Customization menu;
    public Image partSprite;
    public string partName;

    public void ChangeString()
    {
        GameManager.Instance.audioSystem.PlaySound(GameManager.Instance.gameInfo.buttonPressAudio);

        switch (menu.currentBtnClass)
        {
            case BtnClass.head:
                menu.headString = partName;
                menu.preview.enabled = true;
                menu.preview.sprite = menu.SetSprites();
                break;
            case BtnClass.torso:
                menu.torsoString = partName;
                menu.preview.enabled = true;
                menu.preview.sprite = menu.SetSprites();
                break;
            case BtnClass.legs:
                menu.legsString = partName;
                menu.preview.enabled = true;
                menu.preview.sprite = menu.SetSprites();
                break;
            case BtnClass.accesory:
                menu.accesoryString = partName;
                menu.preview.enabled = true;
                menu.preview.sprite = menu.SetSprites();
                break;
            case BtnClass.ability:
                menu.abilityString = partName;
                break;
            case BtnClass.weapon:
                menu.weaponString = partName;
                menu.previewWeapon.enabled = true;
                menu.previewWeapon.sprite = partSprite.sprite;
                break;
            default:
                break;
        }
    }
}
