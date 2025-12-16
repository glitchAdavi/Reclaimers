using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Btn_LevelUpSkip : MonoBehaviour
{
    [SerializeField] public Button button;
    Timer timerEnable;

    private void OnEnable()
    {
        if (timerEnable == null) timerEnable = GameManager.current.timerService.StartTimer(0.5f, () => button.interactable = true);
    }

    public void Skip()
    {
        GameManager.current.audioService.PlaySound(GameManager.current.gameInfo.acButtonPress);
        GameManager.current.eventService.LevelUpFinish();
    }

    private void OnDisable()
    {
        button.interactable = false;
        timerEnable = null;
    }
}
