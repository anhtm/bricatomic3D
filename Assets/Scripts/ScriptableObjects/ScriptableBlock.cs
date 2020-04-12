using UnityEngine;

[CreateAssetMenu(fileName = "New Block Object", menuName = "Block")]
[System.Serializable]
public class ScriptableBlock : ScriptableObject
{
    public Sprite sprite;
    public GameObject prefab;
    public new string name;
    public int id;
}
