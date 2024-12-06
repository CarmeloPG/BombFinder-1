using System;
using System.Threading;
using UnityEngine;


/// <summary>
/// It is a ScriptableObject witch can limit the number of instances that 
/// can be created of a PlayObject in the game.
/// Can be locked. If it is locked will never create a instance of the object.
/// </summary>
[CreateAssetMenu(fileName = "New PlayObjectSlot", menuName = "ScriptableObjects/PlayObjectSlot")]
public class PlayObjectSlot : ScriptableObject {

	#region Serialize fields
	[SerializeField]
	[Tooltip("Object witch get references.")]
	private PlayObject targetObjectPrefab;
	[SerializeField]
	[Tooltip("Maximum number of instances that can be returned. If < 0 the instances are infinite.")]
	private int capacity;
	[SerializeField]
	[Tooltip("Current number of instances of the targetOpbject witch can be isntantiated.")]
    private int currentQuantity;
    [SerializeField]
	[Tooltip("If it is locked will never create a instance of the object. Stil can be added new objects.")]
	private bool locked = false;
	#endregion

	#region Private fields
	#endregion

	#region Properties
	public PlayObjectData ObjectData {
		get => targetObjectPrefab.Data;
	}

	public int Capacity {
		get => capacity;
	}

	public int CurrentQuantity {
		get => currentQuantity;
	}

	public bool IsLocked {
		get => locked;
		set => locked = value;
	}

    public bool IsEmpty {
		get => currentQuantity == 0;

    }

	public bool IsFull {
		get => currentQuantity == capacity; 
	}
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Constructors
    #endregion


    #region Public methods
    /// <summary>
    /// </summary>
    /// <returns>A refence to targetObjectPrefab or null if it is locked or amount == 0.</returns>
    public PlayObject GetObject() {
		if (!locked && currentQuantity != 0) {
			if (currentQuantity > 0) {
				currentQuantity--;
			}

			return Instantiate(targetObjectPrefab);
		}
		else {
			return null; 
		}
	}

    /// <summary>
    /// Add 1 to amount of instnaces of targetObjectPrefab.
    /// If the slot is full is locked or its capacity is infinite,
    /// it will destroy the new object whitout add 
    /// any value to current quantity.
    /// 
    /// <param name="newObject"></param>
    /// </summary>
    public void AddObject(PlayObject newObject) {
		if (!locked && capacity > 0) {
			currentQuantity = Math.Min(currentQuantity + 1, capacity);
		}

        DestroyImmediate(newObject.gameObject);
    }

	/// <summary>
	/// Sets the current quantity to capacity.
	/// </summary>
	public void Clear() {
		currentQuantity = capacity;
	}
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
	#endregion

	#region Coroutines
	#endregion
}