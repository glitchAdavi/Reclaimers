using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Reclaimers.Systems;

public class UIMenu_Pause : UIMenu
{
    public Button btnReturnToBase;

    Timer returnTimer;

    protected override void OnOpen()
    {
        base.OnOpen();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            btnReturnToBase.interactable = false;
        }
    }

    public void Stats()
    {

    }

    public void ReturnToBase()
    {
        GameManager.Instance.dataPersistenceSystem.LoadGame(!GameManager.Instance.levelSystem.safeToLeave);

        returnTimer = GameManager.Instance.timerSystem.SetTimer(0.1f, () =>
        {
            GameManager.Instance.uiSystem.FadeOut(() =>
            {
                GameManager.Instance.GoToLevel(0);
            });
        }, "UIMENU_Pause_ReturnToBase");
    }

    public void Quit()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            GameManager.Instance.dataPersistenceSystem.LoadGame(!GameManager.Instance.levelSystem.safeToLeave);
        }

        returnTimer = GameManager.Instance.timerSystem.SetTimer(0.1f, () =>
        {
            GameManager.Instance.uiSystem.FadeOut(Application.Quit);
        }, "UIMENU_Pause_ApplicationQuit");
    }
}
