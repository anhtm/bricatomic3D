using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles placing block UI
/// </summary>
public class BlockPlacer : MonoBehaviour
{

    [SerializeField] BlockType blockType = BlockType.OnexOne;

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

    //void PlaceBlockNear(Vector3 position)
    //{
    //    Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(position);
    //    GameObject block = new Block(blockType, nearestPoint).CreateBlock();
    //    BoardManager.Instance.AddBlockToBoard(block);
    //}

    //void RemoveBlockNear(Vector3 position)
    //{
    //    Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(position);
    //    GameObject blockToRemove = BoardManager.Instance.RemoveBlockAt(nearestPoint);
    //    Destroy(blockToRemove);
    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(hitInfo.point);
                BoardManager.Instance.PlaceBlockNear(nearestPoint);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo))
            {
                Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(hitInfo.point);
                BoardManager.Instance.RemoveBlockNear(nearestPoint);
            }
        }
    }
}
