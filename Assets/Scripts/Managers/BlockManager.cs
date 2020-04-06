using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages block creation/deletion with different BlockTypes
/// </summary>
public class BlockManager : MonoBehaviour
{
    internal ScriptableBlock currentBlock;
    
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

    public GameObject InitBlock(ScriptableBlock block, Vector3 position)
    {
        return Instantiate(block.prefab, position, Quaternion.identity);
    }

    void Update()
    {
        bool isLeftClicked = Input.GetMouseButtonDown(0);
        BlockAction mode = ModeManager.Instance.currentMode;

        if (isLeftClicked)
        {
            // Check if the mouse was clicked over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out hitInfo);

            if (isHit && mode == BlockAction.Add && currentBlock.prefab != null)
            {
                PlaceBlockNear(hitInfo.point);
            }
            else if (isHit && mode == BlockAction.Delete)
            {
                RemoveBlock(hitInfo);
            } 
        }   
    }

    void PlaceBlockNear(Vector3 position)
    {
        Vector3 nearestPoint = FindObjectOfType<GridTemplate>().GetNearestPointOnGrid(position);
        GameObject block = InitBlock(currentBlock, nearestPoint);
        BoardManager.Instance.TryAddBlock(currentBlock, block);
    }

    void RemoveBlock(RaycastHit hitInfo)
    {
        GameObject hitObject = hitInfo.collider.gameObject;
        if (hitObject.tag == "block")
        {
            BoardManager.Instance.TryRemoveBlock(hitObject);
        }
    }
}
