using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("BlockCreationPanel")]
    [SerializeField] Button CreateBtn;

    [Header("BlockContextPanel")]
    [SerializeField] GameObject BlockContextPanel;
    [SerializeField] public Button DeleteBtn;

    GameObject blockCaller;

    #region Singleton
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }
    #endregion

    private void Start()
    {
        CreateBtn.onClick.AddListener(CreateDefaultBlock);
        DeleteBtn.onClick.AddListener(DeleteBlock);
    }

    private void CreateDefaultBlock()
    {
        Vector3 defaultPosition = new Vector3(BoardManager.Instance.origins.x, 0, BoardManager.Instance.origins.z);
        GameObject newBlock = BlockManager.Instance.InitBlock(BlockType.OnexOne, defaultPosition);
        BoardManager.Instance.TryAddBlock(newBlock);
    }

    private void DeleteBlock()
    {
        BoardManager.Instance.TryRemoveBlock(blockCaller);
        blockCaller = null;
    }

    public void ToggleBlockContextPanel(GameObject blockCaller)
    {
        this.blockCaller = blockCaller;
        BlockContextPanel.SetActive(!BlockContextPanel.activeInHierarchy);
    }
}
