using UnityEngine;
using System.Collections;
 
public class PlatformInfoSelector : MonoBehaviour {
    #region Readonly fields
    #endregion

    #region Serialize fields
    [SerializeField]
    private GameObject descktopInfo;
    [SerializeField]
    private GameObject mobileInfo;
    #endregion

    #region Private fields
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
    private void Start() {
        if (Application.isMobilePlatform) {
            mobileInfo.SetActive(true);
            descktopInfo.SetActive(false);
        }
        else {
            mobileInfo.SetActive(false);
            descktopInfo.SetActive(true);
        }
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