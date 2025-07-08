using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedJoystick : Joystick
{
    #region Private fields
    private bool run;
    #endregion

    #region Properties
    public bool Run {
        get => run;
        set => run = value;
    }
    #endregion
}