using System;
//using UnityEditor.Media;
using UnityEngine;

public class ScreenManager : MonoBehaviour {
    #region Serialize fields
    //[SerializeField]
    //private float changeSceneDelay = 2f;
    [SerializeField]
    private GameScreen mainScreen;
    [SerializeField]
    private ArcadeScreen arcadeScreen;
    [SerializeField]
    private ClassicScreen classicScreen;
    [SerializeField]
    private ChallengeScreen challengeScreen;
    #endregion

    #region Private fields
    //private PlayObject selectedPlayer;
	//private PlayScreen selectedPlayScreen;
	#endregion

	#region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /*public void Awake() {
        selectedPlayScreen = arcadeScreen[0];
    }*/
    #endregion

    #region Public methods
    /// <summary>
    /// It loads the last game satus saved.
    /// </summary>
    /// <param name="status">The las game status saved.</param>
    public void LoadStatus(GameStatus status) {
        GameStatus.ChallengeData challengeData = status.challengeData;
        DateTime now = DateTime.Now;

        /*arcadeScreen.AirView = status.airView;
        challengeScreen.AirView = status.airView;
        classicScreen.AirView = status.airView;*/

        if (now.Day > challengeData.day ||
            now.Month > challengeData.month ||
            now.Year > challengeData.year) 
        {
            SetDailyChallenge(challengeData);
        }

        challengeScreen.ChallengeData = challengeData;
    }

    /// <summary>
    /// Starts the arcade mode.
    /// </summary>
    /// <param name="player">The player avatar.</param>
    /// <param name="level">The game level.</param>
    public void PlayArcade(PlayObject player, int level) {
        arcadeScreen.CurrentLevel = level;

        ShowScreen(arcadeScreen.ScreenName);

        arcadeScreen.Play(player);
    }

    /// <summary>
    /// Starts a play with a number of tiles by side.
    /// </summary>
    /// <param name="player">The player avatar.</param>
    public void PlayClassic(PlayObject player) {
        //classicScreen.TilesBySide = tilesBySide;

        ShowScreen(classicScreen.ScreenName);

        classicScreen.Play(player);
    }

    /// <summary>
    /// It starts the play in challenge mode.
    /// </summary>
    /// <param name="player">The player avatar.</param>
    public void PlayChallenge(PlayObject player) {
        ShowScreen(challengeScreen.ScreenName);

        challengeScreen.Play(player);
    }

    /// <summary>
    /// It hides all active play screens and shows the main screen.
    /// </summary>
    public void ShowMainScreen() {
        ShowScreen(mainScreen.ScreenName);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// It shows the screen named screenName.
    /// </summary>
    /// <param name="screenName">The name of the screen to show.</param>
    private void ShowScreen(string screenName) {
        mainScreen.Show(mainScreen.ScreenName.Equals(screenName));
        arcadeScreen.Show(arcadeScreen.ScreenName.Equals(screenName));
        classicScreen.Show(classicScreen.ScreenName.Equals(screenName));
        challengeScreen.
            Show(challengeScreen.ScreenName.Equals(screenName));
    }

    /// <summary>
    /// It sets the new challenge and updates the challengeData.
    /// </summary>
    /// <param name="challengeData"></param>
    private void SetDailyChallenge(GameStatus.ChallengeData challengeData) {
        DateTime now = DateTime.Now;
        int timeIndex = UnityEngine.Random.Range(0, 3);
        int tilesBySide = UnityEngine.Random.Range(
            challengeScreen.MinTilesBySide,
            challengeScreen.MaxTilesBySide + 1);

        float bombsDensity = UnityEngine.Random.Range(
            challengeScreen.MinBombDensity, 
            challengeScreen.MaxBombDensity);

        int dificultyIndex = 
            Mathf.FloorToInt(tilesBySide * bombsDensity);

        challengeData.day = now.Day;
        challengeData.month = now.Month;
        challengeData.year = now.Year;
        challengeData.overcome = false;
        challengeData.tilesBySide = tilesBySide;
        challengeData.dificultyIndex = dificultyIndex;
        challengeData.bombs = dificultyIndex * tilesBySide;

        switch (timeIndex) {
            case 0:
                challengeData.time = challengeScreen.ShortTime;
                break;
            case 1:
                challengeData.time = challengeScreen.MediumTime;
                break;
            case 2:
                challengeData.time = challengeScreen.LongTime;
                break;
        }
    }
    #endregion

    #region Coroutines
    #endregion
}