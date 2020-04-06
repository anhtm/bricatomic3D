using System;
using UnityEngine;

/// <summary>
/// Wrapper class for ScriptableBlock
/// </summary>
[Serializable]
public class BlockData
{
    public string metaData;
    public float[] position;

    public BlockData(ScriptableBlock metaData, Vector3 position)
    {
        this.metaData = jsonifyData(metaData);
        this.position = setPositionDataFrom(position);
    }

    public string jsonifyData(ScriptableBlock scriptableBlock)
    {
        return JsonUtility.ToJson(scriptableBlock);
    }

    public void jsonParseData(ScriptableBlock block)
    {
        JsonUtility.FromJsonOverwrite(metaData, block);
    }

    public float[] setPositionDataFrom(Vector3 position)
    {
        return new float[3] { position.x, position.y, position.z };
    }

    public Vector3 convertPositionData()
    {
        return new Vector3(position[0], position[1], position[2]);
    }
}