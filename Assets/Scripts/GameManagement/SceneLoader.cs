using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Events;

public class SceneLoader : MonoBehaviour {
	#region Fields
	[SerializeField]
	private string scenName;
	[SerializeField]
	private float loadDelay = 1f;
	#endregion
	#region Events
	[Header("Events")]
	[Tooltip("Sends the progress of the load.")]
	public UnityEvent<float> OnSceneLoadContinue;
	#endregion

	#region Unity methods
	private void Awake() {
        Invoke(nameof(LoadScene), loadDelay);
    }
	#endregion

	#region Private methods
	private void LoadScene() {
        StartCoroutine(LoadSceneAsync(scenName));
    }
    #endregion

    #region Coroutines
    private IEnumerator LoadSceneAsync(string scene) {
		var asyncOperation = SceneManager.LoadSceneAsync(scene);
		
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
