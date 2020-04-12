using UnityEngine;

/// <summary>
/// Perform movement on a single block and change highlight of block on hover
/// </summary>
public class Block : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset = new Vector3();
    private Vector3 blockPreviousPos;
    private Color startColour;
    private Color highlightColour = Color.white;

    /// <summary>
    /// Lock movement on X or Y axis when press X or Y
    /// </summary>
    /// <returns></returns>
    private Vector3 GetPositionWithLockAxis()
    {
        Axis currentAxisLock = FindObjectOfType<MovementManager>().axisLock;
        switch (currentAxisLock)
        {
            case Axis.X:
                return new Vector3(Input.mousePosition.x, screenPoint.y, screenPoint.z);
            case Axis.Y:
                return new Vector3(screenPoint.x, Input.mousePosition.y, screenPoint.z);
            case Axis.Z:
            case Axis.None:
            default:
                return new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        }
    }

    bool isMoving()
    {
        return ModeManager.Instance.currentMode == BlockAction.Move;
    }

    /// <summary>
    /// Register cursor's offset position from block's position
    /// </summary>
    protected void OnMouseDown()
    {
        if (!isMoving()) return;
        blockPreviousPos = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 cursorScreenPoint = GetPositionWithLockAxis();
        offset = transform.position - Camera.main.ScreenToWorldPoint(cursorScreenPoint);
    }

    /// <summary>
    /// Move block's position along cursor's path
    /// </summary>
    protected void OnMouseDrag()
    {
        if (!isMoving()) return;
        Vector3 cursorScreenPoint = GetPositionWithLockAxis();
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        transform.position = cursorPosition;
    }

    /// <summary>
    /// Set block's position where cursor's path ends. Move block to that position and register to Board
    /// </summary>
    protected void OnMouseUp()
    {
        if (!isMoving()) return;
        Vector3 nearestPoint = FindObjectOfType<GridTemplate>().GetNearestPointOnGrid(gameObject.transform.position);
        gameObject.transform.position = new Vector3(nearestPoint.x, nearestPoint.y, nearestPoint.z);
        BoardManager.Instance.TryMoveBlock(gameObject, blockPreviousPos);
    }

    # region Highlight on hover
    private void Start()
    {
        startColour = GetComponent<Renderer>().material.color; // Register startColour for reference
    } 

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = highlightColour; // Set block's colour to be hightlight colour
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = startColour; // Return to original colour
    }
    #endregion
}
