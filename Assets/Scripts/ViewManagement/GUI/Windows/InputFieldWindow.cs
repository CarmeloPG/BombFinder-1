using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// A window with two bottons and an InputField
/// wicht allows the user to enter a text.
/// </summary>
public class InputFieldWindow : Window {
	#region Serialize fields
	[Header("InputFieldWindow")]
	[SerializeField]
	private Button acceptButton;
	[SerializeField]
	private TMP_InputField inputField;
	#endregion

	#region Private fields
	#endregion

	#region Properties
	#endregion

	#region Events
	[Header("Events")]
	public UnityEvent<string> OnTextValidated;
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    public void OnInputFieldValueChanged(string fieldValue) {
		acceptButton.interactable = !string.IsNullOrEmpty(fieldValue);
	}

	public void Close(bool validateText) {
		if (validateText) {
			OnTextValidated.Invoke(inputField.text);
		}

		Close();
	}
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
	#endregion

	#region Coroutines
	#endregion
}