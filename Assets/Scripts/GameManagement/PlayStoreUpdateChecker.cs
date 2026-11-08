//#if UNITY_ANDROID
using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;
//using Unity.Collections;
//#endif
using UnityEngine;

public class PlayStoreUpdateChecker : MonoBehaviour {
//#if UNITY_ANDROID
    #region Readonly fields
    #endregion

    #region Serialize fields
    [SerializeField]
    private LoadingSceneManager loadingSceneManager;
    #endregion

    #region Private fields
    private AppUpdateManager appUpdateManager;
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /// <summary>
    /// Initializes the update checker.
    /// </summary>
    private void Start() {
        if (!Application.isEditor && Application.internetReachability != NetworkReachability.NotReachable) {
            appUpdateManager = new AppUpdateManager();

            StartCoroutine(CheckForUpdate());
        }
        else {
            loadingSceneManager.LoadMainScene();
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
    /// <summary>
    /// Checks for available updates.
    /// </summary>
    /// <returns>The coroutine enumerator.</returns>
    private IEnumerator CheckForUpdate() {
        bool updateAvailable = false;
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
          appUpdateManager.GetAppUpdateInfo();

        // Wait until the asynchronous operation completes.
        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful) {
            AppUpdateInfo appUpdateInfoResult = appUpdateInfoOperation.GetResult();

            if (appUpdateInfoResult.AppUpdateStatus == AppUpdateStatus.Installing) {
                Application.Quit();
            }
            else {
                updateAvailable =
                    appUpdateInfoResult.UpdateAvailability == UpdateAvailability.UpdateAvailable
                    && appUpdateInfoResult.AppUpdateStatus != AppUpdateStatus.Installed;
            }
        }

        if (updateAvailable) {
            loadingSceneManager.RequestUpdate();
        }
        else {
            loadingSceneManager.LoadMainScene();
        }
    }
    #endregion
//#endif
}