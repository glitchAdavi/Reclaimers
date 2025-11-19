using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Btn_ChangeScene : MonoBehaviour
{
    public int sceneIndex = 0;

    public void ChangeScene()
    {
        GameManager.current.GoToLevel(sceneIndex);
    }
}
