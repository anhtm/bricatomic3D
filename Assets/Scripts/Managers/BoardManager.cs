using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class BoardManager : MonoBehaviour
{
    [SerializeField] internal GameObject boardObj;

    internal ScriptableBlock[,,] boardData;
    internal Vector3 origins;
    internal Vector3 sizeExtent;
    internal int boardHeightLimit = 20; // Limit to 20 floors of blocks

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

    private void Awake()
    {
        UpdateBoardSize();
    }

    void Start()
    {
        InitBoard();
    }


    void UpdateBoardSize()
    {
        if (boardObj == null || Game.current == null) { return; }
        float userChosenBoardSize = (float)Math.Round(Game.current.boardSize);
        boardObj.transform.localScale = new Vector3(userChosenBoardSize, 0, userChosenBoardSize);
    }

    void InitBoard()
    {
        if (boardObj == null) { return; }
        origins = boardObj.GetComponent<Renderer>().bounds.center;
        sizeExtent = boardObj.GetComponent<Renderer>().bounds.extents;

        boardData = new ScriptableBlock[(int)sizeExtent.x * 2, boardHeightLimit, (int)sizeExtent.z * 2];

        if (Game.current != null && Game.current.boardDataJSON != null)
        {
            LoadBoardDataAndSpawn(Game.current.boardDataJSON);
        }
    }

    public void SaveBoard()
    {
        Game.current.boardDataJSON = FormatBoardDataToSave();
    }

    public ArrayList FormatBoardDataToSave()
    {
        ArrayList result = new ArrayList();
        for (int i = 0; i < boardData.GetLength(0); i++)
        {
            for (int j = 0; j < boardData.GetLength(1); j++)
            {
                for (int k = 0; k < boardData.GetLength(2); k++)
                {
                    var data = boardData[i, j, k];
                    if (data != null)
                    {
                        result.Add(new BlockData(data, new Vector3(i, j, k)));
                    }
                }
            }
        }
        return result;
    }

    public void LoadBoardDataAndSpawn(ArrayList boardDataJson)
    {
        foreach (BlockData bd in boardDataJson)
        {
            Vector3 position = bd.convertPositionData();
            ScriptableBlock block = ScriptableObject.CreateInstance<ScriptableBlock>();
            bd.jsonParseData(block);
            boardData[(int)position.x, (int)position.y, (int)position.z] = block;
            BlockManager.Instance.InitBlock(block, position);
        }
    }

    public void TryAddBlock(ScriptableBlock metaData, GameObject block)
    {
        Vector3 position = block.transform.position;
        int yPos = Mathf.RoundToInt(position.y);

        try
        {
            if (IsValid(position))
            {
                
                boardData[(int)position.x, yPos, (int)position.z] = metaData;
                Debug.Log($"BoardManager::TryAddBlock::add block at [{(int)position.x}, {yPos}, {(int)position.z}]");
            }
            else
            {
                Debug.LogWarning($"BoardManager::TryAddBlock::Cannot add block at [{(int)position.x}, {yPos}, {(int)position.z}]\n. Location might be overlapping.");
                Destroy(block);
            }
        } catch (IndexOutOfRangeException)
        {
            Debug.LogWarning($"BoardManager::TryAddBlock::Cannot add block at [{(int)position.x}, {yPos}, {(int)position.z}]\n. Location is out of range.");
            Destroy(block);
        }
    }

    public void TryRemoveBlock(GameObject block)
    {
        Vector3 position = block.transform.position;
        int yPos = Mathf.RoundToInt(position.y);

        if (IsValid(position, false))
        {
            boardData[(int)position.x, yPos, (int)position.z] = null;
            Destroy(block);
        }
        else
        {
            Debug.LogWarning($"BoardManager::TryRemoveBlock::Cannot remove block at [{(int)position.x}, {yPos}, {(int)position.z}]\n. Location is either out of bounds or empty.");
        }
    }

    public void TryMoveBlock(GameObject block, Vector3 previousPos)
    {
        Vector3 currentPos = block.transform.position;
        int yPos = Mathf.RoundToInt(currentPos.y);

        try
        {
            if (IsValid(currentPos))
            {
                BoardDataMoveBlock(currentPos, previousPos);
                Debug.Log($"BoardManager::TryMoveBlock::Moved block to new position [{(int)currentPos.x}, {yPos}, {(int)currentPos.z}]\n. Location might be overlapping.");
            }
            else
            {
                // Reposition the block back to its previous location
                block.transform.position = previousPos;
                Debug.LogWarning($"BoardManager::TryMoveBlock::Cannot move block to [{(int)currentPos.x}, {yPos}, {(int)currentPos.z}]\n. Location might be overlapping.");
            }
        }
        catch (IndexOutOfRangeException)
        {
            block.transform.position = previousPos;
        }
    }

    private void BoardDataMoveBlock(Vector3 currentPos, Vector3 previousPos)
    {
        ScriptableBlock currentBlockData = boardData[(int)previousPos.x, (int)previousPos.y, (int)previousPos.z];
        boardData[(int)currentPos.x, (int)currentPos.y, (int)currentPos.z] = currentBlockData;
        // Remove old positioning
        boardData[(int)previousPos.x, (int)previousPos.y, (int)previousPos.z] = null;
    }

    private bool IsValid(Vector3 position, bool checkIsOverlap = true)
    {
        int coordX = (int)position.x;
        int coordY = Mathf.RoundToInt(position.y);
        int coordZ = (int)position.z;

        bool isInBound = coordX < boardData.GetLength(0) && coordY < boardData.GetLength(1) && coordZ < boardData.GetLength(2);
        return checkIsOverlap ? isInBound && !IsOverlap(coordX, coordY, coordZ) : isInBound;
    }

    private bool IsOverlap(int x, int y, int z)
    {
        return boardData[x, y, z] != null;
    }

    void PrintBoard()
    {
        for (int i = 0; i < boardData.GetLength(0); i++)
        {
            for (int j = 0; j < boardData.GetLength(1); j++)
            {
                for (int k = 0; k < boardData.GetLength(2); k++)
                {
                    var go = boardData[i, j, k];
                    var info = go != null ? go.ToString() : "NULL";
                    if (go != null)
                    {
                        Debug.Log($"Position at [{i}, {j}, {k}] is " + info);
                    }
                }
            }
        }
    }

}
