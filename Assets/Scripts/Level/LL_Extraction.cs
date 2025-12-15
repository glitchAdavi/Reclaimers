using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LL_Extraction : LevelLogic
{
    public LevelScript currentLevelScript;
    public int levelStage = 0;

    public float stage1ProgressionMax = 180f;
    public float stage1Progression = 0f;
    Timer timerStage1Progression;


    public float stage2ProgressionMax = 300f;
    Timer timerStage2Progression;

    public bool hasStage3 = true;
    public EnemyPawn boss;

    public bool setupStage1 = false;
    public bool setupStage2 = false;
    public bool setupStage3 = false;

    public bool triggerWin = false;
    public bool triggerLose = false;


    public override void Activate()
    {
        
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                currentLevelScript = gameObject.AddComponent<LS_1>();
                break;
            case 1:
                currentLevelScript = gameObject.AddComponent<LS_2>();
                break;
            case 2:
                currentLevelScript = gameObject.AddComponent<LS_3>();
                break;
        }

        stage1ProgressionMax = currentLevelScript.stage1Time;
        stage2ProgressionMax = currentLevelScript.stage2Time;
        hasStage3 = currentLevelScript.hasStage3;

        GameManager.current.eventService.RequestUIUseMainMenu(false);
        GameManager.current.eventService.RequestUIMapProgressionEnable(true);
        GameManager.current.eventService.RequestUIMapProgressionSetup(stage1ProgressionMax);
        GameManager.current.eventService.RequestUIMapProgression(0f);

        GameManager.current.eventService.onPlayerDeath += () => triggerLose = true;

        base.Activate();

        levelStage = 1;
    }


    protected override void LogicUpdate()
    {
        if (triggerWin)
        {
            if (GameManager.current.playerPawn.pendingLevelUps < 1)
            {
                Win();
            }
        }

        if (triggerLose) Lose();

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

        timerStage1Progression?.Pause(paused);
        timerStage2Progression?.Pause(paused);
    }

    public void SetupStage1()
    {
        setupStage1 = true;

        timerStage1Progression = GameManager.current.timerService.StartTimer(3600f,
                                                                             1f,
                                                                             () => stage1Progression++);

        GameManager.current.eventService.RequestUIMapStage1();
        currentLevelScript.StartStage1Script();
    }

    public void Stage1()
    {
        GameManager.current.eventService.RequestUIMapProgression(stage1Progression);

        if (stage1Progression >= stage1ProgressionMax)
        {
            GameManager.current.eventService.RequestUIMapProgression(stage1ProgressionMax);
            timerStage1Progression?.Cancel();
            timerStage1Progression = null;
            levelStage = 2;
        }
    }

    public void SetupStage2()
    {
        setupStage2 = true;

        timerStage2Progression = GameManager.current.timerService.StartTimer(3600f,
                                                                             1f,
                                                                             () => stage2ProgressionMax--);

        GameManager.current.eventService.RequestUIMapProgressionSetup(stage2ProgressionMax);
        GameManager.current.eventService.RequestUIMapProgression(stage2ProgressionMax);

        GameManager.current.eventService.RequestUIMapStage2();
        currentLevelScript.StartStage2Script();
    }

    public void Stage2()
    {
        GameManager.current.eventService.RequestUIMapProgression(stage2ProgressionMax);

        if (stage2ProgressionMax <= 0)
        {
            timerStage2Progression?.Cancel();
            timerStage2Progression = null;
            if (hasStage3)
            {
                levelStage = 3;
            } else
            {
                levelStage = -1;
                triggerWin = true;
            }
        }
    }

    public void SetupStage3()
    {
        setupStage3 = true;

        GameManager.current.eventService.SetPawnServiceActive(false);
        GameManager.current.eventService.RequestBossSpawn();
        GameManager.current.eventService.RequestUIMapProgressionSetup(boss.statBlock.lifepoints.Value(), Color.red);
        GameManager.current.eventService.RequestUIMapProgression(boss.GetCurrentLifepoints());

        GameManager.current.eventService.RequestUIMapStage3();
    }

    public void Stage3()
    {
        if (boss != null && !boss.IsPawnDead())
        {
            GameManager.current.eventService.RequestUIMapProgression(boss.GetCurrentLifepoints());

        } else
        {
            GameManager.current.eventService.RequestUIMapProgression(0);
            levelStage = -1;
            triggerWin = true;
        }
    }


    protected override void Win()
    {
        GameManager.current.levelService.win = true;

        GameManager.current.eventService.SetPawnServiceActive(false);
        GameManager.current.eventService.RequestKillAllEnemies();


        base.Win();
    }

    protected override void Lose()
    {
        



        base.Lose();
    }
}
