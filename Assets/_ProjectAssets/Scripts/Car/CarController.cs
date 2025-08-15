using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public static Action<bool> onDriftStateChanged;

    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider rearLeft;
    [SerializeField] private WheelCollider rearRight;

    [SerializeField] private float motorTorque = 1500f;
    [SerializeField] private float maxSteerAngle = 30f;

    [Header("Drift Settings")]
    [SerializeField] private float driftSlip = 0.2f;
    [SerializeField] private float normalSlip = 0.05f;
    [SerializeField] private float normalStiffness = 1.0f;
    [SerializeField] private float driftStiffness = 0.5f;
    [SerializeField] private float driftSpeedThreshold = 20f;
    [SerializeField] private float minSteerToDrift = 0.3f;


    private float steerInput;
    private float throttleInput;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        steerInput = Input.GetAxis("Horizontal");
        throttleInput = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        ApplySteering();
        ApplyDrive();

        bool shouldDrift = Mathf.Abs(steerInput) > minSteerToDrift && rb.linearVelocity.magnitude > driftSpeedThreshold;
        ApplyDriftPhysics(shouldDrift);
    }

    void ApplySteering()
    {
        float steerAngle = maxSteerAngle * steerInput;
        frontLeft.steerAngle = steerAngle;
        frontRight.steerAngle = steerAngle;
    }

    void ApplyDrive()
    {
        float torque = throttleInput * motorTorque;
        rearLeft.motorTorque = torque;
        rearRight.motorTorque = torque;
    }

    void ApplyDriftPhysics(bool drifting)
    {
        WheelFrictionCurve friction = rearLeft.sidewaysFriction;

        float slip = drifting ? driftSlip : normalSlip;
        float stiffness = drifting ? driftStiffness : normalStiffness;

        onDriftStateChanged?.Invoke(drifting);

        friction.extremumSlip = slip;
        friction.asymptoteSlip = slip * 1.5f;
        friction.stiffness = stiffness;

        rearLeft.sidewaysFriction = friction;
        rearRight.sidewaysFriction = friction;
    }
}
