using UnityEngine;

public class ExternalCollider : MonoBehaviour
{
    public delegate void DoOnCollisionEnter(Collision collision);
    public DoOnCollisionEnter onCollisionEnter;

    public delegate void DoOnTriggerEnter(Collider other);
    public DoOnTriggerEnter onTriggerEnter;

    public delegate void DoOnTriggerStay(Collider other);
    public DoOnTriggerStay onTriggerStay;

    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter?.Invoke(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }
}
