using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    public float size = 1f;

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / size);
        int yCount = Mathf.RoundToInt(position.y / size);
        int zCount = Mathf.RoundToInt(position.z / size);

        Vector3 result = new Vector3(
            (float)xCount * size,
            (float)yCount * size,
            (float)zCount * size);

        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = 0; x < 3; x += size)
        {
            for (float y = 0; y < 3; y += size)
            {
                for (float z = 0; z < 3; z += size)
                {
                    var point = GetNearestPointOnGrid(new Vector3(x, y, z));
                    Gizmos.DrawSphere(point, 0.045f);
                }
            }
        }
    }
}
