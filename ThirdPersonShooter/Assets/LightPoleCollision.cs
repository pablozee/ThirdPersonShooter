using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPoleCollision : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 4f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Car")
        {
            rb.isKinematic = false;
            Destroy(gameObject, destroyDelay);
        }
    }
}
