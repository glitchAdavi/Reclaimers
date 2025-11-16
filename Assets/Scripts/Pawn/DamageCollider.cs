using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public delegate void DoOnCollisionEnter(Collision collision);
    public DoOnCollisionEnter onCollisionEnter;

    public delegate void DoOnCollisionStay(Collision collision);
    public DoOnCollisionStay onCollisionStay;

    public delegate void DoOnTriggerEnter(Collider other);
    public DoOnTriggerEnter onTriggerEnter;


    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnter?.Invoke(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        onCollisionStay?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
