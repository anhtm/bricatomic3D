using UnityEngine;


public class Block : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset = new Vector3();
    private Vector3 blockPreviousPos;

    private bool isModel()
    {
        return gameObject.tag == "model";
    }

    protected void OnMouseDown()
    {
        if (isModel())
        {
            CreateObjectOfCurrentType();
        } else
        {
            blockPreviousPos = transform.position;
        }
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = transform.position - Camera.main.ScreenToWorldPoint(cursorScreenPoint);
    }

    void CreateObjectOfCurrentType()
    {
        Instantiate(gameObject, transform.position, Quaternion.identity);
        // untag so it's no longer a model
        gameObject.tag = "Untagged"; 
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
        Debug.Log("nearest point: " + nearestPoint);
        gameObject.transform.position = new Vector3(nearestPoint.x, nearestPoint.y, nearestPoint.z);

        if (isModel())
        {
            BoardManager.Instance.TryAddBlock(gameObject);
        } else
        {
            BoardManager.Instance.TryMoveBlock(gameObject, blockPreviousPos);
        }
    }

    protected void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && !isModel())
        {
            // TODO: Add popup Action menu so player can choose to "Remove" the block
            BoardManager.Instance.TryRemoveBlock(gameObject);
        }
    }

}
