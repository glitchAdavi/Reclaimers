using Reclaimers.Systems;
using UnityEngine;

public class ExtractionLogic : LevelLogic
{
    public LevelScript currentLevelScript;

    public float stage1ProgressionMax = 180f;
    public float stage1Progression = 0f;

    public float stage2ProgressionMax = 300f;
    public float stage2Progression = 0f;

    Timer timerStageProgression;

    public bool hasStage3 = true;
    public Pawn boss;

    public bool triggerWin = false;
    public bool triggerLose = false;

    Timer endTimer;

    public override void Begin()
    {
        currentLevelScript = gameObject.AddComponent<Script1>();

        stage1ProgressionMax = currentLevelScript.stage1Time;
        stage2ProgressionMax = currentLevelScript.stage2Time;
        hasStage3 = currentLevelScript.hasStage3;

        GameManager.Instance.eventSystem.onPlayerDeath += StartLose;
        GameManager.Instance.eventSystem.onBossDeath += StartWin;

        base.Begin();

        SetupStage1();
    }

    protected override void LogicUpdate()
    {
        if (triggerWin) StartWin();
    }

    protected override void LogicPause(bool paused)
    {
        timerStageProgression?.Pause(paused);
    }

    public void SetupStage1()
    {
        GameManager.Instance.eventSystem.UpdateObjective("Scavenge.");
        GameManager.Instance.eventSystem.UpdateObjectiveBar(stage1Progression, stage1ProgressionMax, true);

        timerStageProgression = GameManager.Instance.timerSystem.SetTimer(stage1ProgressionMax, SetupStage2, 1f, () =>
        {
            stage1Progression++;

            GameManager.Instance.eventSystem.UpdateObjectiveBar(stage1Progression, stage1ProgressionMax, true);

            if (stage1Progression > stage1ProgressionMax) timerStageProgression.Finish();
        }, "LEVELLOGIC_Stage1", true);

        currentLevelScript.StartStage1Script();
    }

    public void SetupStage2()
    {
        GameManager.Instance.eventSystem.UpdateObjective("Survive.");
        GameManager.Instance.eventSystem.UpdateObjectiveBar(stage2Progression, stage2ProgressionMax, false);

        stage2Progression = stage2ProgressionMax;
        timerStageProgression?.Cancel();
        timerStageProgression = GameManager.Instance.timerSystem.SetTimer(stage2ProgressionMax, SetupStage3, 1f, () =>
        {
            stage2Progression--;
            
            GameManager.Instance.eventSystem.UpdateObjectiveBar(stage2Progression, stage2ProgressionMax, false);
        }, "LEVELLOGIC_Stage2", true);

        currentLevelScript.StartStage2Script();
    }

    public void SetupStage3()
    {
        if (!hasStage3) triggerWin = true;

        GameManager.Instance.eventSystem.UpdateObjective("Subdue.");

        GameManager.Instance.eventSystem.ToggleSpawn(false);
        GameManager.Instance.eventSystem.RequestBossSpawn();
    }

    protected void StartWin()
    {
        triggerWin = false;
        GameManager.Instance.eventSystem.TriggerWin();

        endTimer = GameManager.Instance.timerSystem.SetTimer(1f, Win, "LEVEL_Logic_StartWin");
    }

    protected void StartLose()
    {
        triggerLose = false;
        GameManager.Instance.eventSystem.TriggerLose();

        Lose();
    }
}
