using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A store is a GameStore witch uses an array of PlayObjectSlot to get
/// new instances of the objects.
/// 
/// The store manages static data structures. 
/// 
/// The array of slots can only be managed from editor and
/// it do not change in execution timeReward. 
/// 
/// To good work, each slot must have got a PlayObject with
/// a unique identifier.
/// </summary>
public class Store : GameStore {
    #region Serialize fields
    [Header("Store")]
	[SerializeField]
	private PlayObjectSlot[] slots;
	#endregion

	#region Private fields
    private Dictionary<string, PlayObjectSlot> slotsDict;
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods

    #endregion

    #region Public methods
    /// <summary>
    /// If the playObjectSlot that contains the playObject is locked, 
    /// the palyer cannot select the playObject.
    /// </summary>
    /// <param name="playObjectData"></param>
    /// <returns>True if the playObjectSlot is locked.</returns>
    public override bool IsLocked(PlayObjectData playObjectData) {
        return FindSlot(playObjectData).IsLocked;
    }

    /// <summary>
    /// Adds a new object to the store. 
    /// Increases the cuantity of the slot attached to the new object
    /// and destroys the new object.
    /// </summary>
    /// 
    /// <param name="newObject">The new object.</param>
    public override void AddPlayObject(PlayObject newObject) {
        var slot = FindSlot(newObject.Data);

        base.AddPlayObject(newObject);

        if (slot != null) {
            slot.AddObject(newObject);
        }
    }

    /// <summary>
    /// Get the object witch data are objectData.
    /// The object become the root of hierarchy and it's showed
    /// .
    /// <param name="objectData">A reference to the PlayObject named objectData</param>
    /// <returns>Null if the PlayObject is not in the store.</returns>
    public override PlayObject GetPlayObject(PlayObjectData objectData) {
        var slot = FindSlot(objectData);
        
        if (slot == null) {
            return null;
        }
        else {
            var playObject = slot.GetObject();

            if (playObject == null) {
                return null;
            }
            else {
                OnRemovePlayObject.Invoke(playObject.Data);

                return playObject;
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="objectData"></param>
    /// <returns>The amount of PlayObject named objectData in the store.</returns>
    public override int GetObjectQuantity(PlayObjectData objectData) {
        var slot = FindSlot(objectData);

        return slot == null || slot.IsLocked? 0 : slot.CurrentQuantity;
    }

    /// <summary>
    /// Creates the list of objectsData
    /// </summary>
    public override void Set() {
        base.Set();

        slotsDict = new Dictionary<string, PlayObjectSlot>();

        foreach (var slot in slots) {
            slot.Clear();

            playObjectsData.Add(slot.ObjectData);

            slotsDict.Add(slot.ObjectData.Identifier, slot);
        }
    }

    /// <summary>
    /// Clears all the content of store.
    /// </summary>
    public override void Clear() {
        base.Clear();

        foreach (var slot in slots) {
            slot.Clear();
        }
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    private PlayObjectSlot FindSlot(PlayObjectData objectData) {
        PlayObjectSlot slot;

        if (slotsDict.TryGetValue(objectData.Identifier, out slot)) {
            return slot;
        }
        else {
            return null;
        }
    }
    #endregion
}