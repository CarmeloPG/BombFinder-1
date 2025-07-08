using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.ComponentModel.Design;

/// <summary>
/// In the shop the ball can sell and buy their PlayObjects. 
/// </summary>
public class Shop : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    [Tooltip("If true, the customer can only sell a PlayObject if the seller has got enough playerMoney. Otherwise, the customer can allways sell PlayObjects.")]
    private bool chargeToShop;
    [SerializeField]
    [Tooltip("It will play when a transaction is levelSuccess.")]
    private Animator shopAnimator;
    [SerializeField]
    [Tooltip("Shows the information of a PlayOject when it has being bought or sold.")]
    private PlayObjectInfoBox transactionObjectInfobox;
    [SerializeField]
    [Tooltip("Shows the price of a PlayOvject witch has being bought or sold.")]
    private TextMeshProUGUI transactionMoneyAmount;
    [SerializeField]
    [Tooltip("Shows the icon of the playerMoney type selected in a puchase or a sale.")]
    private Image transactionMoneyIcon;
    /*[SerializeField]
    [Tooltip("It will be shown when a transaction fails.")]
    private Window transactionErrorWindow;*/
    


    [Header("Stores")]
    [SerializeField]
    private GameStoreTab shopStoreTab;
    [SerializeField]
    private GameStoreTab playerStoreTab;
    [SerializeField]
    [Tooltip("Must hava a MoneyInfoBox component.")]
    private Button[] buyButttons;
    [SerializeField]
    [Tooltip("Must have a MoneyInfoBox component.")]
    private Button[] sellButtons;
    #endregion

    #region Private fields
    private PlayObjectData
        equipedObject,
        selectedData;
    private MoneyType selectedMoneyType;
    private GameStore
        playerStore,
        shopStore;
    #endregion

    #region Properties
    public GameStore ShopStore {
        set {
            shopStore = value;
            shopStoreTab.GameStore = value;
        }
    }

    public GameStore PlayerStore {
        set {
            playerStore = value;
            playerStoreTab.GameStore = value;
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When a PlayObject is bougth, it sends the data of the bought PlayObject.")]
    public UnityEvent<PlayObjectData> OnBuyPlayObject;
    [Tooltip("When a PlayObject is equiped, it sends the data of the equiped PlayObject.")]
    public UnityEvent<PlayObjectData> OnEquipPlayObject;
    [Tooltip("When a PlayObject of the shop store tab is selected, it sends the data of the selected PlayObject.")]
    public UnityEvent<PlayObjectData> OnSelectShopPlayObject;
    #endregion

    #region Unity methods
    /// <summary>
    /// 
    /// </summary>
    private void OnEnable() {
        ShowGameStoresData();

        if (equipedObject != null) {
            playerStoreTab.SelectShopToggle(equipedObject);
        }
        else {
            Debug.Log("No equiped object on enable.");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDisable() {
        shopStoreTab.ClearGameStoreData();
        playerStoreTab.ClearGameStoreData();
    }
    #endregion

    #region Public methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameStatus"></param>
    public void LoadShop(GameStatus gameStatus) {
        shopStore = shopStoreTab.GameStore;
        playerStore = playerStoreTab.GameStore;

        foreach (Button button in buyButttons) {
            button.interactable = false;
        }

        foreach (Button button in sellButtons) {
            button.interactable = false;
        }

        playerStoreTab.LoadStore();
        shopStoreTab.LoadStore();

        SelectPlayerPlayObject(
            playerStore.GetObjectData(gameStatus.equipedObject));

        EquipSelectedObject();
    }

    /// <summary>
    /// When a PlayObject is selected its shows the
    /// prices of the selected PlayObject.
    /// </summary>
    /// 
    /// <param name="playObjectData">The data of selectred PlayObject.</param>
    public void SelectShopPlayObject(PlayObjectData playObjectData) {
        SelectPlayObjectData(playObjectData, buyButttons, playerStore);

        OnSelectShopPlayObject.Invoke(playObjectData);
    }

    /// <summary>
    /// When a PlayObject is selected its shows the
    /// prices of the selected PlayObject.
    /// </summary>
    /// 
    /// <param name="playObjectData">The data of selectred PlayObject.</param>
    public void SelectPlayerPlayObject(PlayObjectData playObjectData) {
        SelectPlayObjectData(playObjectData, sellButtons, chargeToShop ? shopStore : null);
        //ShowSelectedDataPrices(sellButtons, chargeToShop ? shopStore : null);
    }

    /// <summary>
    /// When a buy button is pressed it shows its data in the
    /// TransactionInfoBox to confirm the transaction.
    /// </summary>
    /// 
    /// <param name="moneyInfoBox">The pressed MoneyButton.</param>
    public void ShowTransactionInfoBox(MoneyInfoBox moneyInfoBox) {
        if (selectedData != null) {
            selectedMoneyType = moneyInfoBox.MoneyType;

            //transactionObjectInfobox.gameObject.SetActive(true);
            transactionObjectInfobox.ShowInfo(selectedData);

            transactionMoneyIcon.sprite = moneyInfoBox.MoneyIcon;
            transactionMoneyAmount.text = 
                selectedData.GetPrice(selectedMoneyType).ToString();
        }
    }

    /// <summary>
    /// The ball buys the selected PlayObject to the shop.
    /// </summary>
    public void BuySelectedObject() {
        string objectBoughtId = selectedData.Identifier;

        ExchangeSelectedObject(shopStore, playerStore);

        OnBuyPlayObject.Invoke(selectedData);

        ShowGameStoresData();

        playerStoreTab.SelectShopToggle(objectBoughtId);
    }

    /// <summary>
    /// The ball sells the selected PlayObject from the shop.
    /// </summary>
    public void EquipSelectedObject() {
        if (selectedData != null) {
            switch (selectedData.Type) {
                case PlayObjectType.Player:
                    equipedObject = selectedData;

                    OnEquipPlayObject.Invoke(equipedObject);
                    break;
            }
        }
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// Selects the data of a PlayObject.
    /// </summary>
    /// <param name="playObjectData">The data of the PlayObject.</param>
    /// <param name="buttons">The buttons are interactables if the gameStore has enough money to buy the object.</param>
    /// <param name="gameStore">The game store</param>
    private void SelectPlayObjectData(PlayObjectData playObjectData, Button[] buttons, GameStore gameStore) {
        MoneyInfoBox moneyInfoBox;
        MoneyType moneyType;
        int price;

        selectedData = playObjectData;

        foreach (var button in buttons) {
            if (selectedData != null && IsEquiped(selectedData)) {
                if (button.gameObject.activeSelf) {
                    button.gameObject.SetActive(false);
                }
            }
            else {
                moneyInfoBox = button.GetComponentInChildren<MoneyInfoBox>(true);
                moneyType = moneyInfoBox.MoneyType;
                price = 
                    selectedData == null ? -1 : selectedData.GetPrice(moneyType);

                if (price < 0) {
                    price = 0;
                    button.interactable = false;
                }
                else {
                    button.interactable =
                            gameStore == null
                            || gameStore.GetMoneyQuantity(moneyType) >= price;
                }

                if (!button.gameObject.activeSelf) {
                    button.gameObject.SetActive(true);
                }

                moneyInfoBox.ShowMoneyAmount(moneyType, price);
            }
        }
    }

    /// <summary>
    /// It checks if a PlayObject is equiped.
    /// </summary>
    /// <param name="playObjectData">The data of the PlayObject.</param>
    /// <returns>True if the PlayObject is equiped.</returns>
    private bool IsEquiped(PlayObjectData playObjectData) {
        return
            equipedObject != null && 
            equipedObject.Identifier.
                Equals(playObjectData.Identifier);
    }

    /// <summary>
    /// Buy the selected PlayObject from the seller
    /// to the customer.
    /// </summary>
    /// <param name="seller">The seller.</param>
    /// <param name="customer">The customer.</param>
    private void ExchangeSelectedObject(GameStore seller, GameStore customer) {
        int price = selectedData.GetPrice(selectedMoneyType);

        customer.SubstractMoney(selectedMoneyType, price);
        seller.AddMoney(selectedMoneyType, price);
        customer.AddPlayObject(
            seller.GetPlayObject(selectedData));

        if (shopAnimator.runtimeAnimatorController != null) {
            shopAnimator.Play("TransactioOkAnimation");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void ShowGameStoresData() {
        shopStoreTab.ShowGameStoreData();
        playerStoreTab.ShowGameStoreData();
    }

    /// <summary>
    /// Charges the selected obect from the customer to the seller.
    /// </summary>
    /// <param name="seller">If null, the playerMoney will not be added.</param>
    /// <param name="customer">If null, the playerMoney will not be charged.</param>    
    /*private void ChargeSelectedObject(GameStore seller, GameStore customer) {
        int price = selectedData.GetPrice(selectedMoneyType);

        if (customer != null) {
            customer.SubstractMoney(selectedMoneyType, price);
        }

        if (seller != null) {
            seller.AddMoney(selectedMoneyType, price);
        }
    }*/

    /*private void HideTransactionInfoBox() {
        transactionObjectInfobox.ClearInfo();
        transactionObjectInfobox.gameObject.SetActive(false);
    }*/
    #endregion

    #region Coroutines
    #endregion
}