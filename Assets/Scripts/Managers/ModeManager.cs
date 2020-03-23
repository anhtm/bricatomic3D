using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    public BlockAction currentMode = BlockAction.Add;

    [SerializeField] List<Texture2D> cursorTextures = new List<Texture2D>(3);

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

    // Start is called before the first frame update
    void Start()
    {
        SetCursor();
    }

    void PopulateCursorMapper()
    {   
        for (var i = 0; i < cursorTextures.Count; i++)
        {
            cursorMapper.Add((BlockAction)i, cursorTextures[i]);
            Debug.Log(cursorMapper[(BlockAction)i]);
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
