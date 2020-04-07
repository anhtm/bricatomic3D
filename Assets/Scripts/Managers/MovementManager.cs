using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public Axis axisLock;

    private void Update()
    {
        if (ModeManager.Instance.currentMode != BlockAction.Move) return;

        if (Input.GetKeyDown(KeyCode.X))
        {
            axisLock = Axis.X;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            axisLock = Axis.Y;
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            axisLock = Axis.Z;
        }
    }
}
