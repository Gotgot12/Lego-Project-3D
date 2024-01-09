using UnityEngine;

public class GrabObject : MonoBehaviour
{
    private Vector3 offset;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            HandleMouseDragXY();
        }
        else
        {
            HandleMouseDragZX();
        }

        // Always snap to the nearest point on the grid
        SnapToGrid();
    }

    private void HandleMouseDragXY()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            transform.position = new Vector3(
                hit.point.x + offset.x,
                hit.point.y + offset.y,
                transform.position.z
            );
        }
    }

    private void HandleMouseDragZX()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            transform.position = new Vector3(
                hit.point.x + offset.x,
                transform.position.y,
                hit.point.z + offset.z
            );
        }
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            offset = transform.position - hit.point;
        }
    }

    private void SnapToGrid()
    {
        transform.position = new Vector3(
            Mathf.Round(transform.position.x / GridManager.Instance.GridSize) * GridManager.Instance.GridSize,
            Mathf.Round(transform.position.y / GridManager.Instance.GridSize) * GridManager.Instance.GridSize,
            Mathf.Round(transform.position.z / GridManager.Instance.GridSize) * GridManager.Instance.GridSize
        );
    }
}
