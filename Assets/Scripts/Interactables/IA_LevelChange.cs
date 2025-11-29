using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_LevelChange : InteractableArea
{
    public int levelIndex = 0;

    public bool savePlayerData = false; // NADA QUE VER CON SISTEMA DE GUARDADO POR AHORA

    public void ChangeLevel()
    {
        GameManager.current.GoToLevel(levelIndex);
    }

    protected override void OnFinishEffect()
    {
        GameManager.current.SavePlayer();
        GameManager.current.uiService.FadeOut(ChangeLevel);
    }
}
