﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles placing block UI
/// </summary>
public class BlockPlacer : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;

    #region Singleton
    private static BlockPlacer _instance = null;

    public static BlockPlacer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BlockPlacer>();
            }
            return _instance;
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit hitInfo;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(hitInfo.point);
        //        BoardManager.Instance.PlaceBlockAt(nearestPoint);
        //    }
        //}

        //if (Input.GetMouseButtonDown(1))
        //{
        //    RaycastHit hitInfo;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (Physics.Raycast(ray, out hitInfo))
        //    {
        //        Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(hitInfo.point);
        //        BoardManager.Instance.RemoveBlockAt(nearestPoint);
        //    }
        //}
    }
}
