using System.Collections;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// A mobile bomb.
/// 
/// The mobile bombs are spawned in a random position and it moves to
/// a random direction. When it falls to the deep, it disables itself.
/// </summary>
public class MobileBomb : MonoBehaviour {
    #region Readonly fields
    private readonly string
        //The name of the parameter that plays the explosion animation.
        //explode_trigger = "attack01",
        //The name of teh parameter that plays the walk animation.
        walk_bool = "walk",
        //The name og the parameter that plays the damage animation:
        damage_trigger = "damage";
    #endregion

    #region Serialize fields
    [SerializeField]
    [Tooltip("The bomb contained.")]
    private Bomb bomb;
    [SerializeField]
    [Tooltip("The animator.")]
    private Animator animator;
    [SerializeField]
    [Tooltip("The riggidbody of the bomb.")]
    private Rigidbody bombRigidbody;
    [SerializeField]
    [Tooltip("Velocity of movement")]
    private float velocity;
    [SerializeField]
    [Tooltip("The effect of the steps of the bomb when it is walking.")]
    private ParticleSystem walkEffect;
    [SerializeField]
    [Tooltip("It will be played when the bomb hits the player.")]
    private ParticleSystem hitEffect;
    #endregion

    #region Private fields
    //The velocity of the rigidbody.
    private Vector3 deltaPosition;
    #endregion

    #region Properties
    /// <summary>
    /// It enables or disables the mobile bomb.
    /// </summary>
    public bool EnableBomb {
        get => gameObject.activeSelf;
        set {
            gameObject.SetActive(value);
            
            if (value) {
                StartCoroutine(Walk());
            }
            else {
                //bombRigidbody.velocity = Vector3.zero;

                animator.SetBool(walk_bool, false);

                StopCoroutine(Walk());
            }
        }
    }

    /// <summary>
    /// The bomb.
    /// </summary>
    public Bomb Bomb {
        get => bomb;
    }
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("When it explodes, it sends itself.")]
    //public UnityEvent<MobileBomb> OnExplodeMobileBomb;
    #endregion

    #region Unity methods
    /// <summary>
    /// When it is disabled, it disables the bomb.
    /// </summary>
    private void OnDisable() {
        if (EnableBomb == true) {
            EnableBomb = false;
        }
    }

    /// <summary>
    /// When it is destroyed, it removes all listeners of its events.
    /// </summary>
    private void OnDestroy() {
        bomb.OnExplode.RemoveAllListeners();
    }

    /// <summary>
    /// If it enter on collision with a player, it expldes.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag.Equals(Constants.tag_player)) {
            //bombRigidbody.velocity = Vector3.zero;
            hitEffect.transform.position = collision.transform.position;

            StopAllCoroutines();

            walkEffect.Stop();
            hitEffect.Play();

            animator.SetBool(walk_bool, false);
            animator.SetTrigger(damage_trigger);
            
            bomb.ExplodeBomb();

            Invoke(nameof(DisableBomb), 3f);

            //OnExplodeMobileBomb.Invoke(this);
        }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// It starts to go to a direccion.
    /// </summary>
    /// <param name="direction">The direction.</param>
    public void GoTo(Vector3 direction) {
        deltaPosition = 
            direction.normalized * velocity * Time.deltaTime;
        
        //bombRigidbody.velocity = deltaPosition;
        
        transform.LookAt(transform.position + direction);
        animator.SetBool(walk_bool, true);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// It disables the bomb.
    /// </summary>
    private void DisableBomb() {
        EnableBomb = false;
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Once per second, it checks if has fallen to the deep.
    /// If it has fallen to the deep, it disables itself.
    /// </summary>
    /// <returns>WaitForSeconds: 1sg</returns>
    private IEnumerator Walk() {
        
        while (true) {
            yield return null;

            /*if (!Physics.Raycast(new Vector3(transform.position.x, .21f, transform.position.y), Vector3.down, .5f)) {
                bombRigidbody.useGravity = true;
            }*/

            if (transform.position.y < 0f) {
                //bombRigidbody.useGravity = false;

                EnableBomb = false;
            }
            else {
                transform.position += deltaPosition;
            }
        }
    }
    #endregion
}