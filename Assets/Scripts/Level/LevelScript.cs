using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelScript : MonoBehaviour
{
    Timer timerLevelScript;

    public virtual void StartStage1Script()
    {
        //GameManager.current.eventService.SetPawnServiceActive(true);
        //GameManager.current.eventService.SetPawnServiceIdle(true);
    }

    public virtual void StartStage2Script()
    {
        //GameManager.current.eventService.SetPawnServiceIdle(false);
        //GameManager.current.eventService.SetPawnServiceAlert(true);


        timerLevelScript = GameManager.current.timerService.StartTimer(1f, Minute1);
    }

    public virtual void Minute1()
    {
        Debug.Log("Start Minute 1");
        timerLevelScript = GameManager.current.timerService.StartTimer(60f, Minute2);
    }

    public virtual void Minute2()
    {
        Debug.Log("Start Minute 2");
        timerLevelScript = GameManager.current.timerService.StartTimer(60f, Minute3);
    }

    public virtual void Minute3()
    {
        Debug.Log("Start Minute 3");
        timerLevelScript = GameManager.current.timerService.StartTimer(60f, Minute4);
    }

    public virtual void Minute4()
    {
        Debug.Log("Start Minute 4");
        timerLevelScript = GameManager.current.timerService.StartTimer(60f, Minute5);
    }

    public virtual void Minute5()
    {
        Debug.Log("Start Minute 5");
    }


}
