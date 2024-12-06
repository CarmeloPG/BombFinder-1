using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Encapsulates a Toggle and a PlayObjectType.
/// The PlayObjectType is selected in the editor.
/// </summary>
public class PlayObjectTypeToggle : MonoBehaviour {

	#region Readonly fileds
	#endregion

	#region Serialize fields
	[SerializeField]
	private Toggle toggle;
	[SerializeField]
	private PlayObjectType playObjectType;
	#endregion

	#region Private fields
	#endregion

	#region Properties
	public bool IsOn {
		get => toggle.isOn;
		set => toggle.isOn = value;
	}

	public PlayObjectType playOjectType {
		get => playObjectType;
	}
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    private void Awake() {
        toggle.onValueChanged.Invoke(toggle.isOn);
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