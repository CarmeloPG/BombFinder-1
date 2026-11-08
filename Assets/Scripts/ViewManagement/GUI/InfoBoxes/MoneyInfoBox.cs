using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

/// <summary>
/// An UI element witch shows the data of a type of Money. The type of
/// the playerMoney can't be changed, sonly can be changed the amount of playerMoney.
/// </summary>
public class MoneyInfoBox : MonoBehaviour {

    #region Readonly fileds
    #endregion

    #region Serialize fields
    [SerializeField]
    [Tooltip("Definiton of Money witch its data is showed.")]
    private MoneyDefinition moneyDefinition;
    [SerializeField]
    private TextMeshProUGUI moneyAmountText;
    [SerializeField]
    private Image moneyIconImage;

    /*[Header("Aniamtion options")]
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private AnimationClip moneyAnimation;*/
    #endregion

    #region Private fields
    private int moneyAmount;
    private AudioManager audioManager;
    #endregion

    #region Properties
    public MoneyType MoneyType {
        get => moneyDefinition.MoneyType;
    }

    public Sprite MoneyIcon {
        get => moneyDefinition.Icon;
    }
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    private void Awake() {
        audioManager = AudioManager.Instance;
        moneyIconImage.sprite = moneyDefinition.Icon;
    }

    private void OnEnable() {
        ShowMoneyAmount(moneyDefinition.MoneyType, moneyAmount);
        /*if (moneyAmount > 1000) {
            ShowMoneyAmount(moneyDefinition.MoneyType, moneyAmount);
        }
        else {
            ShowMoneyAmount(moneyDefinition.MoneyType, moneyAmount);
        }*/
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Shows the amount of a playerMoney.
    /// </summary>
    /// <param name="money">The playerMoney</param>
    public void ShowMoneyAmount(Money money) {
        ShowMoneyAmount(money.MoneyType, money.Amount);
	}

    /// <summary>
    /// Shows the amount of a MoneyType. 
    /// It only show the amoun if the 
    /// MonyeType is equals to MoneyType of this info box.
    /// </summary>
    /// <param name="moneyType">The type of playerMoney.</param>
    /// <param name="amount">The amount.</param>

    public void ShowMoneyAmount(MoneyType moneyType, int amount) {
        if (moneyDefinition.MoneyType == moneyType) {
            moneyAmount = amount;
            moneyAmountText.text = 
                amount.ToString("N0", LocalizationManager.CurrentCulture);

            /*if (gameObject.activeInHierarchy) {
                animator.Play(moneyAnimation.name);
            }*/

            /*if (gameObject.activeInHierarchy && audioManager != null) {
                audioManager.PlayEffect(moneyDefinition.Sound);
            }*/
        }
        else {
            moneyAmountText.text = "Error MoneyType";

            Debug.LogError($"The MoneyType of this box is {moneyDefinition.MoneyType}, your MoneyType is {moneyType}");
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