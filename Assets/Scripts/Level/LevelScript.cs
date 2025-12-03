using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelScript", menuName = "LevelScript")]
public class LevelScript : ScriptableObject
{
    Timer timerLevelScript;

    public void StartScript()
    {
        timerLevelScript = GameManager.current.timerService.StartTimer(1f, Minute1);
    }

    public void Minute1()
    {
        GameManager.current.pawnService.SetSpawnVars(1f, 1, 100);


        timerLevelScript = GameManager.current.timerService.StartTimer(60f, Minute2);
    }

    public void Minute2()
    {


        timerLevelScript = GameManager.current.timerService.StartTimer(60f, Minute3);
    }

    public void Minute3()
    {


        timerLevelScript = GameManager.current.timerService.StartTimer(60f, Minute4);
    }

    public void Minute4()
    {


        timerLevelScript = GameManager.current.timerService.StartTimer(60f, Minute5);
    }

    public void Minute5()
    {


    }


}
