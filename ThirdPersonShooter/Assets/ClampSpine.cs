using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampSpine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.y > 30f)
        {
            Quaternion newRotation = Quaternion.Euler(transform.rotation.x, 30f, transform.rotation.z);
            transform.rotation = newRotation;
        }
        if (transform.rotation.y < -30f)
        {
            Quaternion newRotation = Quaternion.Euler(transform.rotation.x, -30f, transform.rotation.z);
            transform.rotation = newRotation;
        }
    }
}
