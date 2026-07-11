using Reclaimers.DeveloperConsole;
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
        public bool affectedByPause = true;

        public Timer(float t, Action c, string id, bool pa)
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

            affectedByPause = pa;
        }

        public Timer(float t, Action c, float s, Action sc, bool pe, string id, bool pa)
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

            affectedByPause = pa;
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
                if (callback != null) callback();
                if (stepCallback != null && stepExecuteOnEnd) stepCallback();
                ended = true;
            }
        }

        public void Reset()
        {
            durationLeft = duration;
        }

        public void Pause(bool paused)
        {
            if (!affectedByPause) return;
            this.paused = paused;
        }

        public void Cancel()
        {
            ended = true;
        }

        public void Finish()
        {
            if (stepExecuteOnEnd) stepCallback();
            callback();
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

        private bool _print = false;

        private void Start()
        {
            this.Register();
        }

        public void ManagedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.P)) _print = true;

            foreach (Timer t in timers)
            {
                if (_print) LogSystem.Log(t.id);

                if (!t.paused) t.TimerUpdate(Time.deltaTime);
                if (t.ended) timers.Remove(t);
            }

            _print = false;
            activeTimerCount = timers.Count;
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
            return SetTimer(time, callback, 0f, null, identifier, false, true);
        }

        public Timer SetForeverTimer(float partialTime, Action partialCallback, string identifier = "")
        {
            return SetTimer(-1f, null, partialTime, partialCallback, identifier, false, true);
        }

        public Timer SetTimer(float time, Action callback, float partialTime, Action partialCallback, string identifier = "", bool partialExecuteOnEnd = false, bool affectedByPause = true)
        {
            Timer timer = new Timer(time, callback, partialTime, partialCallback, partialExecuteOnEnd, identifier, affectedByPause);
            timers.Add(timer);
            return timer;
        }

        public Timer SetTimerIndependent(float time, Action callback, string identifier = "")
        {
            return SetTimer(time, callback, 0f, null, identifier, false, true);
        }

        public Timer SetForeverTimerIndependent(float partialTime, Action partialCallback, string identifier = "")
        {
            return SetTimer(-1f, null, partialTime, partialCallback, identifier, false, true);
        }

        public Timer SetTimerIndependent(float time, Action callback, float partialTime, Action partialCallback, string identifier = "", bool partialExecuteOnEnd = false, bool affectedByPause = true)
        {
            Timer timer = new Timer(time, callback, partialTime, partialCallback, partialExecuteOnEnd, identifier, affectedByPause);
            return timer;
        }
    }
}

