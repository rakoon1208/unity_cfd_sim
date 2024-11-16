using UnityEngine;
using UnityEngine.UI;

public class LBMUIController : MonoBehaviour
{
    public LBMSimulation lbmSimulation;
    public Slider viscositySlider;
    public Text viscosityLabel;

    void Start()
    {
        // Initialize the slider value and add a listener for when it changes
        viscositySlider.value = lbmSimulation.viscosity;
        viscositySlider.onValueChanged.AddListener(OnViscosityChanged);

        // Update label to show initial viscosity value
        UpdateViscosityLabel(lbmSimulation.viscosity);
    }

    void OnViscosityChanged(float newValue)
    {
        // Update viscosity in the simulation and label
        lbmSimulation.SetViscosity(newValue);
        UpdateViscosityLabel(newValue);
    }

    private void UpdateViscosityLabel(float value)
    {
        viscosityLabel.text = $"Viscosity: {value:F2}";
    }
}
