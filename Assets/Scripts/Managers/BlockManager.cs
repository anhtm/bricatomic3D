﻿using System.Collections;
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
    [SerializeField] public List<BlockSlot> BlookLookUpTable;


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

    public void CreateBlockFrom(BlockType type)
    {
       
    }
}
