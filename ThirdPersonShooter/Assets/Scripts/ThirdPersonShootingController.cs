using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ThirdPersonShootingController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;

    private PlayerInputActions inputActions;
    private bool aim;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
        } else
        {
            aimVirtualCamera.gameObject.SetActive(false);
        }
    }

    void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }

    void AimInput(bool newAimState)
    {
        aim = newAimState;
    }
}
