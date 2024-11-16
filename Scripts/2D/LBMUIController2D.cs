using UnityEngine;
using UnityEngine.UI;

public class LBMUIController2D : MonoBehaviour
{
    public LBMSimulation2D lbmSimulation;
    public Slider viscositySlider;
    public Text viscosityLabel;
    public Slider inletVelocitySlider;

    void Start()
    {
        viscositySlider.value = lbmSimulation.viscosity;
        viscositySlider.onValueChanged.AddListener(OnViscosityChanged);
        UpdateViscosityLabel(lbmSimulation.viscosity);

        inletVelocitySlider.value = lbmSimulation.inletVelocity.x;
        inletVelocitySlider.onValueChanged.AddListener(OnInletVelocityChanged);
    }

    void OnViscosityChanged(float newValue)
    {
        lbmSimulation.viscosity = newValue;
        lbmSimulation.lbmShader.SetFloat("viscosity", newValue);
        UpdateViscosityLabel(newValue);
    }

    void OnInletVelocityChanged(float newVelocity)
    {
        lbmSimulation.inletVelocity = new Vector2(newVelocity, 0);
        InitializeInletVelocity();
    }

    void UpdateViscosityLabel(float value)
    {
        viscosityLabel.text = $"Viscosity: {value:F2}";
    }

    void InitializeInletVelocity()
    {
        int gridSize = lbmSimulation.gridSize;
        Vector2[] velocities = new Vector2[gridSize * gridSize];
        lbmSimulation.velocityBuffer.GetData(velocities);

        for (int y = 0; y < gridSize; y++)
        {
            int index = y * gridSize;
            velocities[index] = lbmSimulation.inletVelocity;
        }

        lbmSimulation.velocityBuffer.SetData(velocities);
    }
}
