using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class LL_Extraction : LevelLogic
{
    public LevelScript currentLevelScript;
    public int levelStage = 0;

    public float levelMainProgressionMax = 180f;
    public float levelMainProgression = 0f;
    Timer timerLevelMainProgression;


    public float levelSurviveProgressionMax = 300f;
    Timer timerLevelSurviveProgression;

    public bool setupStage1 = false;
    public bool setupStage2 = false;
    public bool setupStage3 = false;
    public EnemyPawn boss;

    public override void Activate()
    {
        
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                currentLevelScript = gameObject.AddComponent<LS_1>();
                break;
            case 1:
                currentLevelScript = gameObject.AddComponent<LS_1>();
                break;
            case 2:
                currentLevelScript = gameObject.AddComponent<LS_1>();
                break;
        }
        

        Debug.Log($"Starting Extraction Mode Logic");
        GameManager.current.eventService.RequestUIUseMainMenu(false);
        GameManager.current.eventService.RequestUIMapProgressionEnable(true);
        GameManager.current.eventService.RequestUIMapProgressionSetup(levelMainProgressionMax);
        GameManager.current.eventService.RequestUIMapProgression(0f);

        base.Activate();

        levelStage = 1;
    }


    protected override void LogicUpdate()
    {
        switch (levelStage)
        {
            case 1:
                if (!setupStage1) SetupStage1();
                Stage1();
                break;
            case 2:
                if (!setupStage2) SetupStage2();
                Stage2();
                break;
            case 3:
                if (!setupStage3) SetupStage3();
                Stage3();
                break;
            default:
                break;
        }
            
    }

    public override void Pause(bool paused)
    {
        isPaused = paused;

        timerLevelMainProgression?.Pause(paused);
        timerLevelSurviveProgression?.Pause(paused);
    }

    public void SetupStage1()
    {
        setupStage1 = true;

        currentLevelScript.StartStage1Script();
    }

    public void Stage1()
    {
        if (timerLevelMainProgression == null)
        {
            timerLevelMainProgression = GameManager.current.timerService.StartTimer(3600f,
                                                                                    1f,
                                                                                    () => levelMainProgression++);
        }

        GameManager.current.eventService.RequestUIMapProgression(levelMainProgression);

        if (levelMainProgression >= levelMainProgressionMax)
        {
            GameManager.current.eventService.RequestUIMapProgression(levelMainProgressionMax);
            timerLevelMainProgression?.Cancel();
            timerLevelMainProgression = null;
            levelStage = 2;
        }
    }

    public void SetupStage2()
    {
        setupStage2 = true;

        GameManager.current.eventService.RequestUIMapProgressionSetup(levelSurviveProgressionMax);
        GameManager.current.eventService.RequestUIMapProgression(levelSurviveProgressionMax);

        currentLevelScript.StartStage2Script();
    }

    public void Stage2()
    {
        if (timerLevelSurviveProgression == null)
        {
            timerLevelSurviveProgression = GameManager.current.timerService.StartTimer(3600f,
                1f,
                () => levelSurviveProgressionMax--);

        }

        GameManager.current.eventService.RequestUIMapProgression(levelSurviveProgressionMax);

        if (levelSurviveProgressionMax <= 0)
        {
            timerLevelSurviveProgression?.Cancel();
            timerLevelSurviveProgression = null;
            levelStage = 3;
        }
    }

    public void SetupStage3()
    {
        setupStage3 = true;

        GameManager.current.eventService.SetPawnServiceActive(false);
        GameManager.current.eventService.RequestBossSpawn();
        GameManager.current.eventService.RequestUIMapProgressionSetup(boss.statBlock.lifepoints.Value(), Color.red);
        GameManager.current.eventService.RequestUIMapProgression(boss.GetCurrentLifepoints());
    }

    public void Stage3()
    {
        if (boss != null && !boss.IsPawnDead())
        {
            GameManager.current.eventService.RequestUIMapProgression(boss.GetCurrentLifepoints());

        } else
        {
            levelStage = -1;
            Win();
        }
    }


    protected override void Win()
    {
        GameManager.current.eventService.SetPawnServiceActive(false);
        GameManager.current.eventService.RequestKillAllEnemies();


        base.Win();
    }

    protected override void Lose()
    {
        



        base.Lose();
    }
}
