using UnityEngine;
using TMPro;

public class SpeedometerUI : MonoBehaviour
{
    public BusController busController;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI gearText;

    void Update()
    {
        speedText.text = (busController.currentSpeed ) + " MPH";
        gearText.text = busController.currentGear.ToString();
    }
}