using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Events;


/// <summary>
/// A bomb.
/// </summary>
public class Bomb : MonoBehaviour {
	#region Readonly fields
	private readonly string explosion_key = "attack01";
    #endregion

    #region Serialize fields
    [SerializeField]
	[Tooltip("The animator of the bomb.")]
	private Animator bombAnimator;
    [SerializeField]
    [Tooltip("The cameras vibrates when it explodes.")]
    private CinemachineImpulseSource impulseSource;
    //[SerializeField]
    //[Tooltip("The explosion sound of the bomb. The bomb will explode with only one of this sounds. The selected sound is random.")]
    //private AudioClip explosionSound;
    //[SerializeField]
    //[Tooltip("The possible explosion effects of the bomb. The bomb will explode with only one of this effects. The selected effect is random.")]
    //private ParticleSystem[] explosionEffects;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    [Header("Events")]
	[Tooltip("When it explodes, it sends an event.")]
	public UnityEvent<Bomb> OnExplode;
    #endregion

    #region Unity methods
    /// <summary>
    /// When it is destroyed, it removes all its listeners.
    /// </summary>
    private void OnDestroy() {
        OnExplode.RemoveAllListeners();
    }
    #endregion

    #region Public methods

    /// <summary>
    /// It explodes the bomb.
    /// </summary>
    public void ExplodeBomb() {
        if (!bombAnimator.gameObject.activeSelf) {
            bombAnimator.gameObject.SetActive(true);
        }

        Invoke(nameof(GenerateImpulse), .6f);
		bombAnimator.SetTrigger(explosion_key);
		
		
		OnExplode.Invoke(this);
	}

    /// <summary>
    /// It turns off the pump after the delay time.
    /// </summary>
    /// <param name="delayTime"></param>
    public void TurnOffBomb(float delayTime = 0f) {
        if (delayTime > 0f) {
            Invoke(nameof(TurnOffBomb), delayTime);
        }
        else {
            TurnOffBomb();
        }
	}
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// It turns off the bomb.
    /// </summary>
    private void TurnOffBomb() {
        if (bombAnimator.gameObject.activeSelf) {
            bombAnimator.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// It generates an impulse on cinemachine.
    /// </summary>
    private void GenerateImpulse() {
        impulseSource.GenerateImpulse();
    }
	#endregion

	#region Coroutines
	#endregion
}