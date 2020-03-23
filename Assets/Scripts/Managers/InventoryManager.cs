using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// A component of Inventory Panel
public class InventoryManager : MonoBehaviour
{

    public List<ScriptableBlock> blocks;

    int size = 9;

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

    public GameObject GetPrefabFrom(string name)
    {
        ScriptableBlock block = blocks.Find(x => x.name == name);
        return block.prefab;
    }

}
