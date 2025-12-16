using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour, IUpdate, IPause
{
    [SerializeField] SpriteRenderer _sr;
    bool isPaused = false;

    private void OnEnable()
    {
        GameManager.current.updateService.RegisterUpdate(this);
        GameManager.current.updateService.RegisterPause(this);
    }

    public void ExecuteUpdate()
    {
        if (isPaused) return;


        Ray ray = GameManager.current.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit info, 100f, 1 << 20))
        {
            transform.position = info.point;
        }
    }

    public void Pause(bool paused)
    {
        isPaused = paused;
        _sr.enabled = !paused;
        Cursor.visible = paused;
    }
}
