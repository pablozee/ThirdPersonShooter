using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpine : MonoBehaviour
{
    [SerializeField] private GameObject spine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion newRotation = spine.transform.rotation;
        Quaternion newQuaternion = Quaternion.Euler(newRotation.eulerAngles.x - 50f, newRotation.eulerAngles.y, newRotation.eulerAngles.z);
        spine.transform.rotation = newQuaternion;
    }
}
