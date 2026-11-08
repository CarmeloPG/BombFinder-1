using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// For right work, all game objects in the EndLevelInfoBox prefabs must be disabled.
/// </summary>
public class EndLevelInfoBox : MonoBehaviour {
    #region Readonly fields
    //private readonly string show_animation_key = "Show";
    #endregion

    #region Serialize fields
    /*[SerializeField]
    private TextMeshProUGUI labelSuccess;
    [SerializeField]
    private TextMeshProUGUI labelFailed;*/
    [SerializeField]
	private Button viewVideoButton;
    [SerializeField]
    private Button skipShowRewardButton;
    [SerializeField]
    private GameObject playerMoneyInfoBox;
    [SerializeField]
    private GameObject normalViewButton;
    [SerializeField]
    private GameObject airViewButton;
    [SerializeField]
    private AudioClip moneySound;

    [Header("Money info boxes")]
    [SerializeField]
	private MoneyInfoBox timeReward;
    [SerializeField]
    private MoneyInfoBox moneyCollected;
    [SerializeField]
	private MoneyInfoBox levelEndReward;
	[SerializeField]
	private MoneyInfoBox totalReward;

    [Header("End level animation")]
    [SerializeField]
    private Animator endLevelAnimator;
    [SerializeField]
    private AnimationClip showEndLevelButtons;
    [SerializeField]
    private AnimationClip showEndLevelReward;
    [SerializeField]
    private AnimationClip showMoneyCollectedReward;
    [SerializeField]
    private AnimationClip showLabelSuccess;
    [SerializeField]
    private AnimationClip showTimeReward;
    [SerializeField]
    private AnimationClip showLabelFalied;
    [SerializeField]
    private AnimationClip showTotalReward;
    [SerializeField]
    private AnimationClip getReward;
    #endregion

    #region Private fields
    //private LevelReport levelReport;
    private int reward;
    private AudioManager audioManager;
    private bool isActiveShowReward;
	#endregion

	#region Properties
	/*public LevelReport LevelReport {
		get => levelReport;
	}*/

    public int Reward {
        get => reward;
        private set {
            reward = value;

            totalReward.ShowMoneyAmount(MoneyType.GameMoney, reward);
        }
    }

    public bool AirView {
        set {
            airViewButton.SetActive(!value);
            normalViewButton.SetActive(value);
            playerMoneyInfoBox.SetActive(!value);
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When the player gets the reward, it sends the reward.")]
    public UnityEvent OnGetReward;
    #endregion

    #region Unity methods
    /// <summary>
    /// 
    /// </summary>
    private void Awake() {
        audioManager = AudioManager.Instance;
    }

    /// <summary>
    /// 
    /// </summary>
    /*private void OnEnable() {
        SkipShowReward(false);
    }*/
    #endregion

    #region Public methods
    /// <summary>
    /// It shows the information of a level report.
    /// </summary>
    /// <param name="report">The level report.</param>
    public void ShowInfo(LevelReport report) {
        //levelReport = report;

        if (gameObject.activeSelf) {
            Reward = report.totalReward;
        }
        else {
            gameObject.SetActive(true);

            if (report.levelSuccess) {
                viewVideoButton.interactable = true;

                endLevelAnimator.Play(showLabelSuccess.name);
            }
            else {
                viewVideoButton.interactable = false;

                endLevelAnimator.Play(showLabelFalied.name);
            }

            /*moneyCollected.ShowMoneyAmount(
                MoneyType.GameMoney, report.moneyCollected);

            timeReward.ShowMoneyAmount(
                MoneyType.GameMoney, report.timeReward);

            levelEndReward.ShowMoneyAmount(
                MoneyType.GameMoney, report.levelEndReward);*/

            //audioManager.PlayEffect(moneySound);

            if (report.totalReward > 0) {
                StartCoroutine(ShowReward(report));
            }
            else {
                endLevelAnimator.Play(showEndLevelButtons.name);
            }
        }
    }

    /// <summary>
    /// It clears all the information of the box.
    /// </summary>
    public void ClearInfo() {
        Reward = 0;

        moneyCollected.ShowMoneyAmount(MoneyType.GameMoney, 0);
        levelEndReward.ShowMoneyAmount(MoneyType.GameMoney, 0);
        timeReward.ShowMoneyAmount(MoneyType.GameMoney, 0);
        totalReward.ShowMoneyAmount(MoneyType.GameMoney, 0);

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void GetReward() {
        audioManager.PlayEffect(moneySound);
        OnGetReward.Invoke();
    }

    /// <summary>
    /// 
    /// </summary>
    public void SkipShowReward(bool skip) {
        isActiveShowReward = !skip;

        skipShowRewardButton.gameObject.SetActive(!skip);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowReward(LevelReport report) {
        float waitTime = 1f;
        int 
            currentReward = report.levelEndReward,
            deltaReward;

        isActiveShowReward = true;
        Reward = 0;

        yield return new WaitForSeconds(waitTime);

        if (currentReward > 0) {
            //Level is completed successfully, show the level end reward.
            deltaReward = Mathf.CeilToInt(currentReward * Time.deltaTime / 2);

            audioManager.PlayEffect(moneySound);
            endLevelAnimator.Play(showEndLevelReward.name);
            levelEndReward.ShowMoneyAmount(MoneyType.GameMoney, currentReward);

            yield return new WaitForSeconds(waitTime);

            while ((currentReward > deltaReward) && isActiveShowReward) {
                currentReward -= deltaReward;

                Reward = reward + deltaReward;

                levelEndReward.
                    ShowMoneyAmount(MoneyType.GameMoney, currentReward);

                yield return null;
            }

            Reward = reward + currentReward;

            audioManager.PlayEffect(moneySound);
            levelEndReward.ShowMoneyAmount(MoneyType.GameMoney, 0);
            
            if (isActiveShowReward) {
                yield return new WaitForSeconds(waitTime);
            }

            //labelSuccess.enabled = false;
        }
        /*else {
            //Level is failed.
            yield return new WaitForSeconds(waitTime);

            //labelFailed.enabled = false;
        }*/

        //Show money collected reward
        currentReward = report.moneyCollected;

        if (currentReward > 0) {
            deltaReward = Mathf.CeilToInt(currentReward * Time.deltaTime / 2);

            endLevelAnimator.Play(showMoneyCollectedReward.name);
            audioManager.PlayEffect(moneySound);
            moneyCollected.
                ShowMoneyAmount(MoneyType.GameMoney, currentReward);

            if (isActiveShowReward) {
                yield return new WaitForSeconds(waitTime);
            }

            while ((currentReward > deltaReward) && isActiveShowReward) {
                currentReward -= deltaReward;

                Reward = reward + deltaReward;

                moneyCollected.
                    ShowMoneyAmount(MoneyType.GameMoney, currentReward);

                yield return null;
            }

            Reward = reward + currentReward;

            audioManager.PlayEffect(moneySound);
            moneyCollected.ShowMoneyAmount(MoneyType.GameMoney, 0);

            if (isActiveShowReward) {
                yield return new WaitForSeconds(waitTime);
            }

            //endLevelAnimator.Play(showLabelSuccess.name);
            audioManager.PlayEffect(moneySound);
        }

        //Show time reward
        currentReward = report.timeReward;

        if (currentReward > 0) {
            if (isActiveShowReward) {
                yield return new WaitForSeconds(waitTime);
            }

            currentReward = report.timeReward;
            deltaReward = Mathf.CeilToInt(currentReward * Time.deltaTime / 2);

            endLevelAnimator.Play(showTimeReward.name);
            audioManager.PlayEffect(moneySound);
            timeReward.
                ShowMoneyAmount(MoneyType.GameMoney, currentReward);

            if (isActiveShowReward) {
                yield return new WaitForSeconds(waitTime);
            }

            while ((currentReward > deltaReward) && isActiveShowReward) {
                currentReward -= deltaReward;

                Reward = reward + deltaReward;

                timeReward
                    .ShowMoneyAmount(MoneyType.GameMoney, currentReward);

                yield return null;
            }

            Reward = reward + currentReward;

            audioManager.PlayEffect(moneySound);
            timeReward.ShowMoneyAmount(MoneyType.GameMoney, 0);

            if (isActiveShowReward) {
                yield return new WaitForSeconds(waitTime);
            }

            //endLevelAnimator.Play(showLabelFalied.name);
        }

        //Show total reward
        //skipShowRewardButton.gameObject.SetActive(false);

        if (reward > 0) {
            endLevelAnimator.Play(showTotalReward.name);
        }
        else {
            yield return new WaitForSeconds(waitTime);

            endLevelAnimator.Play(showEndLevelButtons.name);
        }
    }
	#endregion
}