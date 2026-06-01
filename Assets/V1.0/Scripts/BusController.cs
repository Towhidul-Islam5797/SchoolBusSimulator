#region Summary
//<summary>
// This script implements a basic bus controller for a driving game. It allows the player to control the bus using keyboard inputs, including throttle, steering, and gear shifting.
// The bus has three gears: Reverse (R), Neutral (N), and Drive (D). The player can accelerate forward in Drive, reverse in Reverse, and apply brakes in Neutral.
// The script uses Unity's WheelCollider components to simulate realistic wheel physics, including motor torque and braking forces. The bus's current speed is calculated and displayed in miles per hour (MPH).
// The bus's center of mass is adjusted
// to improve stability during driving. The script also updates the visual representation of the wheels based on the physics simulation.
//</summary>
#endregion
#region Milestone Sprint 1 - Basic Bus Controller
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class BusController : MonoBehaviour
//{
//    public enum Gear { R, N, D }

//    [Header("Wheel Colliders")]
//    public WheelCollider frontLeftWheel;
//    public WheelCollider frontRightWheel;
//    public WheelCollider rearLeftWheel;
//    public WheelCollider rearRightWheel;

//    [Header("Wheel Pivots")]
//    public Transform frontLeftPivot;
//    public Transform frontRightPivot;
//    public Transform rearLeftPivot;
//    public Transform rearRightPivot;

//    [Header("Drive Settings")]
//    public float motorForce = 3000f;
//    public float brakeForce = 5000f;
//    public float maxSteerAngle = 25f;

//    [Header("Current State")]
//    public Gear currentGear = Gear.N;
//    public float currentSpeed;

//    private Rigidbody rb;

//    void Start()
//    {
//        rb = GetComponentInParent<Rigidbody>();
//        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
//    }

//    void Update()
//    {
//        HandleGearInput();
//    }

//    void FixedUpdate()
//    {
//        var keyboard = Keyboard.current;
//        if (keyboard == null) return;

//        float throttle = 0f;
//        if (keyboard.wKey.isPressed) throttle = 1f;
//        if (keyboard.sKey.isPressed) throttle = -1f;

//        float steer = 0f;
//        if (keyboard.aKey.isPressed) steer = -1f;
//        if (keyboard.dKey.isPressed) steer = 1f;

//        currentSpeed = rb.linearVelocity.magnitude * 2.237f;

//        ApplyMotor(throttle);
//        ApplySteering(steer);
//        UpdateWheelMeshes();
//    }

//    void HandleGearInput()
//    {
//        var keyboard = Keyboard.current;
//        if (keyboard == null) return;

//        if (keyboard.qKey.wasPressedThisFrame)
//        {
//            if (currentGear == Gear.R) currentGear = Gear.N;
//            else if (currentGear == Gear.N) currentGear = Gear.D;
//        }

//        if (keyboard.eKey.wasPressedThisFrame)
//        {
//            if (currentGear == Gear.D) currentGear = Gear.N;
//            else if (currentGear == Gear.N) currentGear = Gear.R;
//        }
//    }

//    void ApplyMotor(float throttle)
//    {
//        float motor = 0f;
//        float brake = 0f;

//        if (currentGear == Gear.D)
//        {
//            if (throttle > 0f) motor = -(throttle * motorForce);
//            else if (throttle < 0f) brake = brakeForce;
//        }
//        else if (currentGear == Gear.R)
//        {
//            if (throttle < 0f) motor = -(throttle * motorForce);
//            else if (throttle > 0f) brake = brakeForce;
//        }
//        else
//        {
//            brake = brakeForce * 0.1f;
//        }

//        rearLeftWheel.motorTorque = motor;
//        rearRightWheel.motorTorque = motor;

//        frontLeftWheel.brakeTorque = brake;
//        frontRightWheel.brakeTorque = brake;
//        rearLeftWheel.brakeTorque = brake;
//        rearRightWheel.brakeTorque = brake;
//    }

//    void ApplySteering(float steer)
//    {
//        frontLeftWheel.steerAngle = steer * maxSteerAngle;
//        frontRightWheel.steerAngle = steer * maxSteerAngle;
//    }

//    void UpdateWheelMeshes()
//    {
//        UpdateSingleWheel(frontLeftWheel, frontLeftPivot);
//        UpdateSingleWheel(frontRightWheel, frontRightPivot);
//        UpdateSingleWheel(rearLeftWheel, rearLeftPivot);
//        UpdateSingleWheel(rearRightWheel, rearRightPivot);
//    }

//    void UpdateSingleWheel(WheelCollider col, Transform pivot)
//    {
//        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
//        pivot.position = pos;
//        pivot.rotation = rot * Quaternion.Euler(0f, -90f, 0f);
//    }
//}
#endregion
#region Milestone 1 Sprint 2 - Rolling Resistance and Braking Improvements
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class BusController : MonoBehaviour
//{
//    public enum Gear { R, N, D }

