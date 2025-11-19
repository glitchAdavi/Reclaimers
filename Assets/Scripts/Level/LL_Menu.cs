using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LL_Menu : LevelLogic
{
    public override void Activate()
    {
        Debug.Log($"Starting Main Menu Logic");
        GameManager.current.eventService.RequestUIUseMainMenu(true);

        base.Activate();

        GameManager.current.eventService.SetPawnServiceActive(false);
    }
}
