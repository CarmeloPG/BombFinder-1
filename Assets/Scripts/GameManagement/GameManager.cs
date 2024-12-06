using UnityEngine;
using UnityEngine.Events;
using CrazyGames;
using TMPro;
using System;
using CarterGames.Assets.LeaderboardManager;
using CarterGames.Common.Serializiation;
using CarterGames.Assets.LeaderboardManager.Serialization;

/// <summary>
/// Manage general game data.
///    
/// Save game state to persistent storage.
/// Load game state upon startup. 
/// Laises an event when the game state loads.
/// </summary>
public class GameManager : MonoBehaviour {
    #region Readonly fileds
    private readonly string
        //The key name of the string with the last status saved.
        game_data_key = "GameStatus",
        //The names of the loaderboards.
        begginer_leaderboard = "BegginerLeaderboard",
        normal_leaderboard = "NormalLeaderboard",
        expert_leaderboard = "ExpertLeaderboard";

    
    #endregion

    #region Serialize fields
    [SerializeField]
    private ObjectsManager objectsManager;
    [SerializeField]
    private ScreenManager screenManager;
    [SerializeField]
    private AdsManager adsManager;
    //[SerializeField]
    //private int rewardMultiplier = 2;
    [SerializeField]
    private TextMeshProUGUI debugText;
    #endregion

    #region Private fields
    private GameStatus gameStatus;
    private PortalUser userData;
    private SerializableDictionary<string, Leaderboard> loaderboards;
    #endregion

    #region Properties
    public bool MusicMuted {
        get => gameStatus.musicMuted;
        set {
            gameStatus.musicMuted = value;

            SaveGame();
        }
    }

