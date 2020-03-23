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
        GameObject prefab = GetPrefab();
        Debug.Log("Prefab chosen is " + prefab.name);
        BlockManager.Instance.currentPrefab = prefab;
    }

    GameObject GetPrefab()
    {
        string childSprite = transform.GetChild(0).gameObject.GetComponent<Image>().sprite.name;
        return InventoryManager.Instance.GetPrefabFrom(childSprite);
    }
}
