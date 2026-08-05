using UnityEngine;
using Reclaimers.UpdateSystem;

public class Crosshair : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] SpriteRenderer _sr;
    bool isPaused = false;

    Plane aimPlane = new Plane(Vector3.up, new Vector3(0f, 0.25f, 0f));

    private void OnEnable()
    {
        this.Register();
    }

    public void ManagedUpdate()
    {
        if (isPaused) return;

        Ray ray = GameManager.Instance.playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        if (aimPlane.Raycast(ray, out float enterDistance))
        {
            Vector3 pos = ray.GetPoint(enterDistance);
            transform.position = pos;
        }
    }

    private void OnDisable()
    {
        this.Unregister();
    }

    public void ManagedPause(bool paused)
    {
        isPaused = paused;
        _sr.enabled = !paused;
    }
}
