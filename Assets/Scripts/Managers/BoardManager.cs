using UnityEngine;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages data presentation of the board.
/// Used to load board, save board
/// </summary>
public class BoardManager : MonoBehaviour
{
    [SerializeField] internal GameObject boardObj;

    internal GameObject[,,] board;
    internal Vector3 origins;
    internal Vector3 sizeExtent;
    internal int boardHeightLimit = 10; // Limit to 10 floors of blocks

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
        board = new GameObject[(int)sizeExtent.x * 2, boardHeightLimit, (int)sizeExtent.z * 2];
    }

    public void TryAddBlock(GameObject block)
    {
        Vector3 position = block.transform.position;
        try
        {
            if (IsValid(position))
            {
                Debug.Log($"BoardManager::TryAddBlock::add block at [{(int)position.x}, {(int)position.y}, {(int)position.z}]");
                board[(int)position.x, (int)position.y, (int)position.z] = block;
            } else
            {
                Debug.LogWarning($"BoardManager::TryAddBlock::Cannot add block at [{(int)position.x}, {(int)position.y}, {(int)position.z}]\n. Location might be overlapping.");
            }
        } catch (IndexOutOfRangeException)
        {
            Destroy(block);
        }
    }

    public void TryRemoveBlock(GameObject block)
    {
        Vector3 position = block.transform.position;

        if (IsValid(position))
        {
            board[(int)position.x, (int)position.y, (int)position.z] = null;
            Destroy(block);
        } else
        {
            Debug.LogWarning($"BoardManager::TryRemoveBlock::Cannot remove block at [{(int)position.x}, {(int)position.y}, {(int)position.z}]\n. Location is either out of bounds or empty.");
        }
    }

    public void TryMoveBlock(GameObject block, Vector3 previousPos)
    {
        Vector3 currentPos = block.transform.position;
        try
        {
            if (IsValid(currentPos))
            {
                board[(int)currentPos.x, (int)currentPos.y, (int)currentPos.z] = block;
                // Remove old positioning 
                board[(int)previousPos.x, (int)previousPos.y, (int)previousPos.z] = null;
            }
            else
            {
                // Reposition the block back to its previous location
                block.transform.position = previousPos;
                Debug.LogWarning($"BoardManager::TryMoveBlock::Cannot move block to [{(int)currentPos.x}, {(int)currentPos.y}, {(int)currentPos.z}]\n. Location might be overlapping.");
            }
        }
        catch (IndexOutOfRangeException)
        {
            block.transform.position = previousPos;
        }
    }

    private bool IsValid(Vector3 position)
    {
        int coordX = (int)position.x;
        int coordY = (int)position.y;
        int coordZ = (int)position.z;
        bool isInBound = coordX < board.GetLength(0) && coordY < board.GetLength(1) && coordZ < board.GetLength(2);
        return isInBound && !IsOverlap(position);
    }

    private bool IsOverlap(Vector3 position)
    {
        // TODO: Implement checking if block is overlapping other blocks.
        // Note: There has to be some kind of method to calculate the space a block takes 
        return false;
    }

    void PrintBoard()
    {
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                for (int k = 0; k < board.GetLength(2); k++)
                {
                    var go = board[i, j, k];
                    var info = go != null ? board[i, j, k].transform.position.ToString() : "NULL";
                    if (go != null)
                    {
                        Debug.Log($"Position at [{i}, {j}, {k}] is " + info);
                    }
                }
            }
        }
    }

}
