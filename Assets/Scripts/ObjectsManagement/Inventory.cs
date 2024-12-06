using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An inventory is a GameStore witch contains comerciable objects.
/// 
/// The inventory manages dinamyc data structures the PlayObject list can
/// change in execution timeReward and can have got objects with the same identifier.
/// The content objects are scene instances witch are doughters of the 
/// GameObject Inventory. When a playobject leave the inventory, the playobject
/// changes its father in hierarchy.
/// </summary>
public class Inventory : GameStore {

    #region Serialize fileds
    [Header("Inventory")]
    [SerializeField]
    [Tooltip("List of objects in the inventory.")]
    private List<PlayObject> playObjects;
    #endregion

    #region Private fields
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    
    #endregion

    #region Public methods
    /// <summary>
    /// Adds a new object to the inventory.
    /// The new object become child of this
    /// Inventory and it's hiden.
    /// </summary>
    /// 
    /// <param name="newObject">The new PlayObject</param>
    public override void AddPlayObject(PlayObject newObject) {
        if (newObject != null) {
            base.AddPlayObject(newObject);

            newObject.transform.SetParent(transform);

            newObject.ResetTransform();
            newObject.Show(false);
            
            playObjects.Add(newObject);
        }
    }

    /// <summary>
    /// Get the object witch data are objectData.
    /// The object become the root of hierarchy and it's showed
    /// .
    /// <param name="objectData">A reference to the PlayObject named objectData</param>
    /// <returns>Null if the PlayObject is not in the store.</returns>
    public override PlayObject GetPlayObject(PlayObjectData objectData) {
        var theObject = 
                playObjects.Find(playObject => 
                    playObject.Data.Identifier.Equals(objectData.Identifier));

        if (theObject != null) {
            theObject.transform.parent = theObject.transform;

            theObject.ResetTransform();
            theObject.Show(true);

            playObjects.Remove(theObject);
            playObjectsData.Remove(theObject.Data);

            OnRemovePlayObject.Invoke(theObject.Data);
        }

        return theObject;
    }

    /// <summary>
    /// </summary>
    /// <param name="objectData">The number of ComerciableObjects named objectData in the inventory.</param>
    /// <returns></returns>
    public override int GetObjectQuantity(PlayObjectData objectData) {
        var theObjects = playObjects.FindAll(
                o => o.Data.Identifier.Equals(objectData.Identifier));

        return theObjects.Count;
    }

    /// <summary>
    /// Clear and destroy all PlayObject in the invenotry.
    /// </summary>
    public override void Clear() {
        var children = GetComponents<PlayObject>();

        base.Clear();

        playObjects.Clear();

        foreach (var child in children) {
            DestroyImmediate(child.gameObject);
        }
    }

    /// <summary>
    /// The the pley objects and the playerMoney.
    /// </summary>
    public override void Set() {
        base.Set();

        foreach (var playObject in playObjects) {
            playObjectsData.Add(playObject.Data);
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