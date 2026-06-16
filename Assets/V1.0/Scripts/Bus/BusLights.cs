#region Summary
//<summary>
// This script controls the bus's lighting systems. It currently handles
// headlights: toggling the front Spot Lights on/off and swapping the
// headlight bulb material to look lit or unlit.
//</summary>
#endregion
#region Milestone Sprint 1 - Headlights
using UnityEngine;
using UnityEngine.InputSystem;

public class BusLights : MonoBehaviour
{
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

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.lKey.wasPressedThisFrame)
        {
            headlightsOn = !headlightsOn;
            SetHeadlights(headlightsOn);
        }
    }

    void SetHeadlights(bool isOn)
    {
        headlightLeft.enabled = isOn;
        headlightRight.enabled = isOn;

        Material bulbMaterial = isOn ? headlightOnMaterial : headlightOffMaterial;
        bulbLeft.material = bulbMaterial;
        bulbRight.material = bulbMaterial;
    }
}
#endregion