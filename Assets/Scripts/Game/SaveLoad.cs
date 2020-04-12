using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/// <summary>
/// Utility methods to Save and Load game
/// </summary>
public static class SaveLoad {

	public static List<Game> savedGames = new List<Game>();
    private static readonly string fileLocation = Application.persistentDataPath + "/savedGames.gd";

	// Set to static so it can be called from anywhere
	public static void Save() {
        if (!savedGames.Contains(Game.current))
        {
			savedGames.Add(Game.current);
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(fileLocation);
		bf.Serialize(file, savedGames);
		file.Close();
	}	
	
	public static void Load() {
		if (File.Exists(fileLocation)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(fileLocation, FileMode.Open);
			savedGames = (List<Game>) bf.Deserialize(file);
			file.Close();
		}
	}
}
