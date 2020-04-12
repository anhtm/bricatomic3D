using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages Menu screens with their behaviour and style
/// </summary>
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

	public int defaultFontSize = 36;

	private GUILayoutOption[] itemLayoutOptions = new GUILayoutOption[] {
			 GUILayout.MinWidth(Screen.width / 5),
			 GUILayout.MaxWidth(Screen.width / 4),
		};

	private GUIStyle labelStyle;
	private GUIStyle headerStyle;

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
		GUI.skin.button.fontSize = defaultFontSize;
		GUI.skin.window.fontSize = defaultFontSize;
		labelStyle = new GUIStyle(GUI.skin.label)
		{
			fontSize = 36,
			normal =
			{
				textColor = Color.black,
			}
		};
		headerStyle = new GUIStyle(labelStyle)
		{
			fontStyle = FontStyle.Bold,
			alignment = TextAnchor.MiddleCenter,
		};
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

        if (GUILayout.Button("New Game", itemLayoutOptions))
		{
			Game.current = new Game();
			currentMenu = Menu.NewGame;
		}

		GUILayout.Space(10);


		if (GUILayout.Button("Continue", itemLayoutOptions))
		{
			SaveLoad.Load();
			currentMenu = Menu.Continue;
		}

		GUILayout.Space(10);

		if (GUILayout.Button("Quit", itemLayoutOptions))
		{
			Application.Quit();
		}
	}

    void SetUpNewGameMenu()
    {
		GUILayout.Label("Game Settings", headerStyle, itemLayoutOptions);

		GUILayout.Space(10);

		if (Game.current != null)
		{
			GUILayout.Label("Name", labelStyle, itemLayoutOptions);
			Game.current.name = GUILayout.TextField(Game.current.name, 20, new GUIStyle(GUI.skin.textField) {
                fontSize = defaultFontSize,
            }, itemLayoutOptions);

			GUILayout.Label("Board Size", labelStyle, itemLayoutOptions);
			Game.current.boardSize = GUILayout.HorizontalSlider(Game.current.boardSize, 1.0f, 10.0f,
            new GUIStyle(GUI.skin.horizontalSlider) {
                stretchHeight = true,
                fixedHeight = 20,
            },
            new GUIStyle(GUI.skin.horizontalSliderThumb) {
                fixedHeight = 20,
                fixedWidth = 20,
            },
            itemLayoutOptions);
		}

		GUILayout.Space(20);

		if (GUILayout.Button("Save", itemLayoutOptions))
		{
			//Save the current Game as a new saved Game
			SaveLoad.Save();
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

		GUILayout.Space(10);

		if (GUILayout.Button("Cancel", itemLayoutOptions))
		{
			currentMenu = Menu.MainMenu;
		}
	}

	void SetupContinueMenu()
	{
		GUILayout.Box("Select Save File", headerStyle, itemLayoutOptions);
		GUILayout.Space(20);

		foreach (Game game in SaveLoad.savedGames)
		{
			if (GUILayout.Button(game.name, itemLayoutOptions))
			{
				Game.current = game;
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}

		GUILayout.Space(20);

		if (GUILayout.Button("Cancel", itemLayoutOptions))
		{
			currentMenu = Menu.MainMenu;
		}
	}

	void SetupInGameMenu()
	{
		float windowWidthOffset = Screen.width / 4;
		float windowHeightOffset = Screen.height / 4;
		Rect windowRect = new Rect(windowWidthOffset, windowHeightOffset, windowWidthOffset * 2, windowHeightOffset);

		GUILayout.Window(0, windowRect, SetUpOverlay, "");
    }

    void SetUpOverlay(int windowId)
    {
		if (GUILayout.Button("Save & Return"))
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
