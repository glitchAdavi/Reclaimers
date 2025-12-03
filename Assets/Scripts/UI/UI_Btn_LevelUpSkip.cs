using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Btn_LevelUpSkip : MonoBehaviour
{
    public void Skip()
    {
        GameManager.current.eventService.LevelUpFinish();
    }
}
