using UnityEngine;
using UnityEngine.InputSystem;

public class BusController : MonoBehaviour
{
    public enum Gear { R, N, D }

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Pivots")]
    public Transform frontLeftPivot;
    public Transform frontRightPivot;
    public Transform rearLeftPivot;
    public Transform rearRightPivot;

    [Header("Drive Settings")]
    public float motorForce = 3000f;
    public float brakeForce = 5000f;
    public float maxSteerAngle = 25f;

    [Header("Current State")]
    public Gear currentGear = Gear.N;
    public float currentSpeed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    void Update()
    {
        HandleGearInput();
    }

    void FixedUpdate()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float throttle = 0f;
        if (keyboard.wKey.isPressed) throttle = 1f;
        if (keyboard.sKey.isPressed) throttle = -1f;

        float steer = 0f;
        if (keyboard.aKey.isPressed) steer = -1f;
        if (keyboard.dKey.isPressed) steer = 1f;

        currentSpeed = rb.linearVelocity.magnitude * 2.237f;

        ApplyMotor(throttle);
        ApplySteering(steer);
        UpdateWheelMeshes();
    }

    void HandleGearInput()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.qKey.wasPressedThisFrame)
        {
            if (currentGear == Gear.R) currentGear = Gear.N;
            else if (currentGear == Gear.N) currentGear = Gear.D;
        }

        if (keyboard.eKey.wasPressedThisFrame)
        {
            if (currentGear == Gear.D) currentGear = Gear.N;
            else if (currentGear == Gear.N) currentGear = Gear.R;
        }
    }

    void ApplyMotor(float throttle)
    {
        float motor = 0f;
        float brake = 0f;

        if (currentGear == Gear.D)
        {
            if (throttle > 0f) motor = -(throttle * motorForce);
            else if (throttle < 0f) brake = brakeForce;
        }
        else if (currentGear == Gear.R)
        {
            if (throttle < 0f) motor = -(throttle * motorForce);
            else if (throttle > 0f) brake = brakeForce;
        }
        else
        {
            brake = brakeForce * 0.1f;
        }

        rearLeftWheel.motorTorque = motor;
        rearRightWheel.motorTorque = motor;

        frontLeftWheel.brakeTorque = brake;
        frontRightWheel.brakeTorque = brake;
        rearLeftWheel.brakeTorque = brake;
        rearRightWheel.brakeTorque = brake;
    }

    void ApplySteering(float steer)
    {
        frontLeftWheel.steerAngle = steer * maxSteerAngle;
        frontRightWheel.steerAngle = steer * maxSteerAngle;
    }

    void UpdateWheelMeshes()
    {
        UpdateSingleWheel(frontLeftWheel, frontLeftPivot);
        UpdateSingleWheel(frontRightWheel, frontRightPivot);
        UpdateSingleWheel(rearLeftWheel, rearLeftPivot);
        UpdateSingleWheel(rearRightWheel, rearRightPivot);
    }

    void UpdateSingleWheel(WheelCollider col, Transform pivot)
    {
        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
        pivot.position = pos;
        pivot.rotation = rot * Quaternion.Euler(0f, -90f, 0f);
    }
}