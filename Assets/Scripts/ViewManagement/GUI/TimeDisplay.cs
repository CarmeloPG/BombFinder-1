using UnityEngine.UI;
using UnityEngine;
using System;

public class TimeDisplay : MonoBehaviour {
    #region Fields
#pragma warning disable 0649
    [SerializeField]
    private Text timeText;
#pragma warning restore 0649
    #endregion

    #region Events
    public event Action<float> OnTimeChange = delegate { };
    #endregion

    #region Public methods
    public void ShowTime(int seconds) {
        int
            hours = seconds / 3600,
            restSeconds = seconds % 3600;

        timeText.text =
            string.Format("{0:00}:{1:00}:{2:00}",
                hours, restSeconds / 60, restSeconds % 60);

        OnTimeChange(seconds);
    }

    public void ShowTime(float value) {
        ShowTime((int)value);
    }
    #endregion
}
