using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private float blockSize = 1f;

    #region Singleton
    private static BlockManager _instance = null;

    public static BlockManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BlockManager>();
            }
            return _instance;
        }
    }
    #endregion

    void PlaceBlockNear(Vector3 position)
    {
        Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(position);
        GameObject block = InitBlock(nearestPoint);
        BoardManager.Instance.AddBlockToBoard(block);
    }

    GameObject InitBlock(Vector3 blockPosition)
    {
        GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.transform.position = new Vector3(blockPosition.x, blockSize / 2, blockPosition.z);
        block.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
        return block;
    }

    void RemoveBlockNear(Vector3 position)
    {
        Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(position);
        GameObject blockToRemove = BoardManager.Instance.RemoveBlockAt(nearestPoint);
        Destroy(blockToRemove);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitInfo))
                {
                    PlaceBlockNear(hitInfo.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            {
                RaycastHit hitInfo;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hitInfo))
                {
                    RemoveBlockNear(hitInfo.point);
                }
            }
        }
    }
}
