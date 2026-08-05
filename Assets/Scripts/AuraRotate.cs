using UnityEngine;
using Reclaimers.UpdateSystem;

public class AuraRotate : MonoBehaviour, IUpdateable, IPauseable
{
    [SerializeField] bool isPaused = false;
    [SerializeField] float rotateAngle = 1f;

    private void OnEnable()
    {
        this.Register();
    }

    private void OnDisable()
    {
        this.Unregister();
    }

    public void ManagedUpdate()
    {
        if (isPaused) return;

        transform.Rotate(Vector3.forward, rotateAngle);
    }

    public void ManagedPause(bool paused)
    {
        isPaused = paused;
    }
}
