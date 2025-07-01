using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour {
	#region Readonly fields
	#endregion

	#region Serialize fields
	[SerializeField]
	private Toggle muteEffectsToggle;
	[SerializeField]
	private Toggle muteMusicToggle;
    [SerializeField]
    private Slider volumeEffecsSlider;
    [SerializeField]
    private Slider volumeMusicSlider;
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

        muteMusicToggle.onValueChanged.AddListener(
            delegate {
                audioManager.IsMusicMuted = !muteMusicToggle.isOn;
            });

        muteEffectsToggle.onValueChanged.AddListener(
            delegate {
                audioManager.IsEffectsMuted = !muteEffectsToggle.isOn;
            });

        volumeMusicSlider.onValueChanged.AddListener(
            delegate {
                audioManager.MusicVolume = volumeMusicSlider.value;
            });

        volumeEffecsSlider.onValueChanged.AddListener(
            delegate {
                audioManager.EffectsVolume = volumeEffecsSlider.value;
            });
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnEnable() {
        muteMusicToggle.SetIsOnWithoutNotify(!audioManager.IsMusicMuted);
        muteEffectsToggle.SetIsOnWithoutNotify(!audioManager.IsEffectsMuted);
        volumeMusicSlider.SetValueWithoutNotify(audioManager.MusicVolume);
        volumeEffecsSlider.SetValueWithoutNotify(audioManager.EffectsVolume);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDestroy() {
        muteEffectsToggle.onValueChanged.RemoveAllListeners();
        muteMusicToggle.onValueChanged.RemoveAllListeners();
        volumeMusicSlider.onValueChanged.RemoveAllListeners();
        volumeEffecsSlider.onValueChanged.RemoveAllListeners();
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