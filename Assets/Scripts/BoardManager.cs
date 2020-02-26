using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject boardObj;

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
        Debug.Log($"Size x: {sizeExtent.x}");
        Debug.Log($"Size z: {sizeExtent.z}");
        Debug.Log($"origins: {origins}");

        PrintBoard();
    }

    void InitBoard()
    {
        if (boardObj == null) { return; }
        origins = boardObj.GetComponent<Renderer>().bounds.center;
        sizeExtent = boardObj.GetComponent<Renderer>().bounds.extents;
        board = new GameObject[(int)sizeExtent.x * 2, (int)sizeExtent.z * 2];
    }

    public void AddBlockToBoard(GameObject block)
    {
        int coordX = (int)block.transform.position.x;
        int coordZ = (int)block.transform.position.z;
        if (board[coordX, coordZ] == null)
        {
            board[coordX, coordZ] = block;
            Debug.Log($"BoardManager::AddBlockToBoard::Added block {block.transform.position} at [{coordX}, {coordZ}]\t");
        } else
        {
            Debug.Log($"BoardManager::AddBlockToBoard::Cannot add block at [{coordX}, {coordZ}]. Position is occupied.\t");
        }
    }

    /// <summary>
    /// Return the removed block
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public GameObject RemoveBlockAt(Vector3 position)
    {
        int coordX = (int)position.x;
        int coordZ = (int)position.z;
        if (board[coordX, coordZ] != null)
        {
            Debug.Log($"BoardManager::RemoveBlock::Removed block at [{coordX}, {coordZ}]\t. Block is now: {board[coordX, coordZ]}");

            var blockToRemove = board[coordX, coordZ];
            board[coordX, coordZ] = null;
            return blockToRemove;
        }
        
        Debug.Log($"BoardManager::RemoveBlock::Cannot remove block at [{coordX}, {coordZ}]. Position is already empty.\t");
        return null;
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
