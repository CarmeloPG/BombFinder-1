using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VirtualControls : MonoBehaviour {
	#region Readonly fields
	#endregion

	#region Serialize fields
	[SerializeField]
	private InputObserver inputObserver;
	[SerializeField]
	private Toggle changeViewToggle;
	[SerializeField]
	private Toggle runToggle;
	#endregion

	#region Private fields
	private bool run;
	private float
		verticlaAxis,
		horizontalAxis;
	#endregion

	#region Properties
	/// <summary>
	/// 
	/// </summary>
	public float VerticalAxis {
		get => verticlaAxis;
		set => verticlaAxis = value;
	}

	/// <summary>
	/// 
	/// </summary>
	public float HorizontalAxis {
		get => horizontalAxis;
		set =>horizontalAxis = value;
	}

	/// <summary>
	/// 
	/// </summary>
	public bool Run {
		get => run;
		set => run = value;
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
	private void Start() {
		run = false;
		verticlaAxis = 0f;
		horizontalAxis = 0f;
	}

	/// <summary>
	/// 
	/// </summary>
    private void OnEnable() {
		Invoke(nameof(ResetToggles), .1f);
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