#region Summary
//<summary>
// This script runs one traffic light. It cycles Green, Yellow, Red on
// timers set in the Inspector, and lights only the active lamp.
//</summary>
#endregion

///Milestone 3 Sprint 1 - Traffic Light Cycle
using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
    void Update()
    {
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

    [Header("Lamp Materials")]
    public Material redOnMaterial;
    public Material yellowOnMaterial;
    public Material greenOnMaterial;
    public Material lampOffMaterial;

    [Header("Cycle Settings")]
    public float greenDuration = 10f;
    public float yellowDuration = 3f;
    public float redDuration = 10f;
    public LightState startState = LightState.Red;

    private LightState currentState;
    private float stateTimer = 0f;

    void Start()
    {
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

        redLamp.material = redOn ? redOnMaterial : lampOffMaterial;
        yellowLamp.material = yellowOn ? yellowOnMaterial : lampOffMaterial;
        greenLamp.material = greenOn ? greenOnMaterial : lampOffMaterial;

        redLight.enabled = redOn;
        yellowLight.enabled = yellowOn;
        greenLight.enabled = greenOn;
    }
    #endregion
}
