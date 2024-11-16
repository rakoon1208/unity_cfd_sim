using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBMSimulation : MonoBehaviour
{
    [Header("Simulation Settings")]
    public int gridSize = 32; // Size of the 3D grid
    public float viscosity = 0.02f; // Viscosity parameter

    [Header("Compute Shader")]
    public ComputeShader lbmShader;

    private ComputeBuffer densityBuffer;
    private ComputeBuffer velocityBuffer;

    // Grid properties
    private float[,,] density; // Density values for each cell
    private Vector3[,,] velocity; // Velocity values for each cell

    private int kernelHandle;
    private int totalCells;

    void Start()
    {
        InitializeGrid();
        InitializeComputeShader();
    }

    void InitializeGrid()
    {
        // Initialize density and velocity fields
        density = new float[gridSize, gridSize, gridSize];
        velocity = new Vector3[gridSize, gridSize, gridSize];

        // Initialize each cell with default density and zero velocity
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    density[x, y, z] = 1.0f; // Default density
                    velocity[x, y, z] = Vector3.zero; // Zero initial velocity
                }
            }
        }
    }

    void InitializeComputeShader()
    {
        // Get the kernel handle for the compute shader
        kernelHandle = lbmShader.FindKernel("CSMain");

        // Calculate total cells
        totalCells = gridSize * gridSize * gridSize;

        // Create compute buffers for density and velocity
        densityBuffer = new ComputeBuffer(totalCells, sizeof(float));
        velocityBuffer = new ComputeBuffer(totalCells, sizeof(float) * 3);

        // Initialize buffers with initial data
        float[] initialDensity = new float[totalCells];
        Vector3[] initialVelocity = new Vector3[totalCells];

        int index = 0;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                for (int z = 0; z < gridSize; z++)
                {
                    initialDensity[index] = density[x, y, z];
                    initialVelocity[index] = velocity[x, y, z];
                    index++;
                }
            }
        }

        // Set data to buffers
        densityBuffer.SetData(initialDensity);
        velocityBuffer.SetData(initialVelocity);

        // Bind buffers to the compute shader
        lbmShader.SetBuffer(kernelHandle, "densityBuffer", densityBuffer);
        lbmShader.SetBuffer(kernelHandle, "velocityBuffer", velocityBuffer);

        // Set other shader parameters
        lbmShader.SetInt("gridSize", gridSize);
        lbmShader.SetFloat("viscosity", viscosity);
    }

    void Update()
    {
        // Dispatch the compute shader
        int threadGroups = Mathf.CeilToInt(gridSize / 8.0f);
        lbmShader.Dispatch(kernelHandle, threadGroups, threadGroups, threadGroups);
    }

    private void OnDestroy()
    {
        // Release buffers to free up memory
        if (densityBuffer != null) densityBuffer.Release();
        if (velocityBuffer != null) velocityBuffer.Release();
    }

    public void SetViscosity(float newViscosity)
    {
        // Update the viscosity parameter dynamically
        viscosity = newViscosity;
        lbmShader.SetFloat("viscosity", viscosity);
    }
}
