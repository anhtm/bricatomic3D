using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages block creation/deletion with different BlockTypes
/// </summary>
public class BlockManager : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    internal GameObject currentPrefab;
    
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

    public GameObject InitBlock(Vector3 position)
    {
        return Instantiate(currentPrefab, position, Quaternion.identity);
    }

    void Update()
    {
        bool isLeftClicked = Input.GetMouseButtonDown(0);
        BlockAction mode = ModeManager.Instance.currentMode;

        if (isLeftClicked)
        {
            RaycastHit hitInfo;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out hitInfo, 1000.0f, layerMask);

            if (isHit && mode == BlockAction.Add && currentPrefab != null)
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
        Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(position);
        GameObject block = InitBlock(nearestPoint);
        BoardManager.Instance.TryAddBlock(block);
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
