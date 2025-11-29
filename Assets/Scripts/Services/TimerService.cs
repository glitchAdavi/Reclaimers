using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    public float timeLeft;
    public float lifeTime;
    public Action callback;
    public float partialTimeMax;
    public float partialTime;
    public Action partialCallback;
    public bool executePartialOnEnd;
    public bool cancelled;
    public bool isPaused;

    public Timer(float t, Action c, float p, Action pc, bool pe)
    {
        timeLeft = t;
        lifeTime = 0;
        callback = c;
        partialTimeMax = p;
        partialTime = p;
        partialCallback = pc;
        executePartialOnEnd = pe;

        cancelled = false;
        isPaused = false;
    }

    public float GetLifeTime()
    {
        return lifeTime;
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


        foreach(Timer t in runningTimers)
        {
            if (t.cancelled)
            {
                finishedTimers.Add(t);
                break;
            }

            if (t.isPaused) continue;

            if (t.partialCallback != null)
            {
                t.partialTime -= Time.deltaTime;

                if (t.partialTime <= 0)
                {
                    t.timeLeft -= t.partialTimeMax;
                    t.lifeTime += t.partialTimeMax;

                    t.partialCallback();
                    t.partialTime = t.partialTimeMax;
                }
            }
            else
            {
                t.timeLeft -= Time.deltaTime;
                t.lifeTime += Time.deltaTime;
            }

            if (t.timeLeft <= 0)
            {
                if (t.callback != null) t.callback();
                if (t.executePartialOnEnd && t.partialCallback != null) t.partialCallback();
                finishedTimers.Add(t);
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

    public Timer StartTimer(float time, Action callback)
    {
        return StartTimer(time, callback, 0, null, false);
    }
    public Timer StartTimer(float time, float partialTime, Action partialCallback, bool executePartialOnEnd = false)
    {
        return StartTimer(time, null, partialTime, partialCallback, executePartialOnEnd);
    }
    public Timer StartTimer(float time, Action callback, float partialTime, Action partialCallback, bool executePartialOnEnd = false)
    {
        Timer newTimer = new Timer(time, callback, partialTime, partialCallback, executePartialOnEnd);
        newTimers.Add(newTimer);
        return newTimer;
    }
}
