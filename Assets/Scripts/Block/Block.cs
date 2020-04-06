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

    protected void OnMouseDown()
    {
        if (ModeManager.Instance.currentMode != BlockAction.Move) return;
        blockPreviousPos = transform.position;
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = transform.position - Camera.main.ScreenToWorldPoint(cursorScreenPoint);
    }

    protected void OnMouseDrag()
    {
        if (ModeManager.Instance.currentMode != BlockAction.Move) return;
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        transform.position = cursorPosition;
    }

    protected void OnMouseUp()
    {
        if (ModeManager.Instance.currentMode != BlockAction.Move) return;
        Vector3 nearestPoint = FindObjectOfType<GridTemplate>().GetNearestPointOnGrid(gameObject.transform.position);
        gameObject.transform.position = new Vector3(nearestPoint.x, Mathf.Round(nearestPoint.y), nearestPoint.z);
        ScriptableBlock metaData = InventoryManager.Instance.GetBlockBy(gameObject);
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
