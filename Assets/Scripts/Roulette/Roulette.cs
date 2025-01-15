using I2.Loc;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Roulette : MonoBehaviour {
    #region Readonly fields
    #endregion

    #region Serialize fields
    [SerializeField]
    private TextMeshProUGUI rewardMessage;
    [SerializeField]
    private GameObject rewardBox;
    [SerializeField]
    private Rigidbody2D pivot;
    [SerializeField]
    private RouletteItem[] rouletteItems;

    [Header("Spin settings")]
    [SerializeField]
    private float minSpinForce;
    [SerializeField]
    private float maxSpinForce;

    [Header("Sound settings")]
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private AudioClip spinSound;
    [SerializeField]
    private AudioClip winSound;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    public bool IsStop {
        get {
            if (Mathf.Abs(pivot.angularVelocity) < 1.0f) {
                pivot.angularVelocity = .0f;
            }

            return pivot.angularVelocity == .0f;
        }
    }

    /// <summary>
    /// The roulette is ready when the player can spin it.
    /// </summary>
    public bool IsReady {
        get =>
            gameObject.activeInHierarchy &&
            gameObject.activeSelf &&
            IsStop;
    }
    #endregion

    #region Events
    [Header("Events")]
    //[Tooltip("When the roulete is spining, it sends an event every timeReward a RoletteItem enters in arrow Collider. ")]
    //public UnityEvent OnSpinRoulette;
    [Tooltip("When the roulete ends to spin, it sends the result.")]
    public UnityEvent<int> OnEndSpinRoulette;
    #endregion

    #region Unity methods
    private void OnEnable() {
        pivot.transform.localRotation = Quaternion.identity;

        for (int i = 0; i < rouletteItems.Length; i++) {
            rouletteItems[i].transform.rotation = Quaternion.identity;
        }

        if (rewardBox.activeSelf) {
            rewardBox.SetActive(false);
        }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// 
    /// </summary>
    public void SpinRoulete() {
        if (gameObject.activeSelf && gameObject.activeInHierarchy) {
            pivot.AddTorque(Random.Range(minSpinForce, maxSpinForce));
            audioManager.PlayEffect(spinSound);

            StartCoroutine(CheckResult());
        }
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    private IEnumerator CheckResult() {
        RouletteItem winnerItem;
        string rewardText =
            LocalizationManager.GetTranslation(Constants.i2_term_reward);
        float 
            winnerItemRotation,
            itemAngle = 360 / rouletteItems.Length;

        yield return new WaitForSeconds(.1f);

        while (!IsStop) {
            /*for (int i = 0; i < rouletteItems.Length; i++) {
                rouletteItems[i].transform.rotation = Quaternion.identity;
            }*/

            yield return null;
        }

        winnerItemRotation = 
            (pivot.transform.rotation.eulerAngles.z + (itemAngle/2)) % 360;

        winnerItem = 
            rouletteItems[(int)(winnerItemRotation / itemAngle)];

        rewardMessage.text = $"{rewardText} X {winnerItem.Reward}";

        audioManager.StopAllEffects();
        audioManager.PlayEffect(winSound);

        rewardBox.SetActive(true);

        yield return new WaitForSeconds(2f);

        OnEndSpinRoulette.Invoke(winnerItem.Reward);
    }
    #endregion
}
