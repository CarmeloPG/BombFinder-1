using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Each frame It sends an event if the player 
/// is using the keyboard or the mouse.
/// </summary>
public class InputObserver : MonoBehaviour {
    #region Readonly fields
    private readonly int
        leftMouseButton = (int)PointerEventData.InputButton.Left,
        rightMouseButton = (int)PointerEventData.InputButton.Right;
    #endregion

    #region Serialize fields
    [SerializeField]
    private KeyCode pauseKey;
    [SerializeField]
    private KeyCode changeViewKey;
    [SerializeField]
    private KeyCode exposeTileKey;
    [SerializeField]
    private KeyCode markTileKey;
    [SerializeField]
    private KeyCode startPlayKey;
    #endregion

    #region Private fields
    //If it's enabled, it responses to the player controls.
    private bool isEnabled;
    #endregion

    #region Properties
    /// <summary>
    /// It returns if the player controls are enabled.
    /// </summary>
    public bool IsEnabled {
        get => isEnabled;
        set => isEnabled = value;
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When the expose tile key or the left mouse button is released, it sends the event.")]
    public UnityEvent OnExposeTileKeyUp;
    [Tooltip("When the mark tile key or the left mouse button is released, it sends the event.")]
    public UnityEvent OnMarkTileKeyUp;
    [Tooltip("When the pause key is released, it sends the event.")]
    public UnityEvent OnPauseKeyUp;
    [Tooltip("When the chnage view key is released, it sends the event.")]
    public UnityEvent OnChangeViewKeyUp;
    [Tooltip("When the start play key is released, it sends the event.")]
    public UnityEvent OnStartPlayKeyUp;
    #endregion

    #region Unity methods
    /// <summary>
    /// On update, it checks if the play has pressed
    /// or released a key or a mouse button.
    /// </summary>
    private void Update() {
        if (!Application.isMobilePlatform) {
            if (Input.GetKeyUp(pauseKey)) {
                OnPauseKeyUp.Invoke();
            }

            if (Input.GetKeyUp(startPlayKey)) {
                OnStartPlayKeyUp.Invoke();
            }

            if (isEnabled) {
                if (Input.GetMouseButtonUp(leftMouseButton)
                    || Input.GetKeyUp(exposeTileKey)) {
                    OnExposeTileKeyUp.Invoke();
                }
                else if (Input.GetMouseButtonUp(rightMouseButton)
                         || Input.GetKeyUp(markTileKey)) {
                    OnMarkTileKeyUp.Invoke();
                }

                if (Input.GetKeyUp(changeViewKey)) {
                    OnChangeViewKeyUp.Invoke();
                }
            }
        }
    }
    #endregion

    #region Public methods
    public void AddListener(I_MouseClickListner newListener) {
        OnExposeTileKeyUp.AddListener(newListener.OnLeftMouseButtonUp);
        OnMarkTileKeyUp.AddListener(newListener.OnRightMouseButtonUp);
    }

    public void RemoveListener(I_MouseClickListner newListener) {
        OnExposeTileKeyUp.RemoveListener(newListener.OnLeftMouseButtonUp);
        OnMarkTileKeyUp.RemoveListener(newListener.OnRightMouseButtonUp);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}