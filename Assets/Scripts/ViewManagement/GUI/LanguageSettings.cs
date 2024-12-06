using UnityEngine;
using UnityEngine.UI;
using I2.Loc;

public class LanguageSettings : MonoBehaviour {

    #region Readonly fileds
	#endregion

	#region Serialize fields
	[SerializeField]
	private Toggle spanishToggle;
	[SerializeField]
	private Toggle englishToggle;

    #endregion

    #region Private fields
    /*public string Language {
		set => LocalizationManager.CurrentLanguageCode = value;
	}*/
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /// <summary>
    /// 
    /// </summary>
    private void Awake() {
        //string language = LocalizationManager.CurrentLanguageCode;
        
        switch (LocalizationManager.CurrentLanguageCode) {
            case "es":
                if (!spanishToggle.isOn) {
                    englishToggle.SetIsOnWithoutNotify(false);
                    spanishToggle.SetIsOnWithoutNotify(true);
                }

                break;
            case "en":
                if (!englishToggle.isOn) {
                    spanishToggle.SetIsOnWithoutNotify(false);
                    englishToggle.SetIsOnWithoutNotify(true);
                }

                break;
        }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// If a language is seleceted, sets the language to language selected.
    /// </summary>
    /// <param name="select"></param>
    public void OnLanguageSelect(bool select) {
		if (select) {
            if (spanishToggle.isOn) {
                LocalizationManager.CurrentLanguageCode = "es";
            }
            else {
                LocalizationManager.CurrentLanguageCode = "en";
            }
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