using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class ProgressBar : MonoBehaviour
{
    #region Serialize fields
    [SerializeField]
    private Slider progressSlider;
    [SerializeField]
    private TextMeshProUGUI maxProgressText;
    [SerializeField]
    private TextMeshProUGUI currentProgressText;
    #endregion

    #region Properties
    public float MaxProgress {
        set {
            progressSlider.maxValue = value;
            maxProgressText.text = value.ToString("P0");
        }
    }

    public float MinProgress {
        set {
            progressSlider.minValue = value;
        }
    }

    public float CurrentProgress {
        set {
            progressSlider.value = value;
            currentProgressText.text = value.ToString("P0");
        }
    }
    #endregion

    #region Events
    public event Action<float> OnProgressChange = delegate { };
    #endregion

    #region Unity methods
    private void Awake() {
        progressSlider.value = progressSlider.minValue;
        maxProgressText.text = progressSlider.maxValue.ToString("P0");
        currentProgressText.text = progressSlider.minValue.ToString("P0");
    }
    #endregion

    #region Public methods
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}
