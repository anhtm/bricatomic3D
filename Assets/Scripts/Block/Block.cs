using UnityEngine;

public class Block : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset = new Vector3();
    private Vector3 blockPreviousPos;

    protected void OnMouseDown()
    {
        blockPreviousPos = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = transform.position - Camera.main.ScreenToWorldPoint(cursorScreenPoint);
    }

    protected void OnMouseDrag()
    {
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        transform.position = cursorPosition;
    }

    protected void OnMouseUp()
    {
        Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(gameObject.transform.position);
        gameObject.transform.position = new Vector3(nearestPoint.x, nearestPoint.y, nearestPoint.z);
        BoardManager.Instance.TryMoveBlock(gameObject, blockPreviousPos);
    }
}
