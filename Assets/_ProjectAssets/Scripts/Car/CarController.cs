using UnityEngine;

public class CarController : MonoBehaviour
{
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    public float motorTorque = 1500f;
    public float maxSteerAngle = 30f;

    [Header("Drift Settings")]
    public float driftSlip = 0.2f;
    public float normalSlip = 0.05f;
    public float normalStiffness = 1.0f;
    public float driftStiffness = 0.5f;
    public float driftSpeedThreshold = 20f;
    public float minSteerToDrift = 0.3f;

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

        friction.extremumSlip = slip;
        friction.asymptoteSlip = slip * 1.5f;
        friction.stiffness = stiffness;

        rearLeft.sidewaysFriction = friction;
        rearRight.sidewaysFriction = friction;
    }
}
