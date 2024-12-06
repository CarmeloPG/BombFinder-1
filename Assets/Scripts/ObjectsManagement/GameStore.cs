using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// An abstract that impletements methods to manage the moneyType and
/// defines the methods witch must be implemented in an Inventory or a Store.
/// </summary>
public abstract class GameStore : MonoBehaviour {

    #region Serialize fields
    [SerializeField]
    private Money[] moneys;
    #endregion

    #region Private fields
    protected List<PlayObjectData> playObjectsData;
    private Dictionary<MoneyType, Money> moneyDict;
    #endregion

    #region Properties
    /// <summary>
    /// Gets an array with the data of all objects.
    /// </summary>
    public PlayObjectData[] PlayObjectsData {
        get => playObjectsData.ToArray();
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Sends the new amount of a MoneyType.")]
    public UnityEvent<MoneyType, int> OnChangeMoney;
    [Tooltip("Sends the data of the added object.")]
    public UnityEvent<PlayObjectData> OnAddPlayObject;
    [Tooltip("Sends thte data of removed object.")]
    public UnityEvent<PlayObjectData> OnRemovePlayObject;
    #endregion

    #region Unity methods
    #endregion

    #region abstract methods
    /// <summary>
    /// Adds a new object to the store.
    /// </summary>
    /// 
    /// <param name="newObject">The new object.</param>
    public abstract PlayObject GetPlayObject(PlayObjectData objectData);

    /// <summary>
    /// Get the object witch data are objectData.
    /// The object become the root of hierarchy and it's showed.
    /// 
    /// <param name="objectData">A reference to the PlayObject named objectData</param>
    /// <returns>Null if the PlayObject is not in the store.</returns>
	public abstract int GetObjectQuantity(PlayObjectData objectData);
    #endregion

    #region Public methods
    /// <summary>
    /// If the playObject is locked, the palyer
    /// cannot select it.
    /// </summary>
    /// <param name="playObjectData"></param>
    /// <returns>True if the playObject hasn't in the playObjectsDataList.</returns>
    public virtual bool IsLocked(PlayObjectData playObjectData) {
        return !playObjectsData.Contains(playObjectData);
    }

    /// <summary>
    /// </summary>
    /// <param name="moneyType"></param>
    /// <returns>The amount of moneyType</returns>
    public int GetMoneyQuantity(MoneyType moneyType) {
        var money = FindMoneyOfType(moneyType);

        return money == null ? 0 : money.Amount;
    }


    /// <summary>
    /// Substracs an amount of a MoneyType.
    /// </summary>
    /// <param name="moneyType">The moneyType.</param>
    /// /// <param name="amount">The amount.</param>
    public int  SubstractMoney(MoneyType moneyType, int amount){
        return AddMoney(moneyType, -amount);
    }

    /// <summary>
    /// Adds an amount of game money.
    /// </summary>
    /// <param name="amount">The amont.</param>
    public int AddMoney(int amount) {
        var storeMoney = FindMoneyOfType(MoneyType.GameMoney);

        if (storeMoney != null) {
            storeMoney.Amount += amount;

            AddMoney(storeMoney.MoneyType, storeMoney.Amount);

            return storeMoney.Amount;
        }
        else {
            return -1;
        }
    }

    /// <summary>
    /// /// <summary>
    /// Adds an amount of a MoneyType.
    /// </summary>
    /// <param name="moneyType">The MoneyType.</param>
    /// <param name="amount">The amount.</param>
    /// <returns>The added amount.</returns>
    public int AddMoney(MoneyType moneyType, int amount) {
        var storeMoney = FindMoneyOfType(moneyType);

        if (storeMoney != null) {
            storeMoney.Amount += amount;

            OnChangeMoney.Invoke(moneyType, storeMoney.Amount);

            return storeMoney.Amount;
        }
        else {
            return -1;
        }
	}

    /// <summary>
    /// </summary>
    /// <param name="moneyType">The MoneyType.</param>
    /// /// <param name="amount">The amont.</param>
    /*public void SetMoney(MoneyType moneyType, int amount) {
        var storeMoney = FindMoneyOfType(moneyType);

        if (storeMoney != null) {
            storeMoney.Amount = amount;
        }
    }*/

    /// <summary>
    /// The way to add a new object must be 
    /// implemented in extended clases
    /// </summary>
    /// <param name="newObject"></param>
    public virtual void AddPlayObject(PlayObject newObject) {
        playObjectsData.Add(newObject.Data);

        OnAddPlayObject.Invoke(newObject.Data);
    }

    /// <summary>
    /// Adds the array of newObjects to the store.
    /// </summary>
    /// <param name="newObjects"></param>
    public void AddPlayObjects(PlayObject[] newObjects) {
        foreach (var newObject in newObjects) {
            AddPlayObject(newObject);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="objectsData">Name of the required PlayObjects</param>
    /// <returns>An array with the PlayObject named in objectsData</returns>
    public PlayObject[] GetPlayObjects(PlayObjectData[] objectsData) {
        var playObjects = new List<PlayObject>();

        foreach(var anObject in objectsData) {
            var playObject = GetPlayObject(anObject);

            if (playObject != null) {
                playObjects.Add(playObject); 
            }
        }

        return playObjects.ToArray();
    }

    /// <summary>
    /// </summary>
    /// <param name="objectId"></param>
    /// <returns>The data of the PlayObject named objectData</returns>
    public PlayObjectData GetObjectData(string objectId) {
        return playObjectsData.Find(data => data.Identifier.Equals(objectId));
    }

    /// <summary>
    /// The the pley objects and the playerMoney.
    /// </summary>
    public virtual void Set() {
        playObjectsData = new List<PlayObjectData>();
        moneyDict = new Dictionary<MoneyType, Money>();

        for (int i = 0; i < moneys.Length; i++) {
            moneys[i].Amount = 0;
            moneyDict.Add(moneys[i].MoneyType, moneys[i]);
        }
    }

    /// <summary>
    /// Clears all the content of store.
    /// </summary>
    public virtual void Clear() {
        foreach (var money in moneys) {
            money.Amount = 0;
        }
    }
    #endregion

    #region Protected methods
    
    #endregion

    #region Private methods
    /// <summary>
    /// Retuns the Money of type moneyType. If it isn't 
    /// in the store, returns null.
    /// </summary>
    /// <param name="moneyType"></param>
    /// <returns>The Money of type moneyType.</returns>
    private Money FindMoneyOfType(MoneyType moneyType) {
        return moneyDict.GetValueOrDefault(moneyType);
    }
    #endregion
}
