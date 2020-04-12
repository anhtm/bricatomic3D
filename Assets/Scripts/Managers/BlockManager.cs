using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages block creation/deletion with mouse raycast
/// </summary>
public class BlockManager : MonoBehaviour
{
    // Current type of block chosen from block selection bar
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
        // Only perform move/delete block with left mouse click
        bool isLeftClicked = Input.GetMouseButtonDown(0);
        BlockAction mode = ModeManager.Instance.currentMode;

        if (isLeftClicked)
        {
            // Check if the mouse was clicked over a UI element. If true, skip raycast
            if (EventSystem.current.IsPointerOverGameObject()) { return; }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out RaycastHit hitInfo);

            if (isHit && mode == BlockAction.Add && currentBlock != null)
            {
                PlaceBlockNear(hitInfo.point);
            }
            else if (isHit && mode == BlockAction.Delete)
            {
                RemoveBlock(hitInfo);
            } 
        }   
    }

    /// <summary>
    /// Place a new block correctly on the grid based on mouse position
    /// </summary>
    /// <param name="position">Mouse click position</param>
    void PlaceBlockNear(Vector3 position)
    {
        Vector3 nearestPoint = FindObjectOfType<GridTemplate>().GetNearestPointOnGrid(position);
        GameObject block = InitBlock(currentBlock, nearestPoint);
        BoardManager.Instance.TryAddBlock(currentBlock, block);
    }

    /// <summary>
    /// Remove the block that mouse's raycast hits
    /// </summary>
    /// <param name="hitInfo">Hit result from mouse</param>
    void RemoveBlock(RaycastHit hitInfo)
    {
        GameObject hitObject = hitInfo.collider.gameObject;
        if (hitObject.CompareTag("block"))
        {
            BoardManager.Instance.TryRemoveBlock(hitObject);
        }
    }
}
