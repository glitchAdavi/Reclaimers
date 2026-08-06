using Reclaimers.DeveloperConsole;
using Reclaimers.Systems;
using Reclaimers.UpdateSystem;
using UnityEngine;

public abstract class LevelScript : MonoBehaviour
{
    public float stage1Time = 120f;
    public float stage2Time = 300f;
    public bool hasStage3 = true;

    Timer timerLevelScript;

    public virtual void StartStage1Script()
    {
        GameManager.Instance.eventSystem.ToggleSpawn(true);
        GameManager.Instance.eventSystem.PopulateLevel();
    }

    public virtual void StartStage2Script()
    {
        GameManager.Instance.eventSystem.ToggleSpawn(true);
        GameManager.Instance.eventSystem.SetSafeToLeave(false);
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(1f, Minute1, "SCRIPT_StartMinute1");
    }

    public virtual void Minute1()
    {
        LogSystem.Log("Start Minute 1");
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(60f, Minute2, "SCRIPT_Minute1");
    }

    public virtual void Minute2()
    {
        LogSystem.Log("Start Minute 2");
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(60f, Minute3, "SCRIPT_Minute2");
    }

    public virtual void Minute3()
    {
        LogSystem.Log("Start Minute 3");
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(60f, Minute4, "SCRIPT_Minute3");
    }

    public virtual void Minute4()
    {
        LogSystem.Log("Start Minute 4");
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(60f, Minute5, "SCRIPT_Minute4");
    }

    public virtual void Minute5()
    {
        LogSystem.Log("Start Minute 5");
    }
}
