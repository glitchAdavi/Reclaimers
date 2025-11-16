using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCameraOnStart : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
