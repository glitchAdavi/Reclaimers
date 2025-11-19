using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_LevelChange : InteractableArea
{
    public int levelIndex = 0;

    protected override void OnFinishEffect()
    {
        GameManager.current.GoToLevel(levelIndex);
    }
}
