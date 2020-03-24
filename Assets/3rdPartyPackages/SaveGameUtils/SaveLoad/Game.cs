using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game { //don't need ": Monobehaviour" because we are not attaching it to a game object

	public static Game current;
	public string name;
    public GameObject[,,] board;
	public float boardSize;

	public Game () {
		name = "New Game";
		boardSize = 3.0f;
	}
		
}