//    [Header("Wheel Colliders")]
//    public WheelCollider frontLeftWheel;
//    public WheelCollider frontRightWheel;
//    public WheelCollider rearLeftWheel;
//    public WheelCollider rearRightWheel;

//    [Header("Wheel Pivots")]
//    public Transform frontLeftPivot;
//    public Transform frontRightPivot;
//    public Transform rearLeftPivot;
//    public Transform rearRightPivot;

//    [Header("Drive Settings")]
//    public float motorForce = 3000f;
//    public float brakeForce = 5000f;
//    public float maxSteerAngle = 25f;

//    [Header("Rolling Resistance")]
//    public float rollingResistance = 300f;

//    [Header("Current State")]
//    public Gear currentGear = Gear.N;
//    public float currentSpeed;

//    private Rigidbody rb;

//    void Start()
//    {
//        rb = GetComponentInParent<Rigidbody>();
//    }

//    void Update()
//    {
//        HandleGearInput();
//    }

//    void FixedUpdate()
//    {
//        var keyboard = Keyboard.current;
//        if (keyboard == null) return;

//        float throttle = 0f;
//        if (keyboard.wKey.isPressed) throttle = 1f;
//        if (keyboard.sKey.isPressed) throttle = -1f;

//        float steer = 0f;
//        if (keyboard.aKey.isPressed) steer = -1f;
//        if (keyboard.dKey.isPressed) steer = 1f;

//        currentSpeed = rb.linearVelocity.magnitude * 2.237f;

//        ApplyMotor(throttle);
//        ApplySteering(steer);
//        UpdateWheelMeshes();
//    }

//    void HandleGearInput()
//    {
//        var keyboard = Keyboard.current;
//        if (keyboard == null) return;

//        if (keyboard.qKey.wasPressedThisFrame)
//        {
//            if (currentGear == Gear.R) currentGear = Gear.N;
//            else if (currentGear == Gear.N) currentGear = Gear.D;
//        }

//        if (keyboard.eKey.wasPressedThisFrame)
//        {
//            if (currentGear == Gear.D) currentGear = Gear.N;
//            else if (currentGear == Gear.N) currentGear = Gear.R;
//        }
//    }

//    void ApplyMotor(float throttle)
//    {
//        float motor = 0f;
//        float brake = 0f;

//        if (currentGear == Gear.D)
//        {
//            if (throttle > 0f)
//                motor = -throttle * motorForce;
//            else if (throttle < 0f)
//                brake = brakeForce;
//            else
//                brake = rollingResistance;
//        }
//        else if (currentGear == Gear.R)
//        {
//            if (throttle > 0f)
//                motor = throttle * motorForce;
//            else if (throttle < 0f)
//                brake = brakeForce;
//            else
//                brake = rollingResistance;
//        }
//        else
//        {
//            brake = brakeForce;
//        }

//        // Zero motorTorque before applying brake.
//        // If both are non-zero at the same time the wheels fight each other and spin visually.
//        rearLeftWheel.motorTorque = 0f;
//        rearRightWheel.motorTorque = 0f;

//        rearLeftWheel.motorTorque = motor;
//        rearRightWheel.motorTorque = motor;

//        frontLeftWheel.brakeTorque = brake;
//        frontRightWheel.brakeTorque = brake;
//        rearLeftWheel.brakeTorque = brake;
//        rearRightWheel.brakeTorque = brake;
//    }

//    void ApplySteering(float steer)
//    {
//        frontLeftWheel.steerAngle = steer * maxSteerAngle;
//        frontRightWheel.steerAngle = steer * maxSteerAngle;
//    }

//    void UpdateWheelMeshes()
//    {
//        UpdateSingleWheel(frontLeftWheel, frontLeftPivot);
//        UpdateSingleWheel(frontRightWheel, frontRightPivot);
//        UpdateSingleWheel(rearLeftWheel, rearLeftPivot);
//        UpdateSingleWheel(rearRightWheel, rearRightPivot);
//    }

//    void UpdateSingleWheel(WheelCollider col, Transform pivot)
//    {
//        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
//        pivot.position = pos;
//        pivot.rotation = rot * Quaternion.Euler(0f, -90f, 0f);
//    }
//}
#endregion

#region Milestone 1 Sprint 3 - Final Bus Controller with Audio and Visual Enhancements
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class BusController : MonoBehaviour
//{
//    public enum Gear { R, N, D }

//    [Header("Wheel Colliders")]
//    public WheelCollider frontLeftWheel;
//    public WheelCollider frontRightWheel;
//    public WheelCollider rearLeftWheel;
//    public WheelCollider rearRightWheel;

//    [Header("Wheel Pivots")]
//    public Transform frontLeftPivot;
//    public Transform frontRightPivot;
//    public Transform rearLeftPivot;
//    public Transform rearRightPivot;

//    [Header("Drive Settings")]
//    public float motorForce = 3000f;
//    public float brakeForce = 5000f;
//    public float rollingResistance = 300f;

//    [Header("Acceleration")]
//    [Tooltip("How fast the throttle ramps up and down. Lower = slower buildup. Higher = instant.")]
//    public float accelerationSensitivity = 1.5f;

//    [Header("Steering")]
//    [Tooltip("How fast the wheels turn and return to center.")]
//    public float steeringSensitivity = 3f;
//    [Tooltip("Max steer angle at low speed.")]
//    public float maxSteerAngle = 25f;
//    [Tooltip("Max steer angle at high speed.")]
//    public float minSteerAngleAtSpeed = 8f;
//    [Tooltip("Speed in MPH at which steering is fully reduced.")]
//    public float steeringReductionSpeed = 40f;

//    [Header("Current State")]
//    public Gear currentGear = Gear.N;
//    public float currentSpeed;

//    private Rigidbody rb;
//    private float currentThrottle;
//    private float currentSteer;

//    void Start()
//    {
//        rb = GetComponentInParent<Rigidbody>();
//    }

//    void Update()
//    {
//        HandleGearInput();
//    }

//    void FixedUpdate()
//    {
//        var keyboard = Keyboard.current;
//        if (keyboard == null) return;

//        float targetThrottle = 0f;
//        if (keyboard.wKey.isPressed) targetThrottle = 1f;
//        if (keyboard.sKey.isPressed) targetThrottle = -1f;

//        float targetSteer = 0f;
//        if (keyboard.aKey.isPressed) targetSteer = -1f;
//        if (keyboard.dKey.isPressed) targetSteer = 1f;

//        // Smoothly ramp throttle and steer toward the target value.
//        // MoveTowards reaches the target cleanly unlike Lerp which never fully arrives.
//        currentThrottle = Mathf.MoveTowards(currentThrottle, targetThrottle, accelerationSensitivity * Time.fixedDeltaTime);
//        currentSteer = Mathf.MoveTowards(currentSteer, targetSteer, steeringSensitivity * Time.fixedDeltaTime);

//        currentSpeed = rb.linearVelocity.magnitude * 2.237f;

//        ApplyMotor(currentThrottle);
//        ApplySteering(currentSteer);
//        UpdateWheelMeshes();
//    }

//    void HandleGearInput()
//    {
//        var keyboard = Keyboard.current;
//        if (keyboard == null) return;

//        if (keyboard.qKey.wasPressedThisFrame)
//        {
//            if (currentGear == Gear.R) currentGear = Gear.N;
//            else if (currentGear == Gear.N) currentGear = Gear.D;
//        }

//        if (keyboard.eKey.wasPressedThisFrame)
//        {
//            if (currentGear == Gear.D) currentGear = Gear.N;
//            else if (currentGear == Gear.N) currentGear = Gear.R;
//        }
//    }

//    void ApplyMotor(float throttle)
//    {
//        float motor = 0f;
//        float brake = 0f;

//        if (currentGear == Gear.D)
//        {
//            if (throttle > 0f)
//                motor = -throttle * motorForce;
//            else if (throttle < 0f)
//                brake = brakeForce;
//            else
//                brake = rollingResistance;
//        }
//        else if (currentGear == Gear.R)
//        {
//            if (throttle > 0f)
//                motor = throttle * motorForce;
//            else if (throttle < 0f)
//                brake = brakeForce;
//            else
//                brake = rollingResistance;
//        }
//        else
//        {
//            brake = brakeForce;
//        }

//        rearLeftWheel.motorTorque = 0f;
//        rearRightWheel.motorTorque = 0f;

//        rearLeftWheel.motorTorque = motor;
//        rearRightWheel.motorTorque = motor;

//        frontLeftWheel.brakeTorque = brake;
//        frontRightWheel.brakeTorque = brake;
//        rearLeftWheel.brakeTorque = brake;
//        rearRightWheel.brakeTorque = brake;
//    }

//    void ApplySteering(float steer)
//    {
//        // Reduce max steer angle as speed increases so the bus doesn't spin out at high speed.
//        float speedAlpha = Mathf.InverseLerp(0f, steeringReductionSpeed, currentSpeed);
//        float effectiveMaxSteer = Mathf.Lerp(maxSteerAngle, minSteerAngleAtSpeed, speedAlpha);

//        frontLeftWheel.steerAngle = steer * effectiveMaxSteer;
//        frontRightWheel.steerAngle = steer * effectiveMaxSteer;
//    }

//    void UpdateWheelMeshes()
//    {
//        UpdateSingleWheel(frontLeftWheel, frontLeftPivot);
//        UpdateSingleWheel(frontRightWheel, frontRightPivot);
//        UpdateSingleWheel(rearLeftWheel, rearLeftPivot);
//        UpdateSingleWheel(rearRightWheel, rearRightPivot);
//    }

//    void UpdateSingleWheel(WheelCollider col, Transform pivot)
//    {
//        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
//        pivot.position = pos;
//        pivot.rotation = rot * Quaternion.Euler(0f, -90f, 0f);
//    }
//}
#endregion
#region Milestone 1 Sprint 4 - Finalized Bus Controller with Speed-Based Steering Reduction and Acceleration Sensitivity
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
    public float rollingResistance = 300f;

    [Header("Speed")]
    [Tooltip("Top speed in MPH. Motor force cuts off at this speed.")]
    public float maxSpeed = 60f;

    [Header("Acceleration")]
    [Tooltip("How fast the throttle ramps up. Lower = heavier buildup.")]
    public float accelerationSensitivity = 1.5f;

    [Header("Steering")]
    [Tooltip("How fast the wheels turn toward full lock when pressing A or D.")]
    public float steeringSpeed = 1.5f;
    [Tooltip("How fast the wheels return to center when no key is pressed.")]
    public float steeringReturnSpeed = 0.8f;
    [Tooltip("Max steer angle at low speed.")]
    public float maxSteerAngle = 25f;
    [Tooltip("Max steer angle at high speed.")]
    public float minSteerAngleAtSpeed = 8f;
    [Tooltip("Speed in MPH at which steering is fully reduced.")]
    public float steeringReductionSpeed = 40f;

    [Header("Current State")]
    public Gear currentGear = Gear.N;
    public float currentSpeed;

    private Rigidbody rb;
    private float currentThrottle;
    private float currentSteer;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
    }

    void Update()
    {
        HandleGearInput();
    }

    void FixedUpdate()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        float targetThrottle = 0f;
        if (keyboard.wKey.isPressed) targetThrottle = 1f;
        if (keyboard.sKey.isPressed) targetThrottle = -1f;

        float targetSteer = 0f;
        if (keyboard.aKey.isPressed) targetSteer = -1f;
        if (keyboard.dKey.isPressed) targetSteer = 1f;

        currentThrottle = Mathf.MoveTowards(currentThrottle, targetThrottle, accelerationSensitivity * Time.fixedDeltaTime);

        // Use a slower return speed when no key is pressed so wheels self-center lazily.
        // Use steeringSpeed when actively steering.
        float steerStep = targetSteer != 0f ? steeringSpeed : steeringReturnSpeed;
        currentSteer = Mathf.MoveTowards(currentSteer, targetSteer, steerStep * Time.fixedDeltaTime);

        currentSpeed = rb.linearVelocity.magnitude * 2.237f;

        ApplyMotor(currentThrottle);
        ApplySteering(currentSteer);
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

        // Below 1 MPH apply zero rolling resistance.
        // WheelCollider snaps the bus to zero even with tiny brake values at very low speed.
        // Let the physics engine handle the final stop naturally.
        float scaledRollingResistance = currentSpeed < 2f ? 0f : rollingResistance;

        if (currentGear == Gear.D)
        {
            if (throttle > 0f)
            {
                float speedRatio = Mathf.Clamp01(currentSpeed / maxSpeed);
                float torqueFalloff = 1f - speedRatio;
                motor = -throttle * motorForce * torqueFalloff;
            }
            else if (throttle < 0f)
                brake = brakeForce;
            else
                brake = scaledRollingResistance;
        }
        else if (currentGear == Gear.R)
        {
            if (throttle > 0f)
            {
                float speedRatio = Mathf.Clamp01(currentSpeed / maxSpeed);
                float torqueFalloff = 1f - speedRatio;
                motor = throttle * motorForce * torqueFalloff;
            }
            else if (throttle < 0f)
                brake = brakeForce;
            else
                brake = scaledRollingResistance;
        }
        else
        {
            brake = brakeForce;
        }

        rearLeftWheel.motorTorque = 0f;
        rearRightWheel.motorTorque = 0f;

        rearLeftWheel.motorTorque = motor;
        rearRightWheel.motorTorque = motor;

        frontLeftWheel.brakeTorque = brake;
        frontRightWheel.brakeTorque = brake;
        rearLeftWheel.brakeTorque = brake;
        rearRightWheel.brakeTorque = brake;
    }

    void ApplySteering(float steer)
    {
        float speedAlpha = Mathf.InverseLerp(0f, steeringReductionSpeed, currentSpeed);
        float effectiveMaxSteer = Mathf.Lerp(maxSteerAngle, minSteerAngleAtSpeed, speedAlpha);

        frontLeftWheel.steerAngle = steer * effectiveMaxSteer;
        frontRightWheel.steerAngle = steer * effectiveMaxSteer;
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
#endregion
