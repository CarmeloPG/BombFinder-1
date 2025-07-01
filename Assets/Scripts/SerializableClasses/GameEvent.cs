using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class GameEvent {
	#region Readonly fields
	#endregion

	#region Serialize fields
	[SerializeField]
	private string eventName;
	[SerializeField]
	[Range(1, 31)]
	private int startDay;
    [SerializeField]
    [Range(1, 12)]
    private int startMonth;
    [SerializeField]
    [Range(1, 31)]
	private int endDay;	
	[SerializeField]
    [Range(1, 12)]
    private int endMonth;
	[SerializeField]
	private PlayObjectSlot[] playObjectSlots;
	#endregion

	#region Private fields
	private int
		currentDay,
		currentMonth;
	#endregion

	#region Properties
	public bool IsActive {
		get {
			if (startMonth == endMonth) {
				return currentDay >= startDay && currentDay <= endDay;
			}
			else {
				return (currentMonth == startMonth && currentDay >= startDay)
					|| (currentMonth == endMonth && currentDay <= endDay);
			}
		}
	}

	public PlayObjectSlot[] PlayObjectSlots {
		get => playObjectSlots;
	}
	#endregion

	#region Events
	//[Header("Events")]
	//[Tooltip("")]
	#endregion

	#region Unity methods
	public GameEvent() {
		DateTime today = DateTime.Today;

		currentDay = today.Day;
		currentMonth = today.Month;

		Debug.Log("Current day: " + currentDay + "currentMonth: " + currentMonth);
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