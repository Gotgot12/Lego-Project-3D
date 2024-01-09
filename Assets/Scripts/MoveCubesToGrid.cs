using UnityEngine;

public class MoveCubesToGrid : MonoBehaviour
{
    public float gridSize = 1f;

    void Update()
    {
        MoveCubesWithTagToGrid("Lego");
    }

    void MoveCubesWithTagToGrid(string tag)
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject cube in cubes)
        {
            MoveCubeToGrid(cube);
        }
    }

    void MoveCubeToGrid(GameObject cube)
    {
        if (cube != null)
        {
            Vector3 newPosition = GetNearestGridPoint(cube.transform.position);
            cube.transform.position = newPosition;

            Debug.Log($"Moved {cube.name} to {newPosition}");
        }
        else
        {
            Debug.LogWarning("Cube is null.");
        }
    }

    Vector3 GetNearestGridPoint(Vector3 position)
    {
        // Snap the position to the nearest grid point
        float x = Mathf.Round(position.x / gridSize) * gridSize;
        float y = Mathf.Round(position.y / gridSize) * gridSize;
        float z = Mathf.Round(position.z / gridSize) * gridSize;

        return new Vector3(x, y, z);
    }
}
