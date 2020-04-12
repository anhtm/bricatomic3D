using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages populating and finding block types in inventory panel
/// </summary>
public class InventoryManager : MonoBehaviour
{

    public List<ScriptableBlock> blocks;

    int size;

    #region Singleton
    private static InventoryManager _instance;

    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryManager>();
            }
            return _instance;
        }
    }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        size = transform.childCount;
        PopulateInventoryUI();
    }

    /// <summary>
    /// Automatically populate sprite images of inventory panel
    /// </summary>
    private void PopulateInventoryUI()
    {
        for (int i = 0; i < size; i++)
        {
            Transform slot = transform.GetChild(i);
            GameObject img = slot.GetChild(0).GetChild(0).gameObject;
            Image imgSprite = img.GetComponent<Image>();

            if (imgSprite)
            {
                imgSprite.sprite = blocks[i].sprite;
            }
        }
    }

    /// <summary>
    /// Find a ScriptableBlock by its name
    /// </summary>
    public ScriptableBlock GetBlockBy(string name)
    {
        ScriptableBlock block = blocks.Find(x => x.name == name);
        return block;
    }
}
