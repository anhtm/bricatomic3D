using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BlockSlot
{
    public BlockType type;
    public GameObject blockPrefab;

    //Constructor (not necessary, but helpful)
    public BlockSlot(BlockType type, GameObject blockPrefab)
    {
        this.type = type;
        this.blockPrefab = blockPrefab;
    }
}


/// <summary>
/// Manages block creation/deletion with different BlockTypes
/// </summary>
public class BlockManager : MonoBehaviour
{
    [SerializeField] public List<BlockSlot> BlockLookUpTable;
    [SerializeField] LayerMask layerMask;

    internal float DEFAULT_UNIT = 1.0f;
    internal GameObject currentPrefab;
    
    #region Singleton
    private static BlockManager _instance;

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

    public GameObject GetPrefab(BlockType type)
    {
        foreach (var slot in BlockLookUpTable)
        {
            if (slot.type == type)
            {
                return slot.blockPrefab;
            }
        }
        return null;
    }

    public GameObject InitBlock(BlockType type, Vector3 position)
    {
        Vector3 blockPosition = new Vector3(position.x, position.y, position.z);
        return Instantiate(GetPrefab(type), blockPosition, Quaternion.identity);
    }

    public GameObject InitBlock(Vector3 position)
    {
        return Instantiate(currentPrefab, position, Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 1000.0f, layerMask))
            {
                PlaceBlockNear(hitInfo.point);
            }
        }

    }

    void PlaceBlockNear(Vector3 position)
    {
        Debug.Log("PlaceBlockNear called");
        Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(position);
        GameObject block = InitBlock(nearestPoint);
        BoardManager.Instance.TryAddBlock(block);
    }
}
