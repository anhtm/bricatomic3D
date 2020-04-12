using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draw Gizmo grid and responsible for finding the nearest position to insert a block
/// </summary>
public class GridTemplate : MonoBehaviour
{
    [SerializeField] private float gapSize = 1f;
    [SerializeField] GameObject ground;

    private void Start()
    {
        DrawBoardGround();
    }

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;
        float xCount = Mathf.Round(position.x / gapSize);
        float yCount = Mathf.Round(position.y / gapSize);
        float zCount = Mathf.Round(position.z / gapSize);

        Vector3 result = new Vector3(
            xCount * gapSize,
            Mathf.Round(yCount * gapSize),
            zCount * gapSize);

        result += transform.position;

        if (result.y < 0)
        {
            Debug.Log("GetNearestPointOnGrid::y" + result.y);
        }

        return result;
    }

    private void OnDrawGizmos()
    {
        //DrawGizmos();
    }

    private void DrawGizmos()
    {
        Gizmos.color = Color.grey;
        float cubeSize = 0.05f;
        float gizmoXorigin = BoardManager.Instance.origins.x - BoardManager.Instance.sizeExtent.x;
        float gizmoZorigin = BoardManager.Instance.origins.z - BoardManager.Instance.sizeExtent.z;

        for (float x = gizmoXorigin; x < BoardManager.Instance.sizeExtent.x * 2; x += gapSize)
        {
            for (float y = 0; y < BoardManager.Instance.boardHeightLimit; y += gapSize)
            {
                for (float z = gizmoZorigin; z < BoardManager.Instance.sizeExtent.z * 2; z += gapSize)
                {
                    var point = GetNearestPointOnGrid(new Vector3(x, y, z));
                    Gizmos.DrawCube(point, new Vector3(cubeSize, cubeSize, cubeSize));
                }
            }
        }
    }

    internal void DrawBoardGround()
    {
        GameObject container = new GameObject("GroundContainer");

        float originX = BoardManager.Instance.origins.x - BoardManager.Instance.sizeExtent.x;
        float originZ = BoardManager.Instance.origins.z - BoardManager.Instance.sizeExtent.z;
        float originY = BoardManager.Instance.origins.y;

        for (float x = originX; x < BoardManager.Instance.sizeExtent.x * 2; x += gapSize)
        {
            for (float z = originZ; z < BoardManager.Instance.sizeExtent.z * 2; z += gapSize)
            {
                var point = new Vector3(x, originY, z);
                GameObject cube = Instantiate(ground, point, Quaternion.identity);
                cube.tag = "Untagged"; 
                cube.transform.parent = container.transform;
            }
        }
    }
}
