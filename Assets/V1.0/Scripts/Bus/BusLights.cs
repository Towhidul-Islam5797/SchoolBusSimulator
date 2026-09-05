#region Summary
//<summary>
// This script controls the bus's lighting systems. It currently handles
// headlights: toggling the front Spot Lights on/off and swapping the
// headlight bulb material to look lit or unlit.
//</summary>
#endregion

///Milestone 2 Sprint 4 - Headlights + Rear Lights + Turn Signals + Hazard Lights
using UnityEngine;
using UnityEngine.InputSystem;
 
public class BusLights : MonoBehaviour
{
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.lKey.wasPressedThisFrame)
        {
            headlightsOn = !headlightsOn;
            SetHeadlights(headlightsOn);
        }

        UpdateRearLights(keyboard);
        UpdateHazardLights(keyboard);

        if (!hazardsOn)
        {
            UpdateTurnSignals(keyboard);
        }
    }
    #region Milestone Sprint 1 - Headlights
    [Header("Headlight Lights")]
    public Light headlightLeft;
    public Light headlightRight;

    [Header("Headlight Bulbs")]
    public Renderer bulbLeft;
    public Renderer bulbRight;

    [Header("Headlight Materials")]
    public Material headlightOnMaterial;
    public Material headlightOffMaterial;

    private bool headlightsOn = false;

    void SetHeadlights(bool isOn)
    {
        headlightLeft.enabled = isOn;
        headlightRight.enabled = isOn;

        Material bulbMaterial = isOn ? headlightOnMaterial : headlightOffMaterial;
        bulbLeft.material = bulbMaterial;
        bulbRight.material = bulbMaterial;
    }
    #endregion

    #region Milestone Sprint 2 - Brake and Reverse Lights
    [Header("Rear Light Bulbs")]
    public Renderer rearLightLeft;
    public Renderer rearLightRight;

    [Header("Rear Spot Lights")]
    public Light rearLightSpotLeft;
    public Light rearLightSpotRight;

    [Header("Rear Light Materials")]
    public Material rearLightOnMaterial;
    public Material rearLightOffMaterial;

    [Header("Rear Light Settings")]
    public float blinkInterval = 0.25f;

    public BusController busController;

    private float blinkTimer = 0f;
    private bool blinkState = false;

    void UpdateRearLights(Keyboard keyboard)
    {
        bool sPressed = keyboard.sKey.isPressed;
        BusController.Gear gear = busController.currentGear;

        bool braking = sPressed && (gear == BusController.Gear.D || gear == BusController.Gear.N);
        bool reversing = gear == BusController.Gear.R;
        bool brakingInReverse = sPressed && gear == BusController.Gear.R;

        if (braking || brakingInReverse)
        {
            SetRearLights(true);
        }
        else if (reversing)
        {
            UpdateBlink();
        }
        else
        {
            SetRearLights(false);
            blinkTimer = 0f;
            blinkState = false;
        }
    }

    void UpdateBlink()
    {
        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            blinkTimer = 0f;
            blinkState = !blinkState;
            SetRearLights(blinkState);
        }
    }

    void SetRearLights(bool isOn)
    {
        Material rearMaterial = isOn ? rearLightOnMaterial : rearLightOffMaterial;
        rearLightLeft.material = rearMaterial;
        rearLightRight.material = rearMaterial;

        rearLightSpotLeft.enabled = isOn;
        rearLightSpotRight.enabled = isOn;
    }
    #endregion

    #region Milestone Sprint 3 - Turn Signals
    //<summary>
    // Left and right turn signals. Z toggles left, C toggles right.
    // Only one side can be active at a time, pressing one cancels the
    // other, the same way a real turn signal stalk works.
    //</summary>
    [Header("Turn Signal Bulbs - Left")]
    public Renderer turnSignalFrontLeft;
    public Renderer turnSignalRearLeft;

    [Header("Turn Signal Bulbs - Right")]
    public Renderer turnSignalFrontRight;
    public Renderer turnSignalRearRight;

    [Header("Turn Signal Spot Lights - Left")]
    public Light turnSignalSpotFrontLeft;
    public Light turnSignalSpotRearLeft;

    [Header("Turn Signal Spot Lights - Right")]
    public Light turnSignalSpotFrontRight;
    public Light turnSignalSpotRearRight;

    [Header("Turn Signal Materials")]
    public Material turnSignalOnMaterial;
    public Material turnSignalOffMaterial;

    [Header("Turn Signal Settings")]
    public float turnSignalBlinkInterval = 0.33f;

    private bool leftSignalOn = false;
    private bool rightSignalOn = false;
    private float turnSignalBlinkTimer = 0f;
    private bool turnSignalBlinkState = false;

    void UpdateTurnSignals(Keyboard keyboard)
    {
        if (keyboard.zKey.wasPressedThisFrame)
        {
            leftSignalOn = !leftSignalOn;
            if (leftSignalOn) rightSignalOn = false;
            turnSignalBlinkTimer = 0f;
            turnSignalBlinkState = false;
        }

        if (keyboard.cKey.wasPressedThisFrame)
        {
            rightSignalOn = !rightSignalOn;
            if (rightSignalOn) leftSignalOn = false;
            turnSignalBlinkTimer = 0f;
            turnSignalBlinkState = false;
        }

        if (leftSignalOn || rightSignalOn)
        {
            turnSignalBlinkTimer += Time.deltaTime;
            if (turnSignalBlinkTimer >= turnSignalBlinkInterval)
            {
                turnSignalBlinkTimer = 0f;
                turnSignalBlinkState = !turnSignalBlinkState;
            }
        }
        else
        {
            turnSignalBlinkState = false;
        }

        SetTurnSignalSide(turnSignalFrontLeft, turnSignalRearLeft, turnSignalSpotFrontLeft, turnSignalSpotRearLeft, leftSignalOn && turnSignalBlinkState);
        SetTurnSignalSide(turnSignalFrontRight, turnSignalRearRight, turnSignalSpotFrontRight, turnSignalSpotRearRight, rightSignalOn && turnSignalBlinkState);
    }

    void SetTurnSignalSide(Renderer front, Renderer rear, Light frontSpot, Light rearSpot, bool isOn)
    {
        Material signalMaterial = isOn ? turnSignalOnMaterial : turnSignalOffMaterial;
        front.material = signalMaterial;
        rear.material = signalMaterial;

        frontSpot.enabled = isOn;
        rearSpot.enabled = isOn;
    }
    #endregion

    #region Milestone Sprint 4 - Hazard Lights
    //<summary>
    // Hazard lights, toggled with X. All 4 turn signal bulbs blink
    // together in sync. While hazards are on, Z and C are locked out
    // completely, turn signals do not respond. Turning hazards off
    // returns Z and C to normal.
    //</summary>
    [Header("Hazard Light Settings")]
    public float hazardBlinkInterval = 0.33f;

    private bool hazardsOn = false;
    private float hazardBlinkTimer = 0f;
    private bool hazardBlinkState = false;

    void UpdateHazardLights(Keyboard keyboard)
    {
        if (keyboard.xKey.wasPressedThisFrame)
        {
            hazardsOn = !hazardsOn;
            hazardBlinkTimer = 0f;
            hazardBlinkState = false;

            if (hazardsOn)
            {
                leftSignalOn = false;
                rightSignalOn = false;
            }
        }

        if (!hazardsOn) return;

        hazardBlinkTimer += Time.deltaTime;
        if (hazardBlinkTimer >= hazardBlinkInterval)
        {
            hazardBlinkTimer = 0f;
            hazardBlinkState = !hazardBlinkState;
        }

        SetTurnSignalSide(turnSignalFrontLeft, turnSignalRearLeft, turnSignalSpotFrontLeft, turnSignalSpotRearLeft, hazardBlinkState);
        SetTurnSignalSide(turnSignalFrontRight, turnSignalRearRight, turnSignalSpotFrontRight, turnSignalSpotRearRight, hazardBlinkState);
    }
    #endregion
}