    public bool EffectsMuted {
        get => gameStatus.effectMuted;
        set {
            gameStatus.effectMuted = value;

            SaveGame();
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("Sends the loaded data.")]
    public UnityEvent<GameStatus> OnLoadGameStatus;
    [Tooltip("When the player can multiply the reward, it sends an event")]
    public UnityEvent OnMultiplyReward;
    [Tooltip("When the player can continue playing a failed level, it sends an event")]
    public UnityEvent OnContinuePlay;
    #endregion

    /// <summary>
    /// On awake it loads the last game status saved.
    /// </summary>
    #region Unity methods
    private void Awake() {
        if (CrazySDK.IsAvailable) {
            CrazySDK.Init(() => {
                if (CrazySDK.IsInitialized) {
                    LoadGameStatus(true);

                    adsManager.InitializeAds();
                }
                else {
                    LoadGameStatus(false);
                }
            });
        }
        else {
            LoadGameStatus(false);
        }
    }

    /*private void OnDestroy() {
        SaveGame();
    }*/
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

    /*public void ClearLeaderBoards() {
        LeaderboardManager.ClearAllLeaderboards();
    }*/

    /// <summary>
    /// It adds the time of the player to the leaderboard.
    /// </summary>
    public void FinishClassic(LevelReport levelReport) {
        if (levelReport.levelSuccess) {
            UpdateLeaderboard(
                FindLeaderboard(levelReport.levelPlayed),
                levelReport);
        }

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

    public void RequestContinuePlay() {
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
    }

    public void GoToLink(string link) {
        Application.OpenURL(link);
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
        gameStatus.airView = levelReport.airView;
        objectsManager.SelectedPlayer = levelReport.player;

        //objectsManager.GiveRewardToPlayer(levelReport.totalReward);

        SaveGame();
    }

    /// <summary>
    /// It loads all leader boards. If any leaderbard 
    /// doesn't exists, it creates the leaderboard.
    /// </summary>
    private void LoadLeaderboards() {
        if (!LeaderboardManager.BoardExists(begginer_leaderboard)) {
            LeaderboardManager.CreateLeaderboard(
                begginer_leaderboard,
                LeaderboardType.Time);
        }

        if (!LeaderboardManager.BoardExists(normal_leaderboard)) {
            LeaderboardManager.CreateLeaderboard(
                normal_leaderboard,
                LeaderboardType.Time);
        }

        if (!LeaderboardManager.BoardExists(expert_leaderboard)) {
            LeaderboardManager.CreateLeaderboard(
                expert_leaderboard,
                LeaderboardType.Time);
        }

        loaderboards = LeaderboardManager.AllLeaderboards;
    }

    /// <summary>
    /// It updates a leaderboard with the result of a classic game.
    /// </summary>
    /// <param name="leaderboard">The leaderboard.</param>
    /// <param name="levelReport">The report of the result of the game.</param>

    public void UpdateLeaderboard(Leaderboard leaderboard, LevelReport levelReport) {
        LeaderboardEntry playerEntry;

        playerEntry = leaderboard.GetEntry(userData.username);

        if (playerEntry == null) {
            playerEntry = new LeaderboardEntryTime(
                userData.username, 
                SerializableTime.FromSeconds(levelReport.timeReward));
            
            playerEntry.EntryUuid = userData.username;

            leaderboard.AddEntry(playerEntry);
        }
        else {
            playerEntry.EntryValue = 
                SerializableTime.FromSeconds(levelReport.timeReward);
        }
    }

    /// <summary>
    /// Finds and return the leaderboard corresponding to
    /// the clasification of a field with a size.
    /// </summary>
    /// <param name="tilesBySide">The size of the field.</param>
    /// <returns>Null if the leaderboard doesn't exists.</returns>
    private Leaderboard FindLeaderboard(int tilesBySide) {
        Leaderboard leaderboard = null;

        switch (tilesBySide) {
            case Constants.begginer_tiles_by_side:
                leaderboard = LeaderboardManager.
                    GetLeaderboard(begginer_leaderboard);

                break;
            case Constants.normal_tiles_by_side:
                leaderboard = LeaderboardManager.
                    GetLeaderboard(normal_leaderboard);

                break;
            case Constants.expert_tiles_by_side:
                leaderboard = LeaderboardManager.
                    GetLeaderboard(expert_leaderboard);

                break;
        }

        return leaderboard;
    }

    /// <summary>
    /// It loads the last game status saved on persistent storage.
    /// </summary>
    private void LoadGameStatus(bool crazySDKInitialized) {
        string data = crazySDKInitialized ?
                CrazySDK.Data.GetString(game_data_key, string.Empty) :
                PlayerPrefs.GetString(game_data_key, string.Empty);

        gameStatus = new GameStatus();

        if (string.IsNullOrEmpty(data)) {
            SaveGame();
            //debugText.text += "New game\n";
        }
        else {
            JsonUtility.FromJsonOverwrite(data, gameStatus);
            //debugText.text += "Old game\n";
        }

        //debugText.text += "Game data loaded.\n";
        
        if (crazySDKInitialized) {
            if (CrazySDK.User.IsUserAccountAvailable) {
                CheckLoggedInUser();
            }
            else {
                gameStatus.isLoggedIn = false;

                OnLoadGameStatus.Invoke(gameStatus);
            }
        }
        else {
            gameStatus.isLoggedIn = false;

            OnLoadGameStatus.Invoke(gameStatus);
        }

        //Some classes can update the game status.
        SaveGame();
    }

    /// <summary>
    /// It saves the game status to persitent storage.
    /// </summary>
    private void SaveGame() {
        string stringGameStatus = JsonUtility.ToJson(gameStatus);

        CrazySDK.Data.SetString(game_data_key, stringGameStatus);
        CrazySDK.User.SyncUnityGameData();

        PlayerPrefs.SetString(game_data_key, stringGameStatus);

        LeaderboardManager.Save();
    }

    /// <summary>
    /// It checks if the user is logged in Crazy Games and
    /// updates the game status.
    /// </summary>
    private void CheckLoggedInUser() {
        CrazySDK.User.GetUser(user => {
            userData = user;
            gameStatus.isLoggedIn = user != null;

            if (user == null) {
                CrazySDK.User.AddAuthListener((newUser) => {
                    userData = newUser;
                    gameStatus.isLoggedIn = true;

                    LoadLeaderboards();

                    OnLoadGameStatus.Invoke(gameStatus);
                });
                
            }
            else {
                LoadLeaderboards();

                OnLoadGameStatus.Invoke(gameStatus);
            }
        });
    }

    /// <summary>
    /// It sends an event to contine playing when the user
    /// has view an adversiting to continue the play after
    /// failed a level.
    /// </summary>
    private void ContinuePlay() {
        adsManager.OnEarnReward.RemoveListener(ContinuePlay);

        OnContinuePlay.Invoke();
    }

    /// <summary>
    /// It sends an event to the palyer multiply the reward
    /// obtanied palying a level.
    /// </summary>
    private void MultiplyReward() {
        adsManager.OnEarnReward.RemoveListener(MultiplyReward);

        OnMultiplyReward.Invoke();
    }

    /// <summary>
    /// It sends the power up required.
    /// </summary>
    /// <param name="powerUpPrefab">The power up required.</param>
    private void GetPowerUp(PlayObject powerUpPrefab) {
        adsManager.OnEarnReward.RemoveListener(delegate {
            GetPowerUp(powerUpPrefab);
        });

        objectsManager.SendPlayObject(Instantiate(powerUpPrefab));
    }
    #endregion

    #region Coroutines
    #endregion
}