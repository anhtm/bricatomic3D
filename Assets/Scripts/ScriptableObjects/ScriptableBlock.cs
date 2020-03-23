using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block Object", menuName = "Block")]
public class ScriptableBlock : ScriptableObject
{
    public Sprite sprite;
    public GameObject prefab;
    public string name;
    public int id;
}
