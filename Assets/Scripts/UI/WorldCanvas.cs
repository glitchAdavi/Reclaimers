using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvas : MonoBehaviour
{
    private void Start()
    {
        if (!GameManager.current.showTutorial) gameObject.SetActive(false);
    }
}
