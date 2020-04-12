using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Manages block add/move/deletion in board
/// Manages load and save board data
/// </summary>
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
        SetBoardSize();
    }

    void Start()
    {
        InitBoard();
    }

    /// <summary>
    /// Set board size based on user's preference
    /// </summary>
    void SetBoardSize()
    {
        if (boardObj == null || Game.current == null) { return; }
        float userChosenBoardSize = (float) Math.Round(Game.current.boardSize);
        boardObj.transform.localScale = new Vector3(userChosenBoardSize, 0, userChosenBoardSize);
    }

    /// <summary>
    /// Init boardData as 3D array, spawn blocks if eligible (when the game is previously saved)
    /// </summary>
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

    /// <summary>
    /// Save board data to Game.current.boardDataJSON
    /// </summary>
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

    /// <summary>
    /// Convert boardDataJson to usable ScriptableBlock format and spawn blocks with their position
    /// </summary>
    /// <param name="boardDataJson"></param>
    public void LoadBoardDataAndSpawn(ArrayList boardDataJson)
    {
        foreach (BlockData bd in boardDataJson)
        {
            Vector3 position = bd.ConvertPositionData();
            ScriptableBlock block = ScriptableObject.CreateInstance<ScriptableBlock>();
            bd.JsonParseData(block);
            boardData[(int)position.x, (int)position.y, (int)position.z] = block;
            BlockManager.Instance.InitBlock(block, position);
        }
    }

    /// <summary>
    /// Add new ScriptableBlock metaData at index that matches block's position
    /// </summary>
    /// <param name="metaData">ScriptableBlock to be stored</param>
    /// <param name="block">A GameObject created by BlockManager</param>
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

    /// <summary>
    /// Destroy the selected block and remove its metaData from boardData
    /// </summary>
    /// <param name="block">the GameObject to be removed</param>
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

    /// <summary>
    /// Move block from one position to another
    /// </summary>
    /// <param name="block">the GameObject that has been moved</param>
    /// <param name="previousPos">block's previous position - keep track of its previous position in case new position is invalid</param>
    public void TryMoveBlock(GameObject block, Vector3 previousPos)
    {
        Vector3 currentPos = block.transform.position;
        int yPos = Mathf.RoundToInt(currentPos.y);

        try
        {
            if (IsValid(currentPos))
            {
                MoveTo(currentPos, previousPos);
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

    private void MoveTo(Vector3 currentPos, Vector3 previousPos)
    {
        ScriptableBlock currentBlockData = boardData[(int)previousPos.x, (int)previousPos.y, (int)previousPos.z];
        boardData[(int)currentPos.x, (int)currentPos.y, (int)currentPos.z] = currentBlockData;
        // Remove old position from board
        boardData[(int)previousPos.x, (int)previousPos.y, (int)previousPos.z] = null;
    }

    /// <summary>
    /// Check if position is valid on board
    /// </summary>
    /// <param name="position">position to check</param>
    /// <param name="checkIsOverlap">If true, this method will check if position already exists in board</param>
    /// <returns></returns>
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
                    var data = boardData[i, j, k];
                    string strData = data != null ? data.ToString() : "NULL";
                    if (data != null)
                    {
                        Debug.Log($"Position at [{i}, {j}, {k}] is " + strData);
                    }
                }
            }
        }
    }

}
