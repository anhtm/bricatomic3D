using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTemplate : MonoBehaviour
{
    [SerializeField] private float gapSize = 1f;

    #region Singleton
    private static GridTemplate _instance = null;

    public static GridTemplate Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GridTemplate>();
            }
            return _instance;
        }
    }
    #endregion

    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {
        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x / gapSize);
        int yCount = Mathf.RoundToInt(position.y / gapSize);
        int zCount = Mathf.RoundToInt(position.z / gapSize);

        Vector3 result = new Vector3(
            xCount * gapSize,
            yCount * gapSize,
            zCount * gapSize);

        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        float gizmoXorigin = BoardManager.Instance.origins.x - BoardManager.Instance.sizeExtent.x;
        float gizmoZorigin = BoardManager.Instance.origins.z - BoardManager.Instance.sizeExtent.z;

        for (float x = gizmoXorigin; x < BoardManager.Instance.sizeExtent.x * 2; x += gapSize)
        {
            for (float z = gizmoZorigin; z < BoardManager.Instance.sizeExtent.z * 2; z += gapSize)
            {
                var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
                Gizmos.DrawSphere(point, 0.1f);
            }
        }
    }
}
