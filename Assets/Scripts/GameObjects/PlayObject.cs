using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// A GameObject witch can be bought, sold and stored in a GameStore.
/// </summary>
public class PlayObject : MonoBehaviour
{
	#region Readonly fields
	private readonly string
		pick_up_key = "Pickup",
		death_key = "Death",
		win_key = "Win";
    #endregion

    #region Serialize fields
    [SerializeField]
	private PlayObjectData data;
	[SerializeField]
	private Rigidbody rigidBody;
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private ParticleSystem stunEffect;
    [SerializeField]
	private SimpleCharacterControl characterControl;

    /*[Header("Sounds")]
    [SerializeField]
    private AudioClip enableSound;
	[SerializeField]
	private AudioClip disableSound;
    [SerializeField]
	private AudioClip collisionSound;*/
	#endregion

	#region Private fields
    #endregion

    #region Properties
	public PlayObjectData Data {
		get => data;
	}

	public float Size {
		set => transform.localScale = new Vector3(value, value, value);
	}

	public float Mass {
		set => rigidBody.mass = value;
	}

	public Vector3 Velocity {
		set => rigidBody.velocity = value;
	}

	public bool EnableControls { 
		set => characterControl.enabled = value; 
	}

	public bool IsStunned {
		get => stunEffect.isPlaying;
	}
	#endregion

	#region Events
	#endregion

	#region Unity methods
	#endregion

	#region Public methods
    public void Show(bool show) {
        gameObject.SetActive(show);
    }

	public void Hide() {
		gameObject.SetActive(false);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="stun"></param>
	public void Stun(bool stun) {
		if (stun) {
            animator.SetTrigger(death_key);

            if (characterControl.enabled) {
				characterControl.enabled = false;
			}

			if (!stunEffect.isPlaying) {
				stunEffect.Play();
			}
		}
		else {
            animator.SetTrigger(pick_up_key);

            if (!characterControl.enabled) {
                characterControl.enabled = true;
            }

            if (stunEffect.isPlaying) {
                stunEffect.Stop();
            }
        }
    }

	/// <summary>
	/// 
	/// </summary>
	public void Win() {
		animator.SetTrigger(win_key);
	}

	/// <summary>
	/// 
	/// </summary>
	public void Pickup() {
		animator.SetTrigger(pick_up_key);
	}

	/// <summary>
	/// 
	/// </summary>
	public void ResetTransform() {
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}
    #endregion

    #region Private methods
    #endregion
}
