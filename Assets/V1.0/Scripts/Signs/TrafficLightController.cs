#region Summary
//<summary>
// This script runs one traffic light. It cycles Green, Yellow, Red on
// timers set in the Inspector, and shows only the active lamp's glowing
// sphere. Hidden lamps reveal the model's own dark tinted lens.
// Point Lights only turn on while the traffic light is in LOD 0.
//</summary>
#endregion

///Milestone 3 Sprint 1b - Traffic Light Cycle + Point Light LOD
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    void Update()
    {
        UpdatePointLightLOD();

        stateTimer += Time.deltaTime;
        if (stateTimer >= GetDuration(currentState))
        {
            stateTimer = 0f;
            currentState = GetNextState(currentState);
            SetLamps(currentState);
        }
    }

    #region Milestone 3 Sprint 1 - Traffic Light Cycle
    public enum LightState { Green, Yellow, Red }

    [Header("Lamps")]
    public Renderer redLamp;
    public Renderer yellowLamp;
    public Renderer greenLamp;

    [Header("Point Lights")]
    public Light redLight;
    public Light yellowLight;
    public Light greenLight;

    [Header("Cycle Settings")]
    public float greenDuration = 10f;
    public float yellowDuration = 3f;
    public float redDuration = 10f;
    public LightState startState = LightState.Red;

    private LightState currentState;
    private float stateTimer = 0f;

    void Start()
    {
        lod0ScreenHeight = lodGroup.GetLODs()[0].screenRelativeTransitionHeight;
        currentState = startState;
        SetLamps(currentState);
    }

    float GetDuration(LightState state)
    {
        if (state == LightState.Green) return greenDuration;
        if (state == LightState.Yellow) return yellowDuration;
        return redDuration;
    }

    LightState GetNextState(LightState state)
    {
        if (state == LightState.Green) return LightState.Yellow;
        if (state == LightState.Yellow) return LightState.Red;
        return LightState.Green;
    }

    void SetLamps(LightState state)
    {
        bool redOn = state == LightState.Red;
        bool yellowOn = state == LightState.Yellow;
        bool greenOn = state == LightState.Green;

        redLamp.enabled = redOn;
        yellowLamp.enabled = yellowOn;
        greenLamp.enabled = greenOn;

        redLight.enabled = redOn && pointLightsAllowed;
        yellowLight.enabled = yellowOn && pointLightsAllowed;
        greenLight.enabled = greenOn && pointLightsAllowed;
    }
    #endregion

    #region Milestone 3 Sprint 1b - Point Light LOD
    //<summary>
    // A LOD Group can only hide meshes, not Lights. This checks how tall
    // the traffic light is on screen, using the same maths as the LOD
    // Group, and allows the Point Lights only while it is in LOD 0.
    // Drag the LOD 0 / LOD 1 divider in the Inspector to tune it.
    //</summary>
    [Header("Point Light LOD")]
    public LODGroup lodGroup;

    private float lod0ScreenHeight;
    private bool pointLightsAllowed = true;

    void UpdatePointLightLOD()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        bool inLod0 = GetScreenHeight(cam) >= lod0ScreenHeight;
        if (inLod0 != pointLightsAllowed)
        {
            pointLightsAllowed = inLod0;
            SetLamps(currentState);
        }
    }

    float GetScreenHeight(Camera cam)
    {
        Vector3 center = lodGroup.transform.TransformPoint(lodGroup.localReferencePoint);
        float distance = Vector3.Distance(cam.transform.position, center);

        Vector3 scale = lodGroup.transform.lossyScale;
        float largestScale = Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));
        float worldSize = lodGroup.size * largestScale;

        float viewHeight = 2f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return worldSize / viewHeight * QualitySettings.lodBias;
    }
    #endregion
}
