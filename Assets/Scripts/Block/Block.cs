using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Block : MonoBehaviour
{
    [SerializeField] private BlockType type;

    internal Vector3 position;

    internal Vector3 scale;

    internal float DEFAULT_UNIT = 1.0f;

    internal GameObject blockObject;

    public Block() { }

    public Block(BlockType type, Vector3 position)
    {
        this.type = type;
        this.position = position;
        this.scale = GetScaleFromType();
    }

    private Vector3 GetScaleFromType()
    {
        switch (type)
        {
            case BlockType.OnexTwo:
                return new Vector3(DEFAULT_UNIT, DEFAULT_UNIT, DEFAULT_UNIT * 2);
            //case BlockType.OnexOne:
            default:
                return new Vector3(DEFAULT_UNIT, DEFAULT_UNIT, DEFAULT_UNIT);
        }
    }

}
