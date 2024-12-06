using UnityEngine;
using TMPro;
 
/// <summary>
/// A 3D money info box.
/// </summary>
public class MoneyBox3D : MonoBehaviour {
    #region Readonly fields
    private readonly string show_money_anim = "UpAndFadeShow";
    #endregion

    #region Serialize fields
    [SerializeField]
    private TextMeshPro moneyAmountText;
    [SerializeField]
    private Animator animator;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    /// <summary>
    /// Shows the amount of the player's money.
    /// </summary>
    /// <param name="amount">The playerMoney</param>
    public void ShowMoneyAmount(int amount) {
        if (amount > 0) {
            moneyAmountText.text = amount.ToString();

            animator.Play(show_money_anim);
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