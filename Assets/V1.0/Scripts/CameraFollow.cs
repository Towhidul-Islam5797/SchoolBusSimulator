#region Summary
//<summary>
// This script implements a dynamic camera follow system for a bus driving game.
//      The camera adjusts its position, rotation, and field of view based on the bus's speed to create an immersive driving experience.
//      The camera follows the rear axle pivot of the bus, maintaining a distance that increases with speed and a height that decreases slightly for a more dramatic effect at higher speeds.
//      The field of view also widens as the bus accelerates to enhance the sensation of speed. Smooth damping is applied to both position and rotation changes to ensure fluid camera movement.
//</summary>
#endregion
#region milestone 1 Sprint 1 - Basic Camera Follow
//using UnityEngine;

//public class CameraFollow : MonoBehaviour
//{
//    public Transform target;
//    public float smoothSpeed = 5f;

//    private Vector3 offset;

//    void Start()
//    {
//        offset = transform.position - target.position;
//    }

//    void LateUpdate()
//    {
//        transform.position = Vector3.Lerp(
//            transform.position,
//            target.position + offset,
//            smoothSpeed * Time.deltaTime
//        );
//    }
//}
#endregion
#region Milestone 1 Sprint 2 - Dynamic Camera Follow
//using UnityEngine;

//public class CameraFollow : MonoBehaviour
//{
//    [Header("Anchors")]
//    public Transform busTransform;

//    [Header("Rotation Settings")]
//    public float cameraXRotation = 20f;

//    [Header("FOV Settings")]
//    public float lowSpeedFOV = 60f;
//    public float highSpeedFOV = 75f;

//    [Header("Damping Settings")]
//    public float lowSpeedRotationDamping = 4f;
//    public float highSpeedRotationDamping = 12f;
//    public float positionSmoothTime = 0.2f;

//    [Header("Speed Range")]
//    public float lowSpeedThreshold = 20f;
//    public float highSpeedThreshold = 80f;

//    private Camera cameraComponent;
//    private Vector3 positionVelocity = Vector3.zero;
//    private Vector3 localPositionOffset;
//    private float currentYRotation;
//    private float yRotationOffset;
//    private Rigidbody busRigidbody;

//    void Start()
//    {
//        cameraComponent = GetComponent<Camera>();
//        busRigidbody = busTransform.GetComponentInParent<Rigidbody>();
//        localPositionOffset = Quaternion.Inverse(busTransform.rotation) * (transform.position - busTransform.position);

//        // Store the difference between the camera's Y and the bus's Y at startup.
//        // This offset is maintained forever so the camera never rotates away from where you placed it.
//        yRotationOffset = transform.eulerAngles.y - busTransform.eulerAngles.y;
//        currentYRotation = transform.eulerAngles.y;
//    }

//    void LateUpdate()
//    {
//        if (busTransform == null) return;

//        float speedKPH = CalculateSpeedKPH();
//        float speedAlpha = CalculateSpeedAlpha(speedKPH);

//        UpdateCameraPosition();
//        UpdateCameraRotation(speedAlpha);
//        UpdateFieldOfView(speedAlpha);
//    }

//    float CalculateSpeedKPH()
//    {
//        if (busRigidbody == null) return 0f;
//        return busRigidbody.linearVelocity.magnitude * 3.6f;
//    }

//    float CalculateSpeedAlpha(float speedKPH)
//    {
//        return Mathf.InverseLerp(lowSpeedThreshold, highSpeedThreshold, speedKPH);
//    }

//    void UpdateCameraPosition()
//    {
//        Vector3 targetPosition = busTransform.position + busTransform.rotation * localPositionOffset;

//        transform.position = Vector3.SmoothDamp(
//            transform.position,
//            targetPosition,
//            ref positionVelocity,
//            positionSmoothTime
//        );
//    }

//    void UpdateCameraRotation(float speedAlpha)
//    {
//        float targetYRotation = busTransform.eulerAngles.y + yRotationOffset;
//        float rotationDamping = Mathf.Lerp(lowSpeedRotationDamping, highSpeedRotationDamping, speedAlpha);

//        currentYRotation = Mathf.LerpAngle(currentYRotation, targetYRotation, rotationDamping * Time.deltaTime);

//        transform.rotation = Quaternion.Euler(cameraXRotation, currentYRotation, 0f);
//    }

//    void UpdateFieldOfView(float speedAlpha)
//    {
//        if (cameraComponent == null) return;
//        cameraComponent.fieldOfView = Mathf.Lerp(lowSpeedFOV, highSpeedFOV, speedAlpha);
//    }
//}
#endregion
#region Milestone 1 Sprint 3 - Finalized Dynamic Camera Follow
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Anchors")]
    public Transform busTransform;

    [Header("Rotation Settings")]
    public float cameraXRotation = 20f;

    [Header("FOV Settings")]
    public float lowSpeedFOV = 60f;
    public float highSpeedFOV = 75f;

    [Header("Damping Settings")]
    public float lowSpeedRotationDamping = 4f;
    public float highSpeedRotationDamping = 12f;

    [Header("Speed Range")]
    public float lowSpeedThreshold = 20f;
    public float highSpeedThreshold = 80f;

    private Camera cameraComponent;
    private Vector3 localPositionOffset;
    private float currentYRotation;
    private float yRotationOffset;
    private Rigidbody busRigidbody;

    void Start()
    {
        cameraComponent = GetComponent<Camera>();
        busRigidbody = busTransform.GetComponentInParent<Rigidbody>();
        localPositionOffset = Quaternion.Inverse(busTransform.rotation) * (transform.position - busTransform.position);
        yRotationOffset = transform.eulerAngles.y - busTransform.eulerAngles.y;
        currentYRotation = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (busTransform == null) return;

        float speedKPH = CalculateSpeedKPH();
        float speedAlpha = CalculateSpeedAlpha(speedKPH);

        UpdateCameraPosition();
        UpdateCameraRotation(speedAlpha);
        UpdateFieldOfView(speedAlpha);
    }

    float CalculateSpeedKPH()
    {
        if (busRigidbody == null) return 0f;
        return busRigidbody.linearVelocity.magnitude * 3.6f;
    }

    float CalculateSpeedAlpha(float speedKPH)
    {
        return Mathf.InverseLerp(lowSpeedThreshold, highSpeedThreshold, speedKPH);
    }

    void UpdateCameraPosition()
    {
        transform.position = busTransform.position + busTransform.rotation * localPositionOffset;
    }

    void UpdateCameraRotation(float speedAlpha)
    {
        float targetYRotation = busTransform.eulerAngles.y + yRotationOffset;
        float rotationDamping = Mathf.Lerp(lowSpeedRotationDamping, highSpeedRotationDamping, speedAlpha);

        currentYRotation = Mathf.LerpAngle(currentYRotation, targetYRotation, rotationDamping * Time.deltaTime);

        transform.rotation = Quaternion.Euler(cameraXRotation, currentYRotation, 0f);
    }

    void UpdateFieldOfView(float speedAlpha)
    {
        if (cameraComponent == null) return;
        cameraComponent.fieldOfView = Mathf.Lerp(lowSpeedFOV, highSpeedFOV, speedAlpha);
    }
}
#endregion