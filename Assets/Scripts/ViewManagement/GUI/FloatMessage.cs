using UnityEngine;
using TMPro;
using I2.Loc;
using System.Collections;

/// <summary>
/// It Shows a message for some timeReward, then it hides the message.
/// </summary>
public class FloatMessage : MonoBehaviour {
    #region Readonly fileds
    #endregion

    #region Serialize fields
    //[SerializeField]
    //private GUIAnimFREE GuiAnim;
    [SerializeField]
    private TextMeshProUGUI message;
    [SerializeField]
    [Tooltip("Show timeReward in seconds")]
    private float showTime = 3.0f;
    [SerializeField]
    [Tooltip("After fadeTime seconds the game object will be disabled")]
    private float fadeTime = 1.0f;

    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    /// <summary>
    /// It shows theMessage. The message will start to be closed after showTime seconds.
    /// </summary>
    /// <param name="theMessage">Text to show.</param>
    public void ShowMessage(string theMessage) {
        message.text = LocalizationManager.GetTranslation(theMessage);

        gameObject.SetActive(true);

        StartCoroutine(ShowMessage());
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    private IEnumerator ShowMessage() {
        float deltaAlpha = fadeTime * Time.deltaTime;
        var color = new Color(
            message.color.r,
            message.color.g,
            message.color.b,
            0f);

        message.color = color;

        while (message.color.a < 1) {
            yield return null;

            color.a += deltaAlpha;
        }

        yield return (showTime);

        while (message.color.a > 0) {
            yield return null;

            color.a -= deltaAlpha;
        }

        gameObject.SetActive(false);
    }
    #endregion
}