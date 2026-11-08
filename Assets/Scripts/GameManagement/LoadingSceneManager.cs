using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;

public class LoadingSceneManager : MonoBehaviour {
	#region Serialize fields
#if UNITY_ANDROID
	[SerializeField]
	private Window updateWindow;
	[SerializeField]
	private GameObject bomb;
#endif
	[SerializeField]
	private string mainSceneName;
	[SerializeField]
	private float loadDelay = 1f;
	#endregion

	#region Events
	[Header("Events")]
	[Tooltip("Sends the progress of the load.")]
	public UnityEvent<float> OnSceneLoadContinue;
    #endregion

    #region Unity methods
/*#if !UNITY_ANDROID || UNITY_EDITOR
    private void Awake() {
        Invoke(nameof(LoadMainScene), loadDelay);
    }
#endif*/
	#endregion


	#region Public methods
#if UNITY_ANDROID
	/// <summary>
	/// 
	/// </summary>
	public void RequestUpdate() {
		bomb.SetActive(false);
		updateWindow.Open();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="link"></param>
	public void GoToLink(string link) {
		Application.OpenURL(link);
		Application.Quit();
	}
#endif

	/// <summary>
	/// 
	/// </summary>
	public void LoadMainScene() {
//#if UNITY_ANDROID
		if (!bomb.activeSelf) {
			bomb.SetActive(true);
		}
//#endif

        StartCoroutine(LoadSceneAsync(mainSceneName));
    }
#endregion

	#region Coroutines
    private IEnumerator LoadSceneAsync(string scene) {
		var asyncOperation = SceneManager.LoadSceneAsync(scene);
		
		yield return new WaitForSecondsRealtime(loadDelay);

        if (asyncOperation != null) {
			float loadProgress = 0f;

			asyncOperation.allowSceneActivation = false;

			while (loadProgress < .9f) {
				loadProgress = asyncOperation.progress;
				
                OnSceneLoadContinue.Invoke(loadProgress);
				
				yield return new WaitForSecondsRealtime(1f);
			}

            asyncOperation.allowSceneActivation = true;
        }
	}
	#endregion
}
