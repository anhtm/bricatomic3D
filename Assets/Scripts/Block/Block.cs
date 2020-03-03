using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset = new Vector3();

    void OnMouseDown()
    {
        CreateObjectOfCurrentType();
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        offset = transform.position - Camera.main.ScreenToWorldPoint(cursorScreenPoint);
    }

    void CreateObjectOfCurrentType()
    {
        if (gameObject.tag == "model")
        {
            Debug.Log("Created block from prototype");
            //Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(gameObject.transform.position);
            //BlockManager.Instance.InitBlock(gameObject, transform.position);
            Instantiate(gameObject, transform.position, Quaternion.identity);
            gameObject.tag = "Untagged"; // untag so it's no longer a model
        }
    }

    void OnMouseDrag()
    {
        Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + offset;
        transform.position = cursorPosition;
    }

    void OnMouseUp()
    {
        Vector3 nearestPoint = GridTemplate.Instance.GetNearestPointOnGrid(gameObject.transform.position);
        gameObject.transform.position = new Vector3(nearestPoint.x, nearestPoint.y + BlockManager.Instance.DEFAULT_UNIT / 2, nearestPoint.z);
        // TODO: Add newly created block to board manager
        // TODO: Check for its validity before create a block
        // if not valid, destroy gameobject
    }
}
