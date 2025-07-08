using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Manage general game data.
///    
/// Save game state to persistent storage.
/// Load game state upon startup. 
/// Laises an event when the game state loads.
/// </summary>
public class GameManager : MonoBehaviour {
    #region Readonly fileds
    //The key name of the string with the last status saved.
    private readonly string game_data_key = "GameStatus";
    #endregion

    #region Serialize fields
    [SerializeField]
    private ObjectsManager objectsManager;
    [SerializeField]
    private ScreenManager screenManager;
    [SerializeField]
    private TextMeshProUGUI debugText;
    #endregion

    #region Private fields
    private GameStatus gameStatus;
    #endregion

    #region Properties
    public bool IsMusicMuted {
        get => gameStatus.isMusicMuted;
        set {
            gameStatus.isMusicMuted = value;

            SaveGame();
        }
    }

    public bool IsEffectsMuted {
        get => gameStatus.isEffectsMuted;
        set {
            gameStatus.isEffectsMuted = value;

            SaveGame();
        }
    }

    public float MusicVolume {
        get => gameStatus.musicVolume;
        set {
            gameStatus.musicVolume = value;

            SaveGame();
        }
    }

    public float EffectsVolume {
        get => gameStatus.effectsVolume;
        set {
            gameStatus.effectsVolume = value;

            SaveGame();
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Sends the loaded data.")]
    public UnityEvent<GameStatus> OnLoadGameStatus;
    /*[Tooltip("When the player can multiply the reward, it sends an event")]
    public UnityEvent OnMultiplyReward;
    [Tooltip("When the player can continue playing a failed level, it sends an event")]
    public UnityEvent OnContinuePlay;*/
    #endregion

    #region Unity methods
    /// <summary>
    /// On awake, it loads the last saved status.
    /// </summary>
    private void Awake() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        LoadGameStatus();
    }
    #endregion

    #region Public methods
    public void PlayCurrentLevel() {
        screenManager.PlayArcade(
            objectsManager.SelectedPlayer,
            gameStatus.currentLevel);
    }

    /// <summary>
    /// It starts the game in challenge mode.
    /// </summary>
    public void PlayChallenge() {
        screenManager.PlayChallenge(objectsManager.SelectedPlayer);
    }


    /// <summary>
    /// It starts the classic mode with tilesBySide.
    /// The classic mode is a competition mode. The player
    /// with best time win.
    /// </summary>
    public void PlayClassic() {
        screenManager.PlayClassic(objectsManager.SelectedPlayer);
    }

    /// <summary>
    /// It ends the current level.
    /// </summary>
    /// <param name="levelReport">The report of the level result.</param>
    public void FinishLevel(LevelReport levelReport) {
        if (levelReport.levelSuccess) {
            gameStatus.currentLevel = 
                levelReport.levelPlayed + 1;
        }
        
        FinishPlay(levelReport);
    }


    /// <summary>
    /// It ends the challenge play mode.
    /// </summary>
    /// <param name="levelReport">The report of the level result.</param>
    public void FinishChallenge(LevelReport levelReport) {
        if (levelReport.levelSuccess) {
            gameStatus.challengeData.overcome = true;
            //gameStatus.SetChallengeDate(DateTime.UtcNow);
        }

        FinishPlay(levelReport);
    }

    /// <summary>
    /// It adds the time of the player to the leaderboard.
    /// </summary>
    public void FinishClassic(LevelReport levelReport) {
        FinishPlay(levelReport);
    }

    
    public void BuyPlayObject(PlayObjectData playObjectData) {
        gameStatus.playerInventory.Add(playObjectData.Identifier);

        SaveGame();
    }

    public void EquipPlayObject(PlayObjectData playObjectData) {
        gameStatus.equipedObject = playObjectData.Identifier;

        SaveGame();
    }

    public void UpdateMoney(MoneyType moneyType, int newAmount) {
        gameStatus.playerMoney = newAmount;

        SaveGame();
    }

    /*public void RequestContinuePlay() {
        adsManager.OnEarnReward.AddListener(ContinuePlay);
        adsManager.ShowRewardedAd();
    }

    public void RequestMultiplyReward() {
        adsManager.OnEarnReward.AddListener(MultiplyReward);

        adsManager.ShowRewardedAd();
    }

    public void RequestPowerUp(PlayObject powerUpPrefab) {
        adsManager.OnEarnReward.AddListener( delegate {
            GetPowerUp(powerUpPrefab);
        });

        adsManager.ShowRewardedAd();
    }*/

    public void GoToLink(string link) {
        Application.OpenURL(link);
    }

    public void Quit() {
        Application.Quit();
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// It ends the play and save the game status.
    /// </summary>
    /// <param name="levelReport">The level report.</param>
    private void FinishPlay(LevelReport levelReport) {
        //gameStatus.airView = levelReport.airView;
        objectsManager.SelectedPlayer = levelReport.player;

        //objectsManager.GiveRewardToPlayer(levelReport.totalReward);

        SaveGame();
    }

    /// <summary>
    /// It loads the last game status saved on persistent storage.
    /// </summary>
    private void LoadGameStatus() {
        string data = PlayerPrefs.GetString(game_data_key, string.Empty);

        gameStatus = new GameStatus();

        if (string.IsNullOrEmpty(data)) {
            SaveGame();
            //debugText.text += "New game\n";
        }
        else {
            JsonUtility.FromJsonOverwrite(data, gameStatus);
            //debugText.text += "Old game\n";
        }

        OnLoadGameStatus.Invoke(gameStatus);
        //Some classes can update the game status.
        SaveGame();
    }

    /// <summary>
    /// It saves the game status to persitent storage.
    /// </summary>
    private void SaveGame() {
        string stringGameStatus = JsonUtility.ToJson(gameStatus);

        PlayerPrefs.SetString(game_data_key, stringGameStatus);
    }

    /// <summary>
    /// It checks if the user is logged in Crazy Games and
    /// updates the game status.
    /// </summary>
    /*private void CheckLoggedInUser() {
        CrazySDK.User.GetUser(user => {
            userData = user;
            //gameStatus.isLoggedIn = user != null;

            if (user == null) {
                gameStatus.isLoggedIn = false;

                CrazySDK.User.AddAuthListener((newUser) => {
                    userData = newUser;
                    gameStatus.isLoggedIn = true;

                    LoadLeaderboards();

                    OnLoadGameStatus.Invoke(gameStatus);
                });
            }
            else {
                gameStatus.isLoggedIn = true;
                
                LoadLeaderboards();

                //OnLoadGameStatus.Invoke(gameStatus);
            }

            OnLoadGameStatus.Invoke(gameStatus);
        });
    }*/
    #endregion

    #region Coroutines
    #endregion
}