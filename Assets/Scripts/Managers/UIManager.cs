using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Dropdown Dropdown;
    public Button InGameMenuTrigger;

    #region Singleton
    private static UIManager _instance;

    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }
    #endregion

    private void Start()
    {
        Dropdown.onValueChanged.AddListener(delegate {
            myDropdownValueChangedHandler(Dropdown);
        });

        InGameMenuTrigger.onClick.AddListener(ToggleInGameMenu);
    }

    void Destroy()
    {
        Dropdown.onValueChanged.RemoveAllListeners();
        InGameMenuTrigger.onClick.RemoveAllListeners();
    }

    private void myDropdownValueChangedHandler(Dropdown target)
    {
        ModeManager.Instance.UpdateMode(target.value);
    }

    private void ToggleInGameMenu()
    {
        if (MenuManager.Instance.currentMenu == MenuManager.Menu.None)
        {
            MenuManager.Instance.currentMenu = MenuManager.Menu.InGameMenu;
        } else
        {
            MenuManager.Instance.currentMenu = MenuManager.Menu.None;
        }
    }

}
