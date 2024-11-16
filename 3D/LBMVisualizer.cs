using UnityEngine;
using Unity.Mathematics;

public class LBMVisualizer : MonoBehaviour
{
    public LBMSimulation lbmSimulation; // Reference to the main LBM simulation
    public Material visualizationMaterial; // Material used to display the fluid

    private Texture3D fluidTexture; // 3D texture to hold fluid data for visualization
    private int gridSize;

    void Start()
    {
        gridSize = lbmSimulation.gridSize;

        // Initialize a 3D texture with the grid size
        fluidTexture = new Texture3D(gridSize, gridSize, gridSize, TextureFormat.RGBA32, false);
        fluidTexture.wrapMode = TextureWrapMode.Clamp;
        visualizationMaterial.SetTexture("_FluidTexture", fluidTexture);
    }

    void Update()
    {
        UpdateVisualization();
    }

    private void UpdateVisualization()
    {
        // Retrieve data from the LBM simulation buffers
        float[] densities = new float[gridSize * gridSize * gridSize];
        lbmSimulation.densityBuffer.GetData(densities);

        Color[] colors = new Color[gridSize * gridSize * gridSize];
        for (int i = 0; i < densities.Length; i++)
        {
            // Map density to a color gradient, e.g., from blue to red
            float density = Mathf.Clamp01(densities[i]); // Normalizing density
            colors[i] = new Color(density, 0.0f, 1.0f - density, 1.0f); // Blue to red gradient
        }

        // Apply the colors to the 3D texture
        fluidTexture.SetPixels(colors);
        fluidTexture.Apply();
    }
}
