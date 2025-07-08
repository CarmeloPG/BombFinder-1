using UnityEngine;
using UnityEngine.UI;
using TMPro;
using I2.Loc;

public class StartlevelInfoBox : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    private TextMeshProUGUI numberLevelText;
    [SerializeField]
    private Localize currentDificultyText;
    [SerializeField]
    private TextMeshProUGUI timeCounterText;
    [SerializeField]
    private TextMeshProUGUI tilesCountText;
    [SerializeField]
	private TextMeshProUGUI bombsCountText;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("Sends an alert when wait timeReward is closed")]
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    public void ShowInfo(ArcadeScreen  arcadeScreen, bool show = true) {
        numberLevelText.text = arcadeScreen.CurrentLevel.ToString();

        ShowPlayScreenInfo(arcadeScreen, show);
    }

    public void ShowInfo(ChallengeScreen challengeScreen, bool show = true) {
        currentDificultyText.Term = challengeScreen.ChallengeDificulty;
        timeCounterText.text = challengeScreen.Time.ToString();

        ShowPlayScreenInfo(challengeScreen, show);

        //gameObject.SetActive(true);
    }

    public void ShowInfo(ClassicScreen classicScreen, bool show = true) {
        ShowPlayScreenInfo(classicScreen, show);
    }

    public void ShowInfo(bool show) {
        gameObject.SetActive(show);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    private void ShowPlayScreenInfo(PlayScreen playScreen, bool show) {
        tilesCountText.text = playScreen.TilesCount.ToString();
        bombsCountText.text = playScreen.BombsCount.ToString();

        ShowInfo(show);
    }
    #endregion

    #region Coroutines
    #endregion
}