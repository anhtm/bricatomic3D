﻿using UnityEngine;
using System.Collections.Generic;

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

    public void PlaceBlockAt(Vector3 position)
    {
        if (PositionIsValid(position))
        {
            AddBlock(position);
            Debug.Log($"BoardManager::PlaceBlockNear::Added block at [{(int)position.x}, {(int)position.y}, {(int)position.z}].");
        }
        else
        {
            Debug.LogWarning($"BoardManager::PlaceBlockNear::Cannot add block at [{(int)position.x}, {(int)position.y}, {(int)position.z}]\n. Location is either out of bounds or taken.");
        }
    }

    void AddBlock(Vector3 position)
    {
        GameObject newBlock = BlockManager.Instance.InitBlock(position);
        board[(int)position.x, (int)position.y, (int)position.z] = newBlock;
    }

    public void RemoveBlockAt(Vector3 position)
    {
        if (PositionIsValid(position, false))
        {
            RemoveBlock(position);
            Debug.Log($"BoardManager::RemoveBlockNear::Removed block at [{(int)position.x}, {(int)position.y}, {(int)position.z}].");
        }
        else
        {
            Debug.LogWarning($"BoardManager::RemoveBlockNear::Cannot remove block at [{(int)position.x}, {(int)position.y}, {(int)position.z}]\n. Location is either out of bounds or empty.");
        }
    }

    public void RemoveBlock(Vector3 position)
    {
        int coordX = (int)position.x;
        int coordY = (int)position.y;
        int coordZ = (int)position.z;
        var blockToRemove = board[coordX, coordY, coordZ];
        board[coordX, coordY, coordZ] = null;
        Destroy(blockToRemove);
    }

    private bool PositionIsValid(Vector3 position, bool toAdd = true)
    {
        int coordX = (int)position.x;
        int coordY = (int)position.y;
        int coordZ = (int)position.z;
        bool validForOperation = (board[coordX, coordY, coordZ] == null) == toAdd;
        return coordX < board.GetLength(0) && coordZ < board.GetLength(1) && validForOperation;
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
                    if (go != null)
                    {
                        Debug.Log($"Position at [{i}, {j}, {k}] is " + board[i, j, k].transform.position + "\t");
                    }
                }
            }
        }
    }

}
