using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_QuitGame : InteractableArea
{
    protected override void OnFinishEffect()
    {
        GameManager.current.QuitGame();
    }
}
