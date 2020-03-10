using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelBlock : Block
{
    new void OnMouseDown()
    {
        CreateObjectOfCurrentType();
        base.OnMouseDown();
    }

    void CreateObjectOfCurrentType()
    {
        Instantiate(gameObject, transform.position, Quaternion.identity);
        gameObject.tag = "Untagged"; // untag so it's no longer a model
    }

    new void OnMouseUp()
    {
        base.OnMouseUp();
        BoardManager.Instance.TryAddBlock(gameObject);
    }
}
