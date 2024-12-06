using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// A button that can be locked. When the button is locked it sowhs
/// the padlock and stops being interactable.
/// </summary>
public class LockButton : MonoBehaviour {
	#region Serialize fields
	[SerializeField]
	private Button button;
	[SerializeField]
	private GameObject padlock;
	#endregion

	#region Private fields
	#endregion

	#region Properties
	public bool locked {
		set {
			button.interactable = !value;
			padlock.SetActive(value);
		}
	}
	#endregion

	#region Events
	//[Header("Events")]
    //[Tooltip("")]
	#endregion

	#region Unity methods
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