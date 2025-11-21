using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LL_Extraction : LevelLogic
{





    public override void Activate()
    {
        Debug.Log($"Starting Extraction Mode Logic");
        GameManager.current.eventService.RequestUIUseMainMenu(false);

        base.Activate();

        GameManager.current.eventService.SetPawnServiceActive(true);
    }


    protected override void LogicUpdate()
    {
        
    }


    protected override void Win()
    {




        base.Win();
    }

    protected override void Lose()
    {




        base.Lose();
    }
}
