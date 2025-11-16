using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateService : MonoBehaviour
{
    public ExecutableList<IUpdate> elUpdates = new ExecutableList<IUpdate>(ExecuteUpdate);
    public ExecutableList<IFixedUpdate> elFixedUpdates = new ExecutableList<IFixedUpdate>(ExecuteFixedUpdate);
    public ExecutableList<ILateUpdate> elLateUpdates = new ExecutableList<ILateUpdate>(ExecuteLateUpdate);
    public ExecutableList<IPause> elPauses = new ExecutableList<IPause>(SetPause);

    public static bool isGamePaused = false;
    public bool isGamePausedInspector = false;
    [SerializeField] private bool triggerPauseChange = false;

    [SerializeField] int elUpdatesCount = 0;
    [SerializeField] int elFixedUpdatesCount = 0;
    [SerializeField] int elLateUpdatesCount = 0;
    [SerializeField] int elPausesCount = 0;

    // Update is called once per frame
    void Update()
    {
        isGamePausedInspector = isGamePaused;

        if (triggerPauseChange == true)
        {
            elPauses.ExecuteAll();
            triggerPauseChange = false;
        }

        elUpdates.ExecuteAll();
    }

    private void FixedUpdate()
    {
        elFixedUpdates.ExecuteAll();
    }

    private void LateUpdate()
    {
        elLateUpdates.ExecuteAll();
    }

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
        triggerPauseChange = true;
    }


    #region UpdateIO
    public void RegisterUpdate(IUpdate u, Action callback = null)
    {
        elUpdates.Add(u);
        elUpdatesCount++;
        if (callback != null) callback();
    }

    public void RegisterFixedUpdate(IFixedUpdate fu, Action callback = null)
    {
        elFixedUpdates.Add(fu);
        elFixedUpdatesCount++;
        if (callback != null) callback();
    }

    public void RegisterLateUpdate(ILateUpdate lu, Action callback = null)
    {
        elLateUpdates.Add(lu);
        elLateUpdatesCount++;
        if (callback != null) callback();
    }

    public void RegisterPause(IPause p, Action callback = null)
    {
        elPauses.Add(p);
        elPausesCount++;
        if (callback != null) callback();
    }


    public void UnregisterUpdate(IUpdate u, Action callback = null)
    {
        elUpdates.Remove(u);
        elUpdatesCount--;
        if (callback != null) callback();
    }

    public void UnregisterFixedUpdate(IFixedUpdate fu, Action callback = null)
    {
        elFixedUpdates.Remove(fu);
        elFixedUpdatesCount--;
        if (callback != null) callback();
    }

    public void UnregisterLateUpdate(ILateUpdate lu, Action callback = null)
    {
        elLateUpdates.Remove(lu);
        elLateUpdatesCount--;
        if (callback != null) callback();
    }

    public void UnregisterPause(IPause p, Action callback = null)
    {
        elPauses.Remove(p);
        elPausesCount--;
        if (callback != null) callback();
    }
    #endregion


    #region Statics
    static public void ExecuteUpdate(IUpdate u)
    {
        u.ExecuteUpdate();
    }

    static public void ExecuteFixedUpdate(IFixedUpdate fu)
    {
        fu.ExecuteFixedUpdate();
    }

    static public void ExecuteLateUpdate(ILateUpdate lu)
    {
        lu.ExecuteLateUpdate();
    }

    static public void SetPause(IPause p)
    {
        p.Pause(isGamePaused);
    }
    #endregion
}
