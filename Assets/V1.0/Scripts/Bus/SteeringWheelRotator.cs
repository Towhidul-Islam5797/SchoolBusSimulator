using UnityEngine;

public class SteeringWheelRotator : MonoBehaviour
{
    public BusController busController;

    [Header("Rotation Settings")]
    public float maxRotationAngle = 180f;

    void Update()
    {
        float targetZ = -busController.currentSteer * maxRotationAngle;
        transform.localEulerAngles = new Vector3(
            transform.localEulerAngles.x,
            transform.localEulerAngles.y,
            targetZ
        );
    }
}