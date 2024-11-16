using UnityEngine;
using Unity.Mathematics;

public class LBMVisualizer : MonoBehaviour
{
    public LBMSimulation lbmSimulation; // Reference to the simulation
    public Material visualizationMaterial; // Material for visualizing the fluid

    private Texture3D fluidTexture;
    private int gridSize;

    void Start()
    {
        gridSize = lbmSimulation.gridSize;

        // Create a 3D texture for visualization
        fluidTexture = new Texture3D(gridSize, gridSize, gridSize, TextureFormat.RGBA32, false);
        visualizationMaterial.SetTexture("_FluidTexture", fluidTexture);
    }

    void Update()
    {
        UpdateVisualization();
    }

    void UpdateVisualization()
    {
        float[] densities = new float[gridSize * gridSize * gridSize];
        lbmSimulation.densityBuffer.GetData(densities);

        Color[] colors = new Color[densities.Length];
        for (int i = 0; i < densities.Length; i++)
        {
            float density = Mathf.Clamp01(densities[i]);
            colors[i] = new Color(density, 0, 1 - density, 1);
        }

        fluidTexture.SetPixels(colors);
        fluidTexture.Apply();
    }
}
