using UnityEngine;

public class LBMVisualizer2D : MonoBehaviour
{
    public LBMSimulation2D lbmSimulation;
    public Material visualizationMaterial;
    
    private Texture2D fluidTexture;
    private int gridSize;

    void Start()
    {
        gridSize = lbmSimulation.gridSize;
        fluidTexture = new Texture2D(gridSize, gridSize, TextureFormat.RGBA32, false);
        visualizationMaterial.mainTexture = fluidTexture;
    }

    void Update()
    {
        UpdateVisualization();
    }

    private void UpdateVisualization()
    {
        float[] densities = new float[gridSize * gridSize];
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
