using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Dropdown dropdown;

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
        dropdown.onValueChanged.AddListener(delegate {
            myDropdownValueChangedHandler(dropdown);
        });
    }

    void Destroy()
    {
        dropdown.onValueChanged.RemoveAllListeners();
    }

    private void myDropdownValueChangedHandler(Dropdown target)
    {
        ModeManager.Instance.UpdateMode(target.value);
    }

}
