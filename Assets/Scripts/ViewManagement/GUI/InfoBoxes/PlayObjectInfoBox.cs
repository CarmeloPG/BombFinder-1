using UnityEngine;
using UnityEngine.UI;
using I2.Loc;

/// <summary>
/// A UI element witch shows the information about a PlayObject.
/// </summary>
public class PlayObjectInfoBox : MonoBehaviour {

    #region Readonly fileds
    #endregion

    #region Serialize fields
    [SerializeField]
    private Localize identifierText;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Sprite defaultIconSprite;
    [SerializeField]
    private Localize descriptionText;
    [SerializeField]
    private MoneyInfoBox[] moneyInfoBoxes;
    #endregion

    #region Private fields
    private PlayObjectData playObjectData;
    #endregion

    #region Properties
    public PlayObjectData Data {
        get => playObjectData;
    }
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    public void ShowInfo(PlayObjectData data) {
        MoneyType moneyType;
        int amount;

        playObjectData = data;
        
        identifierText.Term = data.Identifier;
        descriptionText.Term = data.Description;
        iconImage.sprite = data.Icon;

        foreach (var moneyInfoBox in moneyInfoBoxes) {
            moneyType = moneyInfoBox.MoneyType;
            amount = data.GetPrice(moneyType);

            if (amount >= 0) {
                moneyInfoBox.ShowMoneyAmount(moneyType, amount);
                moneyInfoBox.gameObject.SetActive(true);
            }
            else {
                moneyInfoBox.gameObject.SetActive(false);
            }
        }
    }

    public void ClearInfo() {
        playObjectData = null;

        identifierText.Term = "NoneSelected";//string.Empty;
        descriptionText.Term = "NoneSelected";//string.Empty;
        iconImage.sprite = defaultIconSprite;

        foreach (var moneyInfoBox in moneyInfoBoxes) {
            moneyInfoBox.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}