using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Shows all the content of a GameStore.
/// </summary>

public class GameStoreTab : MonoBehaviour {

    #region Readonly fileds
    #endregion

    #region Serialize fields
    [SerializeField]
    private RectTransform storeEmptyInfo;
    [SerializeField]
    [Tooltip("The playObjectTogglePrefab must to has a PlayObjectInfoBox component.")]
    private Toggle playObjectTogglePrefab;
    [SerializeField]
    private ToggleGroup toggleGroup;
    [SerializeField]
	private GameStore gameStore;
    [SerializeField]
    [Tooltip("Shows the information of a PlayObject when it's selected.")]
    private PlayObjectInfoBox selectedObjectInfoBox;
    [SerializeField]
    [Tooltip("Shows the PlayObjects in the shopStore")]
    private DinamycMultipageNavigation playObjectsPage;
    [SerializeField]
    [Tooltip("Show the amount of the diferent MonyeType in the shopStore.")]
    private MoneyInfoBox[] moneyInfoBoxes;
    #endregion

    #region Private fields
    private Toggle selectedToggle;
    private List<PlayObjectType> selectedPlayObjectTypes;
    private Dictionary<PlayObjectType, List<Toggle>> togglesByType;
    #endregion

    #region Properties
    public GameStore GameStore {
        get => gameStore;
        set {
            ClearGameStoreData();

            gameStore = value;

            ShowGameStoreData();
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Sends the playObjectData of seleceted PlayObject or null if there isn`t selected PlayObject")]
    public UnityEvent<PlayObjectData> PlayObjectSelected;
    #endregion

    #region Unity methods
    private void OnDestroy() {
        gameStore.OnChangeMoney.RemoveListener(ChangeGameStoreMoney);
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Loads the playObjectData of the store and show them.
    /// </summary>
    public void LoadStore() {
        var playObjectTypes = Enum.GetValues(typeof(PlayObjectType)).Cast<PlayObjectType>();

        gameStore.OnChangeMoney.AddListener(ChangeGameStoreMoney);

        selectedToggle = null;
        selectedPlayObjectTypes = new List<PlayObjectType>(playObjectTypes.Count());
        togglesByType = new Dictionary<PlayObjectType, List<Toggle>>();

        foreach (var type in playObjectTypes) {
            togglesByType.Add(type, new List<Toggle>());
        }

        playObjectsPage.LoadNavigation();

        //ShowGameStoreData();
    }

    /// <summary>
    /// It gets the toggle that contains the playObjectData 
    /// and select the toggle.
    /// </summary>
    /// <param name="playObjectData">The play object data.</param>
    public void SelectShopToggle(PlayObjectData playObjectData) {
        Toggle toggle = 
            togglesByType.GetValueOrDefault(playObjectData.Type).
            Find(aToggle => aToggle.name.Equals(playObjectData.Identifier));

        if (toggle != null && !toggle.isOn) {
            toggle.isOn = true;
        }
        else {
            Debug.Log("Toggle null");
        }
    }

    /// <summary>
    /// It selects a toggle of the toggles list.
    /// </summary>
    /// <param name="toggleName">The name of the toggle.</param>
    public void SelectShopToggle(string toggleName) {
        playObjectsPage.SelectToggle(toggleName);
    }

    /// <summary>
    /// When a toggle is selected, shows the information
    /// of the PlayObject attached to the togle.
    /// </summary>
    /// <param name="toggle">The selected toggle.</param>
    public void SelectShopToggle(Toggle toggle) {
        if (toggle.isOn) {
            var data = toggle.GetComponent<PlayObjectInfoBox>().Data;

            selectedToggle = toggle;
            
            selectedObjectInfoBox.ShowInfo(data);
            PlayObjectSelected.Invoke(data);
        }
        else {
            selectedToggle = null;
            
            selectedObjectInfoBox.ClearInfo();
            PlayObjectSelected.Invoke(null);
        }

    }

    /*public void AddPlayObjectToStore(PlayObjectData playObjectData) {
        ShowPlayObjectData(playObjectData);
    }

    public void RemovePlayObjectFromStore(PlayObjectData playObjectData) {
        if (selectedToggle != null) {
            playObjectsPage.RemoveToggleFromCurrentPage(selectedToggle);
            togglesByType.GetValueOrDefault(playObjectData.Type).Remove(selectedToggle);
            
            selectedToggle = null;
        }
    }*/

    public void ChangeGameStoreMoney(MoneyType moneyType, int amount) {
        var moneyInfoBox = Array.Find(moneyInfoBoxes, box => box.MoneyType == moneyType);

        if (moneyInfoBox != null) {
            moneyInfoBox.ShowMoneyAmount(moneyType, amount);
        }
    }


    public void ChangeValuePlayObjectTypeToggle(PlayObjectTypeToggle typeToggle) {
        if (togglesByType.TryGetValue(typeToggle.playOjectType, out var toggles)) {
            if (selectedToggle != null && !toggles.Contains(selectedToggle)) {
                selectedToggle.isOn = false;
            }

            foreach (var toggle in toggles) {
                if (typeToggle.IsOn) {
                    selectedPlayObjectTypes.Add(typeToggle.playOjectType);
                    toggle.gameObject.SetActive(true);
                }
                else {
                    selectedPlayObjectTypes.Remove(typeToggle.playOjectType);
                    toggle.gameObject.SetActive(false);
                }
                
            }

            playObjectsPage.SetCurrentPageSize();
        }
    }

    /// <summary>
    /// Shows the playObjectData of the content in the shopStore.
    /// </summary>
    public void ShowGameStoreData() {
        MoneyType moneyType;
        PlayObjectData[]
            playObjectsData = gameStore.PlayObjectsData;

        ClearGameStoreData();

        foreach (var moneyInfo in moneyInfoBoxes) {
            moneyType = moneyInfo.MoneyType;

            moneyInfo.ShowMoneyAmount(moneyType, gameStore.GetMoneyQuantity(moneyType));
        }

        if (playObjectsData.Length == 0) {
            storeEmptyInfo.gameObject.SetActive(true);
        }
        else {
            storeEmptyInfo.gameObject.SetActive(false);

            foreach (var playObjectData in playObjectsData) {
                ShowPlayObjectData(playObjectData);
            }
        }
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// Clears all playObjectData of the GameStoreTab.
    /// </summary>
    public void ClearGameStoreData() {
        if (selectedToggle != null) {
            selectedToggle.isOn = false;
        }

        playObjectsPage.Clear();
        
        foreach (var playObjectType in togglesByType.Keys) {
            togglesByType.GetValueOrDefault(playObjectType).Clear();
        }
        
        foreach (var moneyInfo in moneyInfoBoxes) {
            moneyInfo.ShowMoneyAmount(moneyInfo.MoneyType, 0);
        }
    }

    /// <summary>
    /// Shows a playObjectData in the playObjectsPage.
    /// </summary>
    /// <param name="playObjectData">The playObjectData.</param>
    private void ShowPlayObjectData(PlayObjectData playObjectData) {
        var storeToggle = Instantiate(playObjectTogglePrefab);
        var infoBox = storeToggle.GetComponent<PlayObjectInfoBox>();
        bool playObjectLocked = gameStore.IsLocked(playObjectData);

        storeToggle.group = toggleGroup;
        storeToggle.name = playObjectData.Identifier;

        infoBox.ShowInfo(playObjectData);

        toggleGroup.RegisterToggle(storeToggle);
        togglesByType.
            GetValueOrDefault(playObjectData.Type).Add(storeToggle);

        playObjectsPage.AddToggle(storeToggle);

        storeToggle.interactable = 
            gameStore.GetObjectQuantity(playObjectData) != 0;

        storeToggle.targetGraphic.
            gameObject.SetActive(!playObjectLocked);

        storeToggle.gameObject.SetActive(
            selectedPlayObjectTypes.Contains(playObjectData.Type));
    }
    #endregion

    #region Coroutines
    #endregion
}