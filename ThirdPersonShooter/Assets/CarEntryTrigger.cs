using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CarEntryTrigger : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private CinemachineVirtualCamera vCam;
    [SerializeField] private Transform carCamFocus;

    private void Start()
    {
        input.enabled = false;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            StarterAssets.ThirdPersonController controller = other.GetComponent<StarterAssets.ThirdPersonController>();
            if (controller.isEnteringCar)
            {
                other.gameObject.SetActive(false);
                input.enabled = true;
                controller.isEnteringCar = false;
                vCam.Follow = carCamFocus;
                vCam.LookAt = carCamFocus;
            }
        }
    }
}
