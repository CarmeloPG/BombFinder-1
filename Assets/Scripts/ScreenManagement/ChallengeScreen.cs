using UnityEngine;
using System.Collections;


/// <summary>
/// In the challenge the player has limited timeReward to find all bombs.
/// 
/// The challenge selects the tiles by side, the bombs density and
/// the timeReward ramdoly.
/// 
/// The challenge can have 3 dificulty levels:
///     1. Easy.
///     2. Normal.
///     3. Hard.
/// 
/// The challenge screen does some changes on PlayScreen, so the 
/// player can play the daily challenge every timeReward as the player
/// needs to overcome the challenge.
/// </summary>
public class ChallengeScreen : PlayScreen {
    #region Readonly fields
    private readonly string
        easyDificulty = Constants.i2_term_easy,
        normalDificulty = Constants.i2_term_normal,
        hardDificulty = Constants.i2_term_hard;
    #endregion

    #region Serialize fields
    [Header("Posible times")]
    [SerializeField]
    private int shortTime = 180;
    [SerializeField]
    private int mediumTime = 360;
    [SerializeField]
    private int longTime = 540;

    [Header("Hurry up settings")]
    [SerializeField]
    [Tooltip("When the remaining time is lower hurryUpTime, the player will be informed.")]
    private int hurryUpTime;
    [SerializeField]
    private AudioClip hurryUpMusic;
    //[SerializeField]
    //private FloatMessage hurryUpMessage;
    #endregion

    #region Private fields
    //The dificulty index is used to get the dificulty of the challenge and is the multiplier of the success reward.
    private int dificultyIndex;
    //private int time;
    //The chanllenge dificulti will be used to set the reward.
    private string challengeDificulty;
    private GameStatus.ChallengeData challengeData;
    private AudioManager audioManager;
    #endregion

    #region Properties
    public GameStatus.ChallengeData ChallengeData {
        set => challengeData = value;
    }

    public int MinTilesBySide {
        get => minTilesBySide;
    }

    public int MaxTilesBySide {
        get => maxTilesBySide;
    }

    public float MinBombDensity {
        get => minBombDensity;
    }

    public float MaxBombDensity {
        get => maxBombDensity;
    }

    public int ShortTime {
        get => shortTime;
    }

    public int MediumTime {
        get => mediumTime;
    }

    public int LongTime {
        get => longTime;
    }

    public int Time {
        get => time;
    }

    public string ChallengeDificulty {
        get => challengeDificulty;
    }
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /// <summary>
    /// It sets the audio manager.
    /// </summary>
    protected override void Awake() {
        base.Awake();

        audioManager = AudioManager.Instance;
    }
    #endregion

    #region Public methods
    /// <summary>
    /// It selects the tiles by side, the bombs density and
    /// the timeReward ramdoly, then start the play.
    /// </summary>
    /// <param name="thePlayer"></param>
    public override void Play(PlayObject thePlayer) {
        base.Play(thePlayer);

        SetChallenge();

        country = new Tile[tilesBySide, tilesBySide];
        //pause = false;

        startlevelInfoBox.ShowInfo(this);

        StartCoroutine(SetPlayScreen());
    }

    /// <summary>
    /// It finish the play and updates the level report.
    /// </summary>
    /// <param name="levelSuccess"></param>
    public override void EndPlay(bool levelSuccess) {
        base.EndPlay(levelSuccess);
        
        if (levelSuccess) {
            int timeReward = time * dificultyIndex;

            levelReport.moneyCollected *= dificultyIndex;
            levelReport.levelEndReward *= dificultyIndex;
            levelReport.totalReward *= dificultyIndex;
            levelReport.timeReward = timeReward;
            levelReport.totalReward += timeReward;

            replayLevelButton.interactable = false;

            endlevelInfoBox.ClearInfo();
            endlevelInfoBox.ShowInfo(levelReport);
        }
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// Sets the data of the current challenge.
    /// </summary>
    /// <param name="challengeData">The data.</param>
    private void SetChallenge() {
        dificultyIndex = challengeData.dificultyIndex;
        time = challengeData.time;

        TilesBySide = challengeData.tilesBySide;
        BombsCount = challengeData.bombs;

        if (dificultyIndex < 4) {
            if (time == shortTime) {
                challengeDificulty = normalDificulty;
            }
            else {
                time = mediumTime;
                challengeDificulty = easyDificulty;
            }
        }
        else if (dificultyIndex < 7) {
            if (time == shortTime) {
                challengeDificulty = hardDificulty;
            }
            else if (time == mediumTime) {
                challengeDificulty = normalDificulty;
            }
            else {
                challengeDificulty = easyDificulty;
            }
        }
        else {
            if (time == longTime) {
                challengeDificulty = normalDificulty;
            }
            else {
                time = mediumTime;
                challengeDificulty = hardDificulty;
            }
        }
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// The contdown of the challenge.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator Play() {
        do {
            if (!pause) {
               CheckPlayerPosition();

                timeDisplay.ShowTime(time);

                time--;

                if (time == hurryUpTime) {
                    audioManager.StopMusic();
                    audioManager.PlayMusic(hurryUpMusic);

                    //hurryUpMessage.ShowMessage(hurryUpTime.ToString());
                }
            }

            yield return new WaitForSeconds(1f);
        } while (time >= 0);

        //EndPlay(false);
        CheckContinues();
    }
    #endregion
}