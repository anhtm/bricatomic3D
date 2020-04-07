using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset = new Vector3();
    private Vector3 blockPreviousPos;
    private Color startColour;
    private Color highlightColour = Color.white;

    private void Start()
    {
        startColour = GetComponent<Renderer>().material.color;
    }

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
                //return new Vector3(screenPoint.x, screenPoint.x, transform.forward * Input.mousePosition.y);
            case Axis.None:
            default:
                return new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        }
    }

    protected void OnMouseDown()
    {
        if (ModeManager.Instance.currentMode != BlockAction.Move) return;
        blockPreviousPos = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 cursorScreenPoint = GetPositionWithLockAxis();
        offset = transform.position - Camera.main.ScreenToWorldPoint(cursorScreenPoint);
    }

    protected void OnMouseDrag()
    {
        if (ModeManager.Instance.currentMode != BlockAction.Move) return;
        Vector3 cursorScreenPoint = GetPositionWithLockAxis();
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        transform.position = cursorPosition;
    }

    protected void OnMouseUp()
    {
        if (ModeManager.Instance.currentMode != BlockAction.Move) return;
        Vector3 nearestPoint = FindObjectOfType<GridTemplate>().GetNearestPointOnGrid(gameObject.transform.position);
        gameObject.transform.position = new Vector3(nearestPoint.x, nearestPoint.y, nearestPoint.z);
        BoardManager.Instance.TryMoveBlock(gameObject, blockPreviousPos);
    }

    void OnMouseEnter()
    {
        GetComponent<Renderer>().material.color = highlightColour;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = startColour;
    }
}
