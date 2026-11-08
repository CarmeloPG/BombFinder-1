using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Each frame It sends an event if the player 
/// is using the keyboard or the mouse.
/// </summary>
public class InputObserver : MonoBehaviour {
    #region Readonly fields
    /*private readonly int
        leftMouseButton = (int)PointerEventData.InputButton.Left,
        rightMouseButton = (int)PointerEventData.InputButton.Right;*/
    #endregion

    #region Serialize fields
    /*[SerializeField]
    private KeyCode pauseKey;
    [SerializeField]
    private KeyCode changeViewKey;
    [SerializeField]
    private KeyCode exposeTileKey;
    [SerializeField]
    private KeyCode markTileKey;
    [SerializeField]
    private KeyCode startPlayKey;*/
    #endregion

    #region Private fields
    //If it's enabled, it responses to the player controls.
    private bool isEnabled;
    private InputAction
        pauseAction,
        changeViewAction,
        exposeTileAction,
        markTileAction,
        startPlayAction;
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
    /// 
    /// </summary>
    private void Awake() {
        pauseAction = InputSystem.actions.FindAction("Pause");
        changeViewAction = InputSystem.actions.FindAction("ChangeView");
        exposeTileAction = InputSystem.actions.FindAction("ExposeTile");
        markTileAction = InputSystem.actions.FindAction("MarkTile");
        startPlayAction = InputSystem.actions.FindAction("StartPlay");
        
        if (!Application.isMobilePlatform) {
            pauseAction.performed += delegate {
                OnPauseKeyUp.Invoke();
            };

            startPlayAction.performed += delegate {
                OnStartPlayKeyUp.Invoke();
            };

            exposeTileAction.performed += delegate {
                if (isEnabled) {
                    OnExposeTileKeyUp.Invoke();
                }
            };

            markTileAction.performed += delegate {
                if (isEnabled) {
                    OnMarkTileKeyUp.Invoke();
                }
            };

            changeViewAction.performed += delegate {
                OnChangeViewKeyUp.Invoke();
            };
        }
    }

    /// <summary>
    /// On update, it checks if the play has pressed
    /// or released a key or a mouse button.
    /// </summary>
    /*private void Update() {
        if (!Application.isMobilePlatform) {
            pauseAction.performed += delegate {
                OnPauseKeyUp.Invoke();
            };

            if (startPlayAction.ReadValue<float>() > 0f) {
                OnStartPlayKeyUp.Invoke();
            }

            if (isEnabled) {
                if (exposeTileAction.ReadValue<float>() > 0f) {
                    OnExposeTileKeyUp.Invoke();
                }
                else if (markTileAction.ReadValue<float>() > 0f) {
                    OnMarkTileKeyUp.Invoke();
                }

                if (changeViewAction.ReadValue<float>() > 0f) {
                    OnChangeViewKeyUp.Invoke();
                }
            }
        }
    }*/
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