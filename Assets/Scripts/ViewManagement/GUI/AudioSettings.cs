using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {
	#region Readonly fields
	#endregion

	#region Serialize fields
	[SerializeField]
	private Toggle effectsToggle;
	[SerializeField]
	private Toggle musicToggle;
    #endregion

    #region Private fields
    private AudioManager audioManager;
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /// <summary>
    /// On awke it checks the audio options.
    /// </summary>
    private void Awake() {
        audioManager = AudioManager.Instance;

        musicToggle.isOn = audioManager.MusicMuted;
        effectsToggle.isOn = audioManager.EffectsMuted;

        musicToggle.onValueChanged.AddListener(
        delegate {
                audioManager.MusicMuted = musicToggle.isOn;
            });

        effectsToggle.onValueChanged.AddListener(
            delegate {
                audioManager.EffectsMuted = effectsToggle.isOn;
            });
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