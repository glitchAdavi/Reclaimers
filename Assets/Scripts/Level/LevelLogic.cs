using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelLogic : MonoBehaviour, IUpdate, IPause
{
    public bool isActive = false;
    public bool isPaused = false;

    public virtual void Activate()
    {
        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
        isActive = true;
    }

    public void ExecuteUpdate()
    {
        if (isPaused) return;

        LogicUpdate();
    }

    public void Pause(bool paused)
    {
        isPaused = paused;
    }

    protected virtual void LogicUpdate()
    {

    }

    protected virtual void Win()
    {
        GameManager.current.gameInfo.currentPlayerStatBlock.CopyValues(GameManager.current.playerPawn.statBlock);
        GameManager.current.gameInfo.useCurrentPlayerStatBlock = true;
        GameManager.current.uiService.FadeOut(GameManager.current.ReturnToMenu);
    }

    protected virtual void Lose()
    {
        GameManager.current.gameInfo.currentPlayerStatBlock.CopyValues(GameManager.current.gameInfo.defaultStatBlock);
        GameManager.current.gameInfo.useCurrentPlayerStatBlock = false;
        GameManager.current.uiService.FadeOut(GameManager.current.ReturnToMenu);
    }
}
