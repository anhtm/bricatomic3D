﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Draw Gizmo grid and responsible for finding the nearest position to insert a block
/// </summary>
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
}
