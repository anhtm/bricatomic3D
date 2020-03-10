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
    internal float DEFAULT_UNIT = 1.0f;

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

    public GameObject GetPrefab()
    {
        int random = Random.Range(0, 2);
        BlockType chosenType = (BlockType)random;
        foreach (var slot in BlockLookUpTable)
        {
            if (slot.type == chosenType)
            {
                return slot.blockPrefab;
            }
        }
        return null;
    }

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

    public GameObject InitBlock(Vector3 position)
    {
        Vector3 blockPosition = new Vector3(position.x, DEFAULT_UNIT / 2, position.z);
        return Instantiate(GetPrefab(), blockPosition, Quaternion.identity);
    }

    public void InitBlock(BlockType type)
    {
        Instantiate(GetPrefab(type), new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z), Quaternion.identity);
    }
}
