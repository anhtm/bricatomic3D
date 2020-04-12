using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages switching between modes
/// </summary>
public class ModeManager : MonoBehaviour
{
    public BlockAction currentMode = BlockAction.None;

    [SerializeField] List<Texture2D> cursorTextures = new List<Texture2D>(3);
    [SerializeField] bool lockMode = true;

    private Dictionary<BlockAction, Texture2D> cursorMapper = new Dictionary<BlockAction, Texture2D>();

    #region Singleton
    private static ModeManager _instance;

    public static ModeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ModeManager>();
            }
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        PopulateCursorMapper();
    }

    void Start()
    {
        SetCursor();
    }

    private void Update()
    {
        if (lockMode) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            UpdateMode((int)BlockAction.Add);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            UpdateMode((int)BlockAction.Delete);
        } else if (Input.GetKeyDown(KeyCode.E))
        {
            UpdateMode((int)BlockAction.Move);
        } else if (Input.GetKeyDown(KeyCode.Escape))
        {
            UpdateMode((int)BlockAction.None);
        }
    }

    void PopulateCursorMapper()
    {   
        for (var i = 0; i < cursorTextures.Count; i++)
        {
            cursorMapper.Add((BlockAction) i, cursorTextures[i]);
        }
    }

    public void UpdateMode(int newMode)
    {
        currentMode = (BlockAction)newMode;
        SetCursor();
    }

    void SetCursor()
    {
        Cursor.SetCursor(cursorMapper[currentMode], Vector3.zero, CursorMode.ForceSoftware);
    }

}
