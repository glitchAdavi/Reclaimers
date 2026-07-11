using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Reclaimers.Systems;

public class UIMenu : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();

    public float animationTime = 0.5f;
    public float animationTimeStep = 0.05f;
    Timer animationTimer;

    public virtual void BeforeOpen()
    {
        Open();
    }

    protected virtual void Open()
    {
        animationTimer?.Finish();
        float animChange = animationTime / animationTimeStep;

        foreach (GameObject o in objects)
        {
            o.SetActive(true);
        }

        animationTimer = GameManager.Instance.timerSystem.SetTimer(animationTime, OnOpen, animationTimeStep, () =>
        {
            transform.localScale += new Vector3(1f / animChange, 1f / animChange, 1f / animChange);
        }, "", false, false);
    }

    protected virtual void OnOpen()
    {
        animationTimer = null;

        transform.localScale = new Vector3(1f, 1f, 1f);

        foreach (GameObject o in objects)
        {
            o.TryGetComponent(out Button b);
            if (b != null) b.interactable = true;
        }
    }

    public virtual void BeforeClose()
    {
        Close();
    }

    protected virtual void Close()
    {
        animationTimer?.Finish();
        float animChange = animationTime / animationTimeStep;

        foreach (GameObject o in objects)
        {
            o.TryGetComponent(out Button b);
            if (b != null) b.interactable = false;
        }

        animationTimer = GameManager.Instance.timerSystem.SetTimer(animationTime, OnClose, animationTimeStep, () =>
        {
            transform.localScale -= new Vector3(1f / animChange, 1f / animChange, 1f / animChange);
        }, "", false, false);
    }

    protected virtual void OnClose()
    {
        animationTimer = null;

        transform.localScale = Vector3.zero;

        foreach (GameObject o in objects)
        {
            o.SetActive(false);
        }
    }
}
