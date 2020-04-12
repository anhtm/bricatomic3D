using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages item selection in inventory panel
/// </summary>
public class SlotActions : MonoBehaviour
{
    Button button;

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
