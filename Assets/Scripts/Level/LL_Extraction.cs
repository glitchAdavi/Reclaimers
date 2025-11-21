using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LL_Extraction : LevelLogic
{
    public int levelStage = 0;

    public float levelMainProgressionMax = 60f;
    public float levelMainProgression = 0f;
    Timer timerLevelMainProgression;


    public float levelSurviveProgressionMax = 10f;
    Timer timerLevelSurviveProgression;



    public override void Activate()
    {
        Debug.Log($"Starting Extraction Mode Logic");
        GameManager.current.eventService.RequestUIUseMainMenu(false);

        base.Activate();

        GameManager.current.eventService.SetPawnServiceActive(true);

        levelStage = 1;
    }


    protected override void LogicUpdate()
    {
        switch (levelStage)
        {
            case 1:
                Stage1();
                break;
            case 2:
                Stage2();
                break;
            default:
                break;
        }
            
    }

    public void Stage1()
    {
        if (timerLevelMainProgression == null)
        {
            timerLevelMainProgression = GameManager.current.timerService.StartTimer(3600f,
                                                                                    1f,
                                                                                    () => levelMainProgression++);
        }

        if (levelMainProgression >= levelMainProgressionMax)
        {
            timerLevelMainProgression?.Cancel();
            timerLevelMainProgression = null;
            levelStage = 2;
        }
    }

    public void Stage2()
    {
        if (timerLevelSurviveProgression == null)
        {
            timerLevelSurviveProgression = GameManager.current.timerService.StartTimer(3600f,
                1f,
                () => levelSurviveProgressionMax--);
        }

        if (levelSurviveProgressionMax <= 0)
        {
            timerLevelSurviveProgression?.Cancel();
            timerLevelSurviveProgression = null;
            levelStage = -1;
            Win();
        }
    }


    protected override void Win()
    {
        GameManager.current.gameInfo.currentPlayerStatBlock.CopyValues(GameManager.current.playerPawn.statBlock);



        base.Win();
    }

    protected override void Lose()
    {
        GameManager.current.gameInfo.currentPlayerStatBlock.CopyValues(GameManager.current.gameInfo.defaultStatBlock);



        base.Lose();
    }
}
