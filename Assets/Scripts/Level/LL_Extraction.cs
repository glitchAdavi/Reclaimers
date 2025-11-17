using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LL_Extraction : LevelLogic
{
    public override void Activate()
    {
        Debug.Log($"Starting LL_Extraction");

        base.Activate();

        GameManager.current.eventService.SetPawnServiceActive(true);
    }
}
