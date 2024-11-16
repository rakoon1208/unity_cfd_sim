using UnityEngine;
using UnityEngine.UI;

public class LBMUIController : MonoBehaviour
{
    public LBMSimulation lbmSimulation;
    public Slider viscositySlider;
    public Text viscosityLabel;

    void Start()
    {
        viscositySlider.value = lbmSimulation.viscosity;
        viscositySlider.onValueChanged.AddListener(OnViscosityChanged);
        UpdateViscosityLabel(lbmSimulation.viscosity);
    }

    void OnViscosityChanged(float newValue)
    {
        lbmSimulation.viscosity = newValue;
        lbmSimulation.lbmShader.SetFloat("viscosity", newValue);
        UpdateViscosityLabel(newValue);
    }

    void UpdateViscosityLabel(float value)
    {
        viscosityLabel.text = $"Viscosity: {value:F2}";
    }
}
