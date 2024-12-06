using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/// <summary>
/// It intercepteds the pointer events and plays a
/// sound for each event.
/// </summary>
public class PointerEventSoundPlayer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {

    #region Readonly fields
    #endregion

    #region Serialize fields
    [SerializeField]
    private AudioClip pointerEnterSound;
    [SerializeField]
    private AudioClip pointerClickSound;
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
    /// On awake it sets the audio manager.
    /// </summary>
    private void Awake() {
        audioManager = AudioManager.Instance;
    }
    #endregion

    /// <summary>
    /// It plays the pointer click sound.
    /// </summary>
    /// <param name="eventData"></param>
    #region Public methods
    public void OnPointerClick(PointerEventData eventData) {
        audioManager.PlayEffect(pointerClickSound);
    }

    /// <summary>
    /// It plays the pointer enter sound.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData) {
        audioManager.PlayEffect(pointerEnterSound);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion

}