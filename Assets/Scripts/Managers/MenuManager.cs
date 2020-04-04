using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public enum Menu
	{
		MainMenu,
		NewGame,
		Continue,
        InGameMenu,
        None
	}

	public Menu currentMenu;

	public Font font;

	#region Singleton
	private static MenuManager _instance;

	public static MenuManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<MenuManager>();
			}
			return _instance;
		}
	}
	#endregion

    void SetStyle()
    {
		GUI.skin.font = font;
	}

	void OnGUI()
	{
		SetStyle();

		Rect uiLayout = new Rect(0, 0, Screen.width, Screen.height);
		GUILayout.BeginArea(uiLayout);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.BeginVertical();
		GUILayout.FlexibleSpace();

		switch (currentMenu)
        {
			case Menu.MainMenu:
				SetUpMainMenu();
				break;
			case Menu.NewGame:
				SetUpNewGameMenu();
				break;
			case Menu.Continue:
				SetupContinueMenu();
				break;
			case Menu.InGameMenu:
				SetupInGameMenu();
				break;
			case Menu.None:
				break;
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}


    void SetUpMainMenu()
    {
		if (GUILayout.Button("New Game"))
		{
			Game.current = new Game();
			currentMenu = Menu.NewGame;
		}

		GUILayout.Space(10);


		if (GUILayout.Button("Continue"))
		{
			SaveLoad.Load();
			currentMenu = Menu.Continue;
		}

		GUILayout.Space(10);

		if (GUILayout.Button("Quit"))
		{
			Application.Quit();
		}
	}

    void SetUpNewGameMenu()
    {
		GUILayout.Box("Game Settings");
		GUILayout.Space(10);

		if (Game.current != null)
		{
			GUILayout.Label("Name");
			Game.current.name = GUILayout.TextField(Game.current.name, 20);

			GUILayout.Label("Board Size");
			Game.current.boardSize = GUILayout.HorizontalSlider(Game.current.boardSize, 1.0f, 10.0f);
		}

		if (GUILayout.Button("Save"))
		{
			//Save the current Game as a new saved Game
			SaveLoad.Save();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

		GUILayout.Space(10);

		if (GUILayout.Button("Cancel"))
		{
			currentMenu = Menu.MainMenu;
		}
	}

	void SetupContinueMenu()
	{
		GUILayout.Box("Select Save File");
		GUILayout.Space(10);

		foreach (Game game in SaveLoad.savedGames)
		{
			if (GUILayout.Button(game.name))
			{
				Game.current = game;
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}

		GUILayout.Space(10);

		if (GUILayout.Button("Cancel"))
		{
			currentMenu = Menu.MainMenu;
		}
	}

	void SetupInGameMenu()
	{
		float windowWidthOffset = Screen.width / 4;
		float windowHeightOffset = Screen.height / 4;
		Rect windowRect = new Rect(windowWidthOffset, windowHeightOffset, windowWidthOffset * 2, windowHeightOffset * 2);

        GUILayout.Window(0, windowRect, SetUpOverlay, "Settings");
    }

    void SetUpOverlay(int windowId)
    {
		if (GUILayout.Button("Save & Quit"))
		{
			BoardManager.Instance.SaveBoard();
			SaveLoad.Save();
			// Back to main menu scene
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}

		if (GUILayout.Button("Cancel"))
		{
			currentMenu = Menu.None;
		}
	}

}
