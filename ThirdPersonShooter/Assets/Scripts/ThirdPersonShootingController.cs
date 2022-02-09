using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ThirdPersonShootingController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private GameObject debugTransform;
    [SerializeField] private float inputThreshold, topClamp, bottomClamp;
    [SerializeField] private bool lockCameraPosition;
    [SerializeField] private GameObject cinemachineCameraTarget;
    [SerializeField] private float cinemachineTargetPitch, cameraAngleOverride, cinemachineTargetYaw;

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
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.transform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (aim)
        {
            aimVirtualCamera.Priority = 12;

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 10f);
        } else
        {
            aimVirtualCamera.Priority = 8;
        }
    }

    private void LateUpdate()
    {
//        CameraRotation();
    }
    private void CameraRotation()
    {
        Vector2 mouseLook = inputActions.Player.MouseLook.ReadValue<Vector2>();
        // if there is an input and camera position is not fixed
        if (mouseLook.sqrMagnitude >= inputThreshold && !lockCameraPosition)
        {
            cinemachineTargetYaw += mouseLook.x * Time.deltaTime;
            cinemachineTargetPitch += mouseLook.y * Time.deltaTime;
        }

        // clamp our rotations so our values are limited 360 degrees
        cinemachineTargetYaw = ClampAngle(cinemachineTargetYaw, float.MinValue, float.MaxValue);
        cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, bottomClamp, topClamp);

        // Cinemachine will follow this target
        cinemachineCameraTarget.transform.rotation = Quaternion.Euler(cinemachineTargetPitch + cameraAngleOverride, cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
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
