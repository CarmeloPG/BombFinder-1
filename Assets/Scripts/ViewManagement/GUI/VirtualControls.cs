using UnityEngine;
using UnityEngine.UI;

public class VirtualControls : MonoBehaviour {
	#region Readonly fields
	#endregion

	#region Serialize fields
	[SerializeField]
	private InputObserver inputObserver;
	[SerializeField]
	private Joystick moveJoystick;
	[SerializeField]
	private Joystick lookJoystick;
    [SerializeField]
	private Toggle changeViewToggle;
	[SerializeField]
	private Toggle runToggle;
	[SerializeField]
	private GameObject playerControls;
	#endregion

	#region Private fields
	private bool run;
	/*private Vector2
		move,
		look;*/
	#endregion

	#region Properties
	/// <summary>
	/// 
	/// </summary>
	public Vector2 Move {
		get => moveJoystick.Direction;//move;
		//set => move = value;
	}

	/// <summary>
	/// 
	/// </summary>
	public Vector2 Look {
		get => lookJoystick.Direction;//look;
		//set => look = value;
	}

	/// <summary>
	/// 
	/// </summary>
	public bool Run {
		get => run;
		set => run = value;
	}

	/// <summary>
	/// 
	/// </summary>
	public bool IsEnabled {
		set => playerControls.SetActive(value);
	}
	#endregion

	#region Events
	//[Header("Events")]
	//[Tooltip("")]
	#endregion

	#region Unity methods
	/// <summary>
	/// 
	/// </summary>
	/*private void Start() {
		//run = false;

		//runToggle.SetIsOnWithoutNotify(false);
        //move = Vector2.zero;
        //look = Vector2.zero;
    }*/

	/// <summary>
	/// 
	/// </summary>
    private void OnEnable() {
		run = false;

        Invoke(nameof(ResetToggles), .5f);
    }
    #endregion

    #region Public methods
    /// <summary>
    /// 
    /// </summary>
    public void StartGame() {
		inputObserver.OnStartPlayKeyUp.Invoke();
	}

	/// <summary>
	/// 
	/// </summary>
	public void ChangeView() {
		inputObserver.OnChangeViewKeyUp.Invoke();
	}

	/// <summary>
	/// 
	/// </summary>
	public void Pause() {
		inputObserver.OnPauseKeyUp.Invoke();
	}

	/// <summary>
	/// 
	/// </summary>
	public void ExposeTile() {
		inputObserver.OnExposeTileKeyUp.Invoke();
	}

	/// <summary>
	/// 
	/// </summary>
	public void MarkTile() {
		inputObserver.OnMarkTileKeyUp.Invoke();
	}
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
	private void ResetToggles() {
		if (changeViewToggle.isOn) {
            changeViewToggle.SetIsOnWithoutNotify(false);
        }

		if (runToggle.isOn) {
			runToggle.isOn = false;
		}
    }
	#endregion

	#region Coroutines
	#endregion
}