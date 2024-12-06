using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using I2.Loc;

/// <summary>
/// An animated window, with a title and a message.
/// </summary>
public class Window : MonoBehaviour {
	#region Readonly fields
	//The names of the animations of the window.
	/*private readonly string
		open_anim_name = "Open",
		close_anim_name = "Close";*/
    #endregion

    #region Serialize fields
    [SerializeField]
	private Localize title;
	[SerializeField]
    private Localize message;

	[Header("Sound options")]
	[SerializeField]
	private AudioClip openSound;
	[SerializeField]
	private AudioClip closeSound;

	[Header("Animation options")]
	[SerializeField]
	private Animator animator;
    [SerializeField]
    private AnimationClip openAnimation;
	[SerializeField]
	private AnimationClip closeAnimation;
	#endregion

	#region Private fileds
	private AudioManager audioManager;
    #endregion

    #region Properties
    public string Title {
		set => title.Term = value;
	}

	public string Message {
		set => message.Term = value;
	}
	#endregion

	#region Events
	[Header("EVents")]
	[Tooltip("When the window is opened, it sends itself.")]
	public UnityEvent WindowOponed;
	[Tooltip("When the window is closed, it sends itself")]
	public UnityEvent WindowClosed;
	//[Tooltip("When a button is pressed, it sends the event.")]
	//public UnityEvent ButtonPressed;
    #endregion

    #region Unity methods
    private void Awake() {
		audioManager = AudioManager.Instance;
    }
    #endregion

    #region Public methods
    public void Open() {
		gameObject.SetActive(true);

		//animator.Play(open_anim_name);

		animator.Play(openAnimation.name);
		audioManager.PlayEffect(openSound);

		WindowOponed.Invoke();
	}

	public void Close() {
        animator.Play(closeAnimation.name);
		audioManager.PlayEffect(closeSound);

		Invoke(nameof(Hide), closeAnimation.length);
    }
	#endregion

	#region Private methods
	private void Hide() {
		gameObject.SetActive(false);

		WindowClosed.Invoke();
	}
	#endregion
}
