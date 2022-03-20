using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterColliderTrigger : MonoBehaviour
{
    private Collider parentCollider;
    private Rigidbody parentRB;

    void Start()
    {
        parentCollider = transform.GetComponentInParent<Collider>();
        parentRB = GetComponentInParent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != transform.parent)
        {
            if (other.tag == "Car")
            {
                parentCollider.isTrigger = true;
                parentRB.isKinematic = false;
            }
        }
    }
}
