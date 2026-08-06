using UnityEngine;
using System.Collections.Generic;

public class Tutorials : MonoBehaviour
{
    [SerializeField] List<GameObject> allTutorials = new List<GameObject>();

    public void ToggleTutorials(bool enable)
    {
        foreach (GameObject tut in allTutorials)
        {
            tut.SetActive(enable);
        }
    }
}
