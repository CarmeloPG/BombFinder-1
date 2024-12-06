using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class EndLevelInfoBox : MonoBehaviour {
    #region Readonly fields
    //private readonly string show_animation_key = "Show";
    #endregion

    #region Serialize fields
    [SerializeField]
    private TextMeshProUGUI labelSuccess;
    [SerializeField]
    private TextMeshProUGUI labelFailed;
    [SerializeField]
	private Button viewVideoButton;
    [SerializeField]
    private GameObject endLevelButtons;
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
    private AnimationClip showMoneyCollectedReward;
    [SerializeField]
    private AnimationClip hideMoneyCollectedReward;
    [SerializeField]
    private AnimationClip showTimeReward;
    [SerializeField]
    private AnimationClip hideTimeReward;
    [SerializeField]
    private AnimationClip showTotalReward;
    [SerializeField]
    private AnimationClip getReward;
    #endregion

    #region Private fields
    private LevelReport levelReport;
    private int reward;
    private AudioManager audioManager;
	#endregion

	#region Properties
	public LevelReport LevelReport {
		get => levelReport;
	}

    public int Reward {
        get => reward;
        private set {
            reward = value;

            totalReward.ShowMoneyAmount(MoneyType.GameMoney, reward);
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When the player gets the reward, it sends the reward.")]
    public UnityEvent OnGetReward;
    #endregion

    #region Unity methods
    private void Awake() {
        audioManager = AudioManager.Instance;
    }
    #endregion

    #region Public methods
    /// <summary>
    /// It shows the information of a level report.
    /// </summary>
    /// <param name="report">The level report.</param>
    public void ShowInfo(LevelReport report) {
        //ClearInfo();

		levelReport = report;
        
        if (gameObject.activeSelf) {
            Reward = report.totalReward;
        }
        else {
            Reward = 0;
            viewVideoButton.interactable = true;

            if (report.levelSuccess) {
                labelSuccess.enabled = true;
                labelFailed.enabled = false;
            }
            else {
                labelSuccess.enabled = false;
                labelFailed.enabled = true;
            }

            moneyCollected.ShowMoneyAmount(
                MoneyType.GameMoney, levelReport.moneyCollected);

            timeReward.ShowMoneyAmount(
                MoneyType.GameMoney, levelReport.timeReward);

            levelEndReward.ShowMoneyAmount(
                MoneyType.GameMoney, levelReport.levelEndReward);

            gameObject.SetActive(true);
            audioManager.PlayEffect(moneySound);

            StartCoroutine(ShowReward());
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
    private IEnumerator ShowReward() {
        float waitTime = 1f;
        int 
            currentReward = levelReport.levelEndReward,
            deltaReward = Mathf.CeilToInt(currentReward * Time.deltaTime / 2);

        //audioManager.PlayEffect(moneySound);

        yield return new WaitForSeconds(waitTime);

        //Show level end reward
        while (currentReward > deltaReward) {
            currentReward -= deltaReward;

            Reward = reward + deltaReward;

            levelEndReward.
                ShowMoneyAmount(MoneyType.GameMoney, currentReward);

            yield return null;
        }

        Reward = reward + currentReward;

        audioManager.PlayEffect(moneySound);
        levelEndReward.ShowMoneyAmount(MoneyType.GameMoney, 0);

        yield return new WaitForSeconds(waitTime);

        //Show money collected reward
        currentReward = levelReport.moneyCollected;
        deltaReward = Mathf.CeilToInt(currentReward * Time.deltaTime / 2);

        endLevelAnimator.Play(showMoneyCollectedReward.name);
        audioManager.PlayEffect(moneySound);

        yield return new WaitForSeconds(waitTime);

        while (currentReward > deltaReward) {
            currentReward -= deltaReward;

            Reward = reward + deltaReward;

            moneyCollected.
                ShowMoneyAmount(MoneyType.GameMoney, currentReward);

            yield return null;
        }

        Reward = reward + currentReward;

        audioManager.PlayEffect(moneySound);
        moneyCollected.ShowMoneyAmount(MoneyType.GameMoney, 0);

        yield return new WaitForSeconds(waitTime);

        endLevelAnimator.Play(hideMoneyCollectedReward.name);
        audioManager.PlayEffect(moneySound);

        if (levelReport.timeReward > 0) {
            //Show time reward
            yield return new WaitForSeconds(waitTime);

            currentReward = levelReport.timeReward;
            deltaReward = Mathf.CeilToInt(currentReward * Time.deltaTime / 2);

            endLevelAnimator.Play(showTimeReward.name);
            audioManager.PlayEffect(moneySound);

            yield return new WaitForSeconds(waitTime);

            while (currentReward > deltaReward) {
                currentReward -= deltaReward;

                Reward = reward + deltaReward;

                timeReward
                    .ShowMoneyAmount(MoneyType.GameMoney, currentReward);

                yield return null;
            }

            Reward = reward + currentReward;

            audioManager.PlayEffect(moneySound);
            timeReward.ShowMoneyAmount(MoneyType.GameMoney, 0);

            yield return new WaitForSeconds(waitTime);

            endLevelAnimator.Play(hideTimeReward.name);
        }

        //Show total reward
        //yield return new WaitForSeconds(waitTime);

        endLevelAnimator.Play(showTotalReward.name);
    }
	#endregion
}