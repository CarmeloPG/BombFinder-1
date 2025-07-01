using UnityEngine;
using UnityEngine.Events;
//using System;

/// <summary>
/// A tile of a country. It responses to the pointer events.
/// </summary>
public class Tile : MonoBehaviour, I_MouseClickListner {//, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {
    #region Readonly fields
    #endregion

    #region Serialize fields
    [SerializeField]
    [Tooltip("Maximum money in the tile.")]
    private int maxReward = 10;
    [SerializeField]
    private MoneyBox3D moneyBox;

    [Header("Avatars")]
    //[SerializeField]
    //[Tooltip("The avatar of the tile.")]
    //private GameObject body;
    [Tooltip("The avatar of the tile when de mouse is over.")]
    [SerializeField]
    private GameObject highLightedMark;
    [SerializeField]
    [Tooltip("The avatar of the bomb mark.")]
    private GameObject bombMark;
    [SerializeField]
    [Tooltip("Each gameObject of the array represents a number of bombPrefab that are around of the tile. The index of the array is the number must represents the gameObject.")]
    private GameObject[] bombsAround;
    [SerializeField]
    [Tooltip("The game objects witch represnts the number of bombs around of the tile when the game is in air view. It must have the same length as bombsAround.")]
    private GameObject[] bombsAroundAirView;

    [Header("Sounds")]
    [SerializeField]
	[Tooltip("Sound when something is walking on the tile.")]
	private AudioClip stepSound;
    [SerializeField]
    [Tooltip("Effect when something is walking on the tile.")]
    private ParticleSystem stepEffect;
    [SerializeField]
    [Tooltip("Effect when the tile is exposed.")]
    private ParticleSystem exposeEffect;
    #endregion

    #region Private fields
    private AudioManager audioManager;
    private Bomb bomb;
    private Vector2 coords;
    private bool 
        isExposed,
        airView;
    private int 
        reward,
        bombsAroundCount;
    #endregion

    #region Properties
    public int Reward {
        get => reward;
    }

    public bool HasBomb {
		get => bomb != null;
	}

    public bool IsMarked {
        get => bombMark.activeSelf;
    }

    public bool IsExposed {
        get => isExposed;
    }

    public bool AirView {
        set {
            airView = value;

            if (bombsAroundCount > 0) {
                bombsAround[bombsAroundCount].SetActive(!value);
                bombsAroundAirView[bombsAroundCount].SetActive(value);
            }
        }
    }

    public bool HighLighted {
        set => highLightedMark.SetActive(value);
    }

    public int BombsAround {
        //get => bombsAroundCount;
        set {
            bombsAroundCount = value;

            if (!isExposed) {
                isExposed = true;
            }

            for (int i = 0; i < bombsAround.Length; i++) {
                bombsAround[i].SetActive((value == 0 && i == value) || (i == value && !airView));
                bombsAroundAirView[i].SetActive(value > 0 && i == value && airView);
            }
        }
    }

	public Bomb Bomb {
        get => bomb;
		set => bomb = value;
	}

	public Vector2 Coords {
		get => coords;
		set => coords = value;
	}

    public AudioManager AudioManager {
        set => audioManager = value;
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When it's exposed, it sends itself.")]
    public UnityEvent<Tile> OnExpose;
    [Tooltip("When it's marked, it sends itself.")]
    public UnityEvent<Tile> OnMark;
    #endregion

    #region Unity methods
    /// <summary>
    /// It is initialized.
    /// </summary>
    private void Awake() {
        bomb = null;
        isExposed = false;
        reward = -1;
        bombsAroundCount = -1;

        ShowBody(false);

        /*if (bombsAround.Length > 0) {
            ShowBody(false);
        }*/
    }

    /// <summary>
    /// Highlight the tile.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag.Equals(Constants.tag_player)) {
            highLightedMark.SetActive(true);
        }
    }

    /// <summary>
    /// End of highlight the tile.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit(Collision collision) {
        if (highLightedMark.gameObject.activeSelf) {
            highLightedMark.SetActive(false);
        }
    }

    /// <summary>
    /// The characters that can walk on the tile have
    /// a trigger. When the trigger enters it plays
    /// the step sound and the step effect.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        stepEffect.transform.position = other.transform.position;

        stepEffect.Play();
        
        if (other.tag.Equals(Constants.tag_player)) {
            audioManager.PlayEffect(stepSound);
        }
        else {
            audioManager.Play3DEffect(stepSound, other.transform.position);
        }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// The left button marks the tile as tile with bomb.
    /// The left button exposes the tile.
    /// </summary>
    public void OnLeftMouseButtonUp() {
        if (!isExposed && highLightedMark.activeSelf) {
            Expose();
        }
    }

    /// <summary>
    /// The right button marks the tile as tile with bomb.
    /// The left button exposes the tile.
    /// </summary>
    public void OnRightMouseButtonUp() {
        if (!isExposed && highLightedMark.activeSelf) {
            Mark();
        }
    }

    /// <summary>
    /// It shows the body. It hides the others avatars.
    /// </summary>
    public void ShowBody(bool show) {
        exposeEffect.Play();        
        highLightedMark.SetActive(!show && IsMarked);

        if (bombsAround[0].activeSelf != show) {
            bombsAround[0].SetActive(show);
        }

        for (int i = 1; i < bombsAround.Length; i++) {
            if (bombsAround[i].activeSelf) {
                bombsAround[i].SetActive(false);
            }

            if (bombsAroundAirView[i].activeSelf) {
                bombsAround[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Mark or dismark the tile as tile with bomb.
    /// </summary>
    public void Mark() {
        bombMark.SetActive(!bombMark.activeSelf);
        //body.SetActive(bombMark.activeSelf);

        OnMark.Invoke(this);
    }

    /// <summary>
    /// Expose the tile. If it has bomb, the bomb explodes.
    /// </summary>
    public void Expose() {
        if (IsMarked) {
            Mark();
        }

        ShowBody(false);

        if (bomb == null) {
            isExposed = true;
            reward = Random.Range(0, maxReward);

            exposeEffect.Play();
            moneyBox.ShowMoneyAmount(reward);
            OnExpose.Invoke(this);
        }
        else {
            bomb.ExplodeBomb();
        }
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}