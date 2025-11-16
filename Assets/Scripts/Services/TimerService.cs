using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Timer
{
    public float timeLeft;
    public Action callback;
    public float partialTimeMax;
    public float partialTime;
    public Action partialCallback;
    public bool cancelled;
    public bool isPaused;

    public Timer(float t, Action c, float p, Action pc)
    {
        timeLeft = t;
        callback = c;
        partialTimeMax = p;
        partialTime = p;
        partialCallback = pc;

        cancelled = false;
        isPaused = false;
    }

    public void Pause(bool paused)
    {
        isPaused = paused;
    }

    public void Cancel()
    {
        cancelled = true;
    }
}


public class TimerService : MonoBehaviour, IUpdate, IPause
{
    List<Timer> newTimers = new List<Timer>();
    List<Timer> runningTimers = new List<Timer>();
    List<Timer> finishedTimers = new List<Timer>();

    public int runningTimerCount = 0;

    private void OnEnable()
    {
        GameManager.current.updateService.RegisterUpdate(this);
    }

    public void ExecuteUpdate()
    {
        foreach (Timer t in newTimers)
        {
            runningTimers.Add(t);
        }

        newTimers.Clear();

        foreach (Timer t in runningTimers)
        {
            if (t.cancelled)
            {
                finishedTimers.Add(t);
                break;
            }

            if (t.isPaused) continue;

            t.timeLeft -= Time.deltaTime;
            
            if (t.timeLeft <= 0)
            {
                if (t.callback != null) t.callback();
                //if (t.partialCallback != null) t.partialCallback();
                finishedTimers.Add(t);
            }

            if (t.partialCallback != null)
            {
                t.partialTime -= Time.deltaTime;

                if (t.partialTime <= 0)
                {
                    t.partialCallback();
                    t.partialTime = t.partialTimeMax;
                }
            }
        }

        foreach (Timer t in finishedTimers)
        {
            runningTimers.Remove(t);
        }

        finishedTimers.Clear();

        runningTimerCount = runningTimers.Count;
    }

    public void Pause(bool paused)
    {
        foreach (Timer t in runningTimers)
        {
            t.Pause(paused);
        }
    }

    public Timer StartTimer(float time, Action callback = null, float partialTime = 0, Action partialCallback = null)
    {
        Timer newTimer = new Timer(time, callback, partialTime, partialCallback);
        newTimers.Add(newTimer);
        return newTimer;
    }
}
