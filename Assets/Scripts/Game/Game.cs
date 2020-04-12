using System.Collections;

/// <summary>
/// Presentation of a game - used to save/load game
/// </summary>
[System.Serializable]
public class Game { //don't need ": Monobehaviour" because we are not attaching it to a game object

	public static Game current;
	public int id = SaveLoad.savedGames.Count + 1;
	public string name;
	public float boardSize;
    // Representation of blocks' position on the game's board
	public ArrayList boardDataJSON;

	public Game () {
		name = "New Game " + id;
		boardSize = 3.0f;
	}
		
}
