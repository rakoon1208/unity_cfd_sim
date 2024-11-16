using UnityEngine;

public class LBMBoundaryHandler : MonoBehaviour
{
    public LBMSimulation lbmSimulation;

    void Start()
    {
        ApplyBoundaryConditions();
    }

    public void ApplyBoundaryConditions()
    {
        int gridSize = lbmSimulation.gridSize;
        float[] densities = new float[gridSize * gridSize * gridSize];
        Vector3[] velocities = new Vector3[gridSize * gridSize * gridSize];

        // Get data from buffers
        lbmSimulation.densityBuffer.GetData(densities);
        lbmSimulation.velocityBuffer.GetData(velocities);

        // Set boundary conditions - zero velocity on edges (walls)
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                velocities[x + y * gridSize + 0 * gridSize * gridSize] = Vector3.zero; // Bottom layer
                velocities[x + y * gridSize + (gridSize - 1) * gridSize * gridSize] = Vector3.zero; // Top layer
            }
        }

        // Set boundary values back to buffers
        lbmSimulation.densityBuffer.SetData(densities);
        lbmSimulation.velocityBuffer.SetData(velocities);
    }

    public void SetInletCondition(Vector3 inletVelocity, int inletLayer)
    {
        int gridSize = lbmSimulation.gridSize;
        Vector3[] velocities = new Vector3[gridSize * gridSize * gridSize];

        // Get velocity data from buffer
        lbmSimulation.velocityBuffer.GetData(velocities);

        // Apply inlet condition at specified layer
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                int index = x + y * gridSize + inletLayer * gridSize * gridSize;
                velocities[index] = inletVelocity;
            }
        }

        // Update buffer with inlet condition
        lbmSimulation.velocityBuffer.SetData(velocities);
    }
}
