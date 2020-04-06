using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotActions : MonoBehaviour
{
    Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(SelectItem);
    }

    void SelectItem()
    {
        ScriptableBlock chosenBlock = GetBlock();
        BlockManager.Instance.currentBlock = chosenBlock;
    }

    ScriptableBlock GetBlock()
    {
        string childSprite = transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name;
        return InventoryManager.Instance.GetBlockBy(childSprite);
    }
}
