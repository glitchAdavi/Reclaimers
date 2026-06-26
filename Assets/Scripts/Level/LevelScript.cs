using Reclaimers.DeveloperConsole;
using Reclaimers.Systems;
using UnityEngine;

public abstract class LevelScript : MonoBehaviour
{
    public float stage1Time = 180f;
    public float stage2Time = 300f;
    public bool hasStage3 = true;
    public float scaling = 1f;

    Timer timerLevelScript;

    public virtual void StartStage1Script()
    {

    }

    public virtual void StartStage2Script()
    {
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(1f, Minute1);
    }

    public virtual void Minute1()
    {
        LogSystem.Log("Start Minute 1");
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(60f, Minute2);
    }

    public virtual void Minute2()
    {
        LogSystem.Log("Start Minute 2");
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(60f, Minute3);
    }

    public virtual void Minute3()
    {
        LogSystem.Log("Start Minute 3");
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(60f, Minute4);
    }

    public virtual void Minute4()
    {
        LogSystem.Log("Start Minute 4");
        timerLevelScript = GameManager.Instance.timerSystem.SetTimer(60f, Minute5);
    }

    public virtual void Minute5()
    {
        LogSystem.Log("Start Minute 5");
    }
}
