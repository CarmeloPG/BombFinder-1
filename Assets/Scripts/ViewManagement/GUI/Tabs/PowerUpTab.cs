using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerUpTab : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    private GameStore playerInventory;
    [SerializeField]
    private MoneyInfoBox playerMoneyInfoBox;

    [Header("Power up 1 options")]
	[SerializeField]
	private Money powerUpPrice;
    [SerializeField]
    private Button powerUpbutton;
    [SerializeField]
    private MoneyInfoBox powerUpMoneyInfoBox;
    [SerializeField]
    private GameObject powerUpBoughtObject;

    /*[Header("Heavy ball options")]
    [SerializeField]
	private int heavyBallPrice = 1000;
    [SerializeField]
    private Button heavyBallButton;
    [SerializeField]
    private MoneyInfoBox heavyBallMoneyInfoBox;
    [SerializeField]
    private GameObject heavyBallBoughtObject;*/
    #endregion

    #region Private fields
    private bool powerUpBougth = false;
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    private void OnEnable() {
        int playerMoney = playerInventory.GetMoneyQuantity(MoneyType.GameMoney);

        playerMoneyInfoBox.ShowMoneyAmount(MoneyType.GameMoney, playerMoney);

        powerUpbutton.interactable = 
            playerMoney >= powerUpPrice.Amount && !powerUpBougth;

        powerUpMoneyInfoBox.ShowMoneyAmount(
            MoneyType.GameMoney, powerUpPrice.Amount);
    }

    public void Reset() {
        powerUpBougth = false;

        powerUpBoughtObject.SetActive(false);
    }
    #endregion

    #region Public methods
    public void BuyPowerUp() {
        powerUpBougth = true;

        powerUpBoughtObject.SetActive(true);
        playerMoneyInfoBox.ShowMoneyAmount(
            MoneyType.GameMoney, 
            playerInventory.SubstractMoney(
                powerUpPrice.MoneyType, 
                powerUpPrice.Amount));
    }
    
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}