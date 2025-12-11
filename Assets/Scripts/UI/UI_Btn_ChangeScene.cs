using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Btn_ChangeScene : MonoBehaviour
{
    public int sceneIndex = 0;

    public void ChangeScene()
    {
        GameManager.current.eventService.RequestUITogglePauseMenu(false);
        if (!GameManager.current.levelService.win) GameManager.current.ResetPlayer();
        GameManager.current.uiService.FadeOut(GameManager.current.ReturnToMenu);
    }
}
