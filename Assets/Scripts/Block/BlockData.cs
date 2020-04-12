using System;
using UnityEngine;

/// <summary>
/// Wrapper class for ScriptableBlock.
/// Use this format to serialize saved data
/// </summary>
[Serializable]
public class BlockData
{
    // String representation of a ScriptableBlock
    public string metaData;
    // Block's position on board. Use array because Unity does not support Vector3 serialization
    public float[] position;

    public BlockData(ScriptableBlock metaData, Vector3 position)
    {
        this.metaData = JsonifyData(metaData);
        this.position = SetPositionDataFrom(position);
    }

    /// <summary>
    /// Convert ScritableBlock to JSON
    /// </summary>
    /// <param name="scriptableBlock">a scriptable block in BoardManager.Instance.boardData</param>
    public string JsonifyData(ScriptableBlock scriptableBlock)
    {
        return JsonUtility.ToJson(scriptableBlock);
    }

    /// <summary>
    /// Convert JSON to scriptable block
    /// </summary>
    /// <param name="block">A ScriptableObject where JSON data is overridden to</param>
    public void JsonParseData(ScriptableBlock block)
    {
        JsonUtility.FromJsonOverwrite(metaData, block);
    }

    /// <summary>
    /// Convert a Vector3 position to float[]
    /// </summary>
    /// <param name="position">Block's position on board, also its index in BoardManager.Instance.boardData</param>
    public float[] SetPositionDataFrom(Vector3 position)
    {
        return new float[3] { position.x, position.y, position.z };
    }

    /// <summary>
    /// Convert position data from float[] to Vector3
    /// </summary>
    /// <returns></returns>
    public Vector3 ConvertPositionData()
    {
        return new Vector3(position[0], position[1], position[2]);
    }
}