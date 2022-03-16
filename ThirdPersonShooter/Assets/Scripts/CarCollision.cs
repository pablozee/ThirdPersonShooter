using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarCollision : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent agent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Car")
        {
            agent.enabled = false;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
