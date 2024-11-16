using UnityEngine;

public class LBMSimulation2D : MonoBehaviour
{
    [Header("Simulation Settings")]
    public int gridSize = 128;
    public float viscosity = 0.02f;
    public Vector2 inletVelocity = new Vector2(1.0f, 0); // Flow direction from left to right

    [Header("Compute Shader")]
    public ComputeShader lbmShader;

    private ComputeBuffer densityBuffer;
    private ComputeBuffer velocityBuffer;
    private int kernelHandle;
    private int totalCells;

    void Start()
    {
        InitializeBuffers();
        kernelHandle = lbmShader.FindKernel("CSMain");
    }

    void InitializeBuffers()
    {
        totalCells = gridSize * gridSize;

        densityBuffer = new ComputeBuffer(totalCells, sizeof(float));
        velocityBuffer = new ComputeBuffer(totalCells, sizeof(float) * 2);

        float[] densities = new float[totalCells];
        Vector2[] velocities = new Vector2[totalCells];

        for (int i = 0; i < totalCells; i++)
        {
            densities[i] = 1.0f;
            velocities[i] = inletVelocity;
        }

        densityBuffer.SetData(densities);
        velocityBuffer.SetData(velocities);

        lbmShader.SetBuffer(kernelHandle, "densityBuffer", densityBuffer);
        lbmShader.SetBuffer(kernelHandle, "velocityBuffer", velocityBuffer);
        lbmShader.SetInt("gridSize", gridSize);
        lbmShader.SetFloat("viscosity", viscosity);
    }

    void Update()
    {
        int threadGroups = Mathf.CeilToInt(gridSize / 8.0f);
        lbmShader.Dispatch(kernelHandle, threadGroups, threadGroups, 1);
    }

    private void OnDestroy()
    {
        densityBuffer.Release();
        velocityBuffer.Release();
    }
}
