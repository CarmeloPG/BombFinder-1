using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RouletteItem : MonoBehaviour {
    #region Readonly fields
    #endregion

    #region Serialize fields
    [SerializeField]
    private int reward;
    [SerializeField]
    private TextMeshProUGUI rewardText;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    public int Reward {
        get => reward;
    }
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    private void Awake() {
        rewardText.text = "X " + reward.ToString();
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
