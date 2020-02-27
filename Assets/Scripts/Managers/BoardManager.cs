using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages data presentation of the board.
/// Used to load board, save board
/// </summary>
public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject boardObj;
    [SerializeField] GameObject blockPrefab;

    internal GameObject[,] board;
    internal Vector3 origins;
    internal Vector3 sizeExtent;

    #region Singleton
    private static BoardManager _instance = null;

    public static BoardManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<BoardManager>();
            }
            return _instance;
        }
    }
    #endregion

    void Start()
    {
        InitBoard();
    }

    void InitBoard()
    {
        if (boardObj == null) { return; }
        origins = boardObj.GetComponent<Renderer>().bounds.center;
        sizeExtent = boardObj.GetComponent<Renderer>().bounds.extents;
        board = new GameObject[(int)sizeExtent.x * 2, (int)sizeExtent.z * 2];
    }

    public void PlaceBlockNear(Vector3 position)
    {
        if (PositionIsValid(position))
        {
            AddBlockAt(position);
            Debug.Log($"BoardManager::PlaceBlockNear::Added block at [{(int)position.x}, {(int)position.z}].");
        }
        else
        {
            Debug.LogWarning($"BoardManager::PlaceBlockNear::Cannot add block at [{(int)position.x}, {(int)position.z}]\n. Location is either out of bounds or taken.");
        }
    }

    void AddBlockAt(Vector3 position)
    {
        Vector3 blockPosition = new Vector3(position.x, blockPrefab.GetComponent<Block>().DEFAULT_UNIT / 2, position.z);
        GameObject block = Instantiate(blockPrefab, blockPosition, Quaternion.identity);
        board[(int)block.transform.position.x, (int)block.transform.position.z] = block;
    }

    public void RemoveBlockNear(Vector3 position)
    {
        if (PositionIsValid(position, false))
        {
            RemoveBlockAt(position);
            Debug.Log($"BoardManager::RemoveBlockNear::Removed block at [{(int)position.x}, {(int)position.z}].");
        }
        else
        {
            Debug.LogWarning($"BoardManager::RemoveBlockNear::Cannot remove block at [{(int)position.x}, {(int)position.z}]\n. Location is either out of bounds or empty.");
        }
    }

    public void RemoveBlockAt(Vector3 position)
    {
        int coordX = (int)position.x;
        int coordZ = (int)position.z;
        var blockToRemove = board[coordX, coordZ];
        board[coordX, coordZ] = null;
        Destroy(blockToRemove);
    }

    private bool PositionIsValid(Vector3 position, bool toAdd = true)
    {
        int coordX = (int)position.x;
        int coordZ = (int)position.z;
        bool validForOperation = (board[coordX, coordZ] == null) == toAdd;
        return coordX < board.GetLength(0) && coordZ < board.GetLength(1) && validForOperation;
    }

    void PrintBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                var go = board[i, j];
                if (go != null)
                {
                    Debug.Log($"Position at [{i}, {j}] is " + board[i, j].transform.position + "\t");
                }
            }
        }
    }

}
