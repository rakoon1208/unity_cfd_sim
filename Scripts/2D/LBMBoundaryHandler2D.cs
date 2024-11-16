using UnityEngine;

public class LBMBoundaryHandler2D : MonoBehaviour
{
    public LBMSimulation2D lbmSimulation;

    public void ApplyObstacle(int startX, int endX, int startY, int endY)
    {
        int gridSize = lbmSimulation.gridSize;
        Vector2[] velocities = new Vector2[gridSize * gridSize];
        lbmSimulation.velocityBuffer.GetData(velocities);

        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                int index = x + y * gridSize;
                velocities[index] = Vector2.zero;
            }
        }

        lbmSimulation.velocityBuffer.SetData(velocities);
    }
}
