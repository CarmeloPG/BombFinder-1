using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

/// <summary>
/// Manages all objects in the game.
/// </summary>
public class ObjectsManager : MonoBehaviour {
	#region Serialize fields
	[SerializeField]
	private GameStore playerInventory;
    [SerializeField]
    private List<GameStore> shopStores;
    [SerializeField]
	[Tooltip("All play objects of the game.")]
	private List<PlayObjectSlot> playObjects;
	#endregion

	#region Private fields
	private PlayObjectData equipedPlayObject;
	#endregion

	#region Properties
	public PlayObjectSlot[] PlayObjects {
		get => playObjects.ToArray();
	}

	public PlayObject SelectedPlayer {
		get {
			PlayObject selectedPlayer = 
				playerInventory.GetPlayObject(equipedPlayObject);

			equipedPlayObject = null;
			
			return selectedPlayer;
		}
		set {
			PlayObjectData theData = Array.Find(
					playerInventory.PlayObjectsData, 
					playObject => playObject.Identifier.Equals(value.Data.Identifier));

			equipedPlayObject = value.Data;

			if (theData == null) {
                playerInventory.AddPlayObject(value);
            }
		}
	}
	#endregion

	#region Events
	[Header("Events")]
	[Tooltip("When enter or exits in pause, it sends the pause state.")]
	public UnityEvent<PlayObject> OnSendPlayObject;
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    public void LoadObjects(GameStatus status) {
		PlayObjectSlot playObjectSlot;

        playerInventory.Set();

		foreach (GameStore shopStore in shopStores) {
            shopStore.Set();
        }
		
        foreach (string id in status.playerInventory) {
            playObjectSlot = playObjects.
				Find (slot => slot.ObjectData.Identifier.Equals (id));

            playerInventory.
				AddPlayObject(playObjectSlot.GetObject());
        }

		equipedPlayObject =
            playerInventory.GetObjectData(status.equipedObject);

        playerInventory.
			AddMoney(MoneyType.GameMoney, status.playerMoney);
    }

	public void EquipPlayObject(PlayObjectData playObjectData) {
		equipedPlayObject = playObjectData;
	}

	public void GiveRewardToPlayer(EndLevelInfoBox endLevelInfoBox) {
        playerInventory.
			AddMoney(MoneyType.GameMoney, endLevelInfoBox.Reward);
    }

	public void SendPlayerPlayObject(PlayObjectData playObjectData) {
		SendPlayObject(
			playerInventory.GetPlayObject(playObjectData));
	}

	public void SendPlayObject(PlayObject playObject) {
        OnSendPlayObject.Invoke(playObject);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}