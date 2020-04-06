using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game { //don't need ": Monobehaviour" because we are not attaching it to a game object

	public static Game current;
	public int id = SaveLoad.savedGames.Count + 1;
	public string name;
    //public ScriptableBlock[,,] boardData;
    public ArrayList boardDataJSON;

	public float boardSize;

	public Game () {
		name = "New Game " + id;
		boardSize = 3.0f;
	}
		
}
