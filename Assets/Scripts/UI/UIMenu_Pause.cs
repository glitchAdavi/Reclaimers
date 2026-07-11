using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenu_Pause : UIMenu
{
    public Button btnReturnToBase;

    protected override void Open()
    {
        base.Open();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            btnReturnToBase.interactable = false;
        }
    }
}
