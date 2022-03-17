    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform backLeftTransform;
    [SerializeField] private Transform backRightTransform;

    private float forwardForce;
    private float steerForce;
    private float currentSteerAngle;
    private bool isBraking;
    private float currentBrakeForce;


    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log(forwardForce);
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    void HandleMotor()
    {
        frontLeft.motorTorque = forwardForce * motorForce;
        frontRight.motorTorque = forwardForce * motorForce;
        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    void ApplyBraking()
    {
        frontLeft.brakeTorque = currentBrakeForce;
        frontRight.brakeTorque = currentBrakeForce;
        backLeft.brakeTorque = currentBrakeForce;
        backRight.brakeTorque = currentBrakeForce;
    }
   
    void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * steerForce;
        frontLeft.steerAngle = currentSteerAngle;
        frontRight.steerAngle = currentSteerAngle;
    }

    void UpdateWheels()
    {
        UpdateSingleWheel(frontLeft, frontLeftTransform);
        UpdateSingleWheel(frontRight, frontRightTransform);
        UpdateSingleWheel(backLeft, backLeftTransform);
        UpdateSingleWheel(backRight, backRightTransform);
    }

    void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    void OnAccelerate(InputValue value)
    {
        forwardForce = value.Get<float>();
    }

    void OnSteer(InputValue value)
    {
        steerForce = value.Get<float>();
    }

    void OnBrake(InputValue value)
    {
        isBraking = value.Get<bool>();
    }
}
