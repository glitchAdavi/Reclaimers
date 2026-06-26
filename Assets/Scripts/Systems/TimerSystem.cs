using Reclaimers.UpdateSystem;
using System;
using UnityEngine;

namespace Reclaimers.Systems
{
    public class Timer
    {
        public string id;

        public float duration;
        public float durationLeft;
        public float durationElapsed;
        public Action callback;

        public float step;
        public float stepLeft;
        public Action stepCallback;
        public bool stepExecuteOnEnd;

        public bool paused = false;
        public bool ended = false;

        public Timer(float t, Action c, string id)
        {
            this.id = id;
            duration = t;
            durationLeft = t;
            durationElapsed = 0;
            callback = c;

            step = 0;
            stepLeft = 0;
            stepCallback = null;
            stepExecuteOnEnd = false;
        }

        public Timer(float t, Action c, float s, Action sc, bool pe, string id)
        {
            this.id = id;
            duration = t;
            durationLeft = t;
            durationElapsed = 0;
            callback = c;

            step = s;
            stepLeft = s;
            stepCallback = sc;
            stepExecuteOnEnd = pe;
        }

        public void TimerUpdate(float deltaTime)
        {
            if (paused || ended) return;

            if (stepCallback != null)
            {
                stepLeft -= deltaTime;

                if (stepLeft <= 0)
                {
                    stepCallback();
                    stepLeft = step;
                }
            }

            durationLeft -= deltaTime;
            durationElapsed = duration - durationLeft;

            if (durationLeft <= 0 && duration >= 0)
            {
                ended = true;
                if (callback != null) callback();
                if (stepCallback != null && stepExecuteOnEnd) stepCallback();
            }
        }

        public void Reset()
        {
            durationLeft = duration;
        }

        public void Pause(bool paused)
        {
            this.paused = paused;
        }

        public void Cancel()
        {
            ended = true;
        }

        public static implicit operator bool(Timer timer)
        {
            return timer != null && !timer.ended;
        }
    }

    public class TimerSystem : MonoBehaviour, IUpdateable, IPauseable
    {
        [SerializeField] CustomList<Timer> timers = new CustomList<Timer>();
        [SerializeField] int activeTimerCount = 0;

        private void Start()
        {
            this.Register();
        }

        public void ManagedUpdate()
        {
            activeTimerCount = timers.Count;

            foreach (Timer t in timers)
            {
                if (t.paused) continue;
                t.TimerUpdate(Time.deltaTime);
                if (t.ended) timers.Remove(t);
            }
        }

        public void ManagedPause(bool paused)
        {
            foreach (Timer t in timers)
            {
                t.Pause(paused);
            }
        }

        public Timer SetTimer(float time, Action callback, string identifier = "")
        {
            return SetTimer(time, callback, 0f, null, identifier, false);
        }

        public Timer SetForeverTimer(float partialTime, Action partialCallback, string identifier = "")
        {
            return SetTimer(-1f, null, partialTime, partialCallback, identifier, false);
        }

        public Timer SetTimer(float time, Action callback, float partialTime, Action partialCallback, string id = "", bool partialExecuteOnEnd = false)
        {
            Timer timer = new Timer(time, callback, partialTime, partialCallback, partialExecuteOnEnd, id);
            timers.Add(timer);
            return timer;
        }

        public Timer SetTimerIndependent(float time, Action callback, string identifier = "")
        {
            return SetTimer(time, callback, 0f, null, identifier, false);
        }

        public Timer SetForeverTimerIndependent(float partialTime, Action partialCallback, string identifier = "")
        {
            return SetTimer(-1f, null, partialTime, partialCallback, identifier, false);
        }

        public Timer SetTimerIndependent(float time, Action callback, float partialTime, Action partialCallback, string id = "", bool partialExecuteOnEnd = false)
        {
            Timer timer = new Timer(time, callback, partialTime, partialCallback, partialExecuteOnEnd, id);
            return timer;
        }
    }
}

