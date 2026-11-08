using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class PlayScreen : GameScreen {
    #region Readonly fields
    #endregion

    #region Serialize fields
    [SerializeField]
    protected AudioManager audioManger;
    [SerializeField]
    protected GameManager gameManager;

    [Header("Controls")]
    [SerializeField]
    protected InputObserver inputObserver;
    [SerializeField]
    private VirtualControls virtualControls;

    [Header("Rewards")]
    [SerializeField]
    protected int levelSuccessReward = 1500;

    [Header("Camera options")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private TrackingObject playerCamera;
    [SerializeField]
    private Camera airCamera;
    [SerializeField]
    private Vector3 playerCameraPosition;
    [SerializeField]
    private Vector3 playerCameraRotation;

    [Header("Satge configuration")]
    [SerializeField]
    [Tooltip("The stage of the game.")]
    protected Transform stage;
    [SerializeField]
    [Tooltip("Maximun number of times that the player can continue playing after to fail the level.")]
    private int continues = 1;
    [SerializeField]
    [Tooltip("The minimum number of tiles by side of the country.")]
    protected int minTilesBySide;
    [SerializeField]
    [Tooltip("The maximum number of tiles by side of the country.")]
    protected int maxTilesBySide;
    [SerializeField]
    [Tooltip("The minimum bombs density of the country.")]
    [Range(0.1f, .2f)]
    protected float minBombDensity;
    [SerializeField]
    [Range(.2f, .3f)]
    [Tooltip("The maximum bombs density of the country.")]
    protected float maxBombDensity;
    [SerializeField]
    [Tooltip("The size of any tile. All tiles must be the same size.")]
    private float tileSize = 1f;
    [SerializeField]
    [Tooltip("Time, in seconds, to put all the tiles on the country.")]
    protected float loadTime = 5f;
    [SerializeField]
    protected Vector3 initialPlayerPosition;
    [SerializeField]
    private Vector3 initialTilePosition;
    [SerializeField]
    [Tooltip("The prefab of the bombs.")]
    private Bomb bombPrefab;
    [Tooltip("Each play, it will select one random theme.")]
    [SerializeField]
    private TileTheme[] tileThemes;
    [Tooltip("The posible skyboxes in the scene.")]
    [SerializeField]
    private Material[] skyboxes;
    [Tooltip("The time interval, in seconds, between interstitial ads.")]
    [SerializeField]
    private float timeBetweenInterstials = 900f;

    [Header("GUI options")]
    [SerializeField]
    private Window continueWindow;
    [SerializeField]
    private Window pauseWindow;
    [SerializeField]
    protected StartlevelInfoBox startlevelInfoBox;
    [SerializeField]
    private GameObject playLevelInfoBox;
    [SerializeField]
    protected EndLevelInfoBox endlevelInfoBox;
    [SerializeField]
    private Button rewardedVideoButton;
    [SerializeField]
    protected Button replayLevelButton;
    [SerializeField]
    protected TimeDisplay timeDisplay;
    [SerializeField]
    private TextMeshProUGUI bombsCountText;
    [SerializeField]
    private Roulette roulette;

    [Header("Audio")]
    //[SerializeField]
    ///private AudioClip levelMusic;
    [SerializeField]
    private AudioClip levelSuccessSound;
    #endregion

    #region Protected fields
    protected LevelReport levelReport;
    protected Tile[,] country;
    protected PlayObject player;
    protected bool
        pause,
        airView,
        playEnded;
    protected int
        time,
        usedContinues,
        tilesBySide,
        tilesCount,
        bombsCount,
        bombsMarkCount;
    #endregion

    #region Private fields
    //private GameObject environment;
    private SimpleCharacterControl playerControls;
    private AdsManager adsManager;
    private float deltaLoadTime;
    #endregion

    #region Properties
    public int TilesCount {
        get => tilesCount;
    }

    public int BombsCount {
        get => bombsCount;
        set {
            bombsCount = value;
            bombsCountText.text = value.ToString();
        }
    }

    public bool Pause {
        get => pause;
        set {
            pause = value;

            inputObserver.IsEnabled = !pause;

            if (!Application.isMobilePlatform) {
                player.EnableControls = !pause;
            }
                        
            if (!pause && player.IsStunned) {
                player.transform.position += Vector3.up;

                player.Stun(false); 
            }
            
            mainCamera.gameObject.SetActive(pause);
            airCamera.gameObject.SetActive(!pause && airView);
            playerCamera.gameObject.SetActive(!pause && !airView);

            if (!Application.isMobilePlatform) {
                if (pause == true) {
                    Cursor.lockState = CursorLockMode.None;
                }
                else {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }
    
    public string PlayerName {
        get => player.Data.Identifier;
    }

    public Sprite PlayerIcon {
        get => player.Data.Icon;
    }

    public int TilesBySide {
        set {
            Vector3 mainCameraPosition = mainCamera.transform.position;
            float center = value / 2;

            tilesBySide = value;
            tilesCount = tilesBySide * tilesBySide;
            deltaLoadTime = loadTime / tilesCount;

            airCamera.orthographicSize = (tilesBySide / 2) + 1;
            airCamera.transform.position = 
                new Vector3(center, center, center);

            mainCamera.transform.position = 
                new Vector3(center,center,-center);
        }
    }

    public bool AirView {
        set {
            airView = value;

            airCamera.gameObject.SetActive(airView);

            if (player != null && country != null) {
                inputObserver.IsEnabled = !airView;
                playerControls.enabled = !airView;

                if (Application.isMobilePlatform) {
                    virtualControls.IsEnabled = !airView;
                }

                playerCamera.gameObject.SetActive(!airView && !playEnded);
                mainCamera.gameObject.SetActive(!airView && playEnded);

                if (!levelReport.levelSuccess) {
                    foreach (Tile tile in country) {
                        tile.AirView = value;
                    }
                }
            }
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When the play starts, it sends the report of the played level.")]
    public UnityEvent OnStartLevel;
    [Tooltip("When a tile is exposed, it sends the tile.")]
    public UnityEvent<Tile> OnExposeTile;
    [Tooltip("When a tile is marked or dismarked, it sends the tile.")]
    public UnityEvent<Tile> OnMarkTile;
    [Tooltip("When a bomb explodes, it sends the bomb.")]
    public UnityEvent<Bomb> OnExplodeBomb;
    [Tooltip("When enter or exits in pause, it sends the pause state.")]
    public UnityEvent<bool> OnPause;
    [Tooltip("When the player wants to multiply the reward, it sends an event.")]
    public UnityEvent OnRequestMultiplyReward;
    [Tooltip("When the player wants to use a power up, it sends de power up data.")]
    public UnityEvent<PlayObjectData> OnRequestPowerUp;
    [Tooltip("When the player wants to view a video to gets a power up, it sends an event.")]
    public UnityEvent OnRequestPowerUpVideo;
    [Tooltip("When the player wants to view a video to continue a failed game, it sends an event.")]
    public UnityEvent OnRequestContinuePlay;
    [Tooltip("When it shows the end level information, sends an event.")]
    public UnityEvent OnShowEndLevelInfo;
    [Tooltip("When it shows the end level information, sends an event.")]
    public UnityEvent OnHideEndLevelInfo;
    [Tooltip("When the player wants to get the the reward, it sends the report of the played level.")]
    public UnityEvent<LevelReport> OnEndLevel;
    [Tooltip("When the player wants to replay the level, it sends an event.")]
    public UnityEvent OnRequestReplay;

    #endregion

    #region Unity methods
    /// <summary>
    /// Initializes the play screen.
    /// </summary>
    protected virtual void Awake() {
        adsManager = AdsManager.Instance;
        levelReport = new LevelReport {
            screen = this,
            timeReward = -1
        };

        mainCamera.gameObject.SetActive(true);
        playerCamera.gameObject.SetActive(false); 
    }

    /// <summary>
    /// Resets the play screen when it is enabled.
    /// </summary>
    private void OnEnable() {
        inputObserver.OnStartPlayKeyUp.AddListener(LookForBombs);
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Starts the play session.
    /// </summary>
    /// <param name="thePlayer"></param>
    public virtual void Play(PlayObject thePlayer) {
        player = thePlayer;
        airView = false;
        playEnded = false;

        playerControls = player.GetComponent<SimpleCharacterControl>();
        playerControls.ControlsCamera = playerCamera;

        player.transform.parent = stage;
        player.transform.localPosition = initialPlayerPosition;
        player.transform.localRotation = Quaternion.Euler(0f, 45f, 0f);

        levelReport.player = player;
        levelReport.levelSuccess = false;

        playerCamera.TransformToTrack = player.transform;

        if (Application.isMobilePlatform) {
            playerControls.VirtualControls = virtualControls;
        }

        Pause = true;
        
        player.Show(false);
    }

    /// <summary>
    /// It updates the levelReport with the game result.
    /// </summary>
    /// <param name="levelSuccess">True if the level is success.</param>
    public virtual void EndPlay(bool levelSuccess) {
        StopAllCoroutines();

        pause = false;
        playEnded = true;

        inputObserver.IsEnabled = false;

        audioManger.StopMusic();

        levelReport.levelSuccess = levelSuccess;
        levelReport.timeReward = -1;
        levelReport.levelEndReward = 
            levelSuccess? levelSuccessReward : 0;

        levelReport.totalReward =
            levelReport.levelEndReward + levelReport.moneyCollected;

        endlevelInfoBox.AirView = airView;

        //endlevelInfoBox.ShowInfo(levelReport);

        playLevelInfoBox.SetActive(false);

        if (levelSuccess) {
            audioManger.PlayEffect(levelSuccessSound);
        }

        if (!airView) {
            playerCamera.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
        }

        if (Time.time - gameManager.LastInterstitialTime >= timeBetweenInterstials) {
            adsManager.OnCloseInterstitial += ContinueAfterInterstitial;

            adsManager.ShowInterstitial();
        }
        else {
            endlevelInfoBox.ShowInfo(levelReport);
            OnShowEndLevelInfo.Invoke();
        }
    }

    /// <summary>
    /// It multiplies the reward of the player.
    /// </summary>
    /// <param name="multiplier">The reward multiplier.</param>
    public void MultiplyReward(int multiplier) {
        levelReport.levelEndReward *= multiplier;
        levelReport.moneyCollected *= multiplier;
        levelReport.timeReward *= multiplier;
        levelReport.totalReward *= multiplier;

        //When the player multiplies the reward, it resets the last interstitial time to the current time.
        gameManager.LastInterstitialTime = Time.time;

        endlevelInfoBox.ShowInfo(levelReport);
    }

    /// <summary>
    /// It gets the reward of the player.
    /// </summary>
    /// <param name="replayLevel">True if the player wants to replay the level after getting the reward.</param>
    public void EndLevel(bool replayLevel) {
        //playLevelInfoBox.SetActive(false);
        endlevelInfoBox.gameObject.SetActive(false);

        OnEndLevel.Invoke(levelReport);

        if (replayLevel) {
            inputObserver.OnStartPlayKeyUp.AddListener(LookForBombs);
        }

        StartCoroutine(ResetPlayScreen(replayLevel));
    }

    /// <summary>
    /// It requests continue play.
    /// </summary>
    public void RequestContinuePlay() {
        OnRequestContinuePlay.Invoke();
    }

    public void RequestPowerUp(PlayObjectData powerUpData) {
        OnRequestPowerUp.Invoke(powerUpData);
    }

    public void ReequestPowerUpVideo() {
        OnRequestPowerUpVideo.Invoke();
    }

    /// <summary>
    /// When a tile is exposed, it look for bombs in
    /// the around tiles.
    /// </summary>
    /// <param name="tile">The tile exposed.</param>
    public void ExposeTile(Tile tile) {
        int coordX = (int)tile.Coords.x;
        int coordY = (int)tile.Coords.y;
        int bombsAround = 0;
        bool yDivisibleTwo = coordY % 2 == 0;
        List<Tile> tilesAround = new List<Tile>();

        if (!player.IsStunned) {
            levelReport.moneyCollected += tile.Reward;

            player.Pickup();
        }

        //Add coordY tiles
        if (coordX > 0) {
            tilesAround.Add(country[coordX - 1, coordY]);
        }

        if (coordX < tilesBySide - 1) {
            tilesAround.Add(country[coordX + 1, coordY]);
        }

        //Add coordY + 1 tiles 
        if (coordY < tilesBySide - 1) {
            tilesAround.Add(country[coordX, coordY + 1]);

            if (coordX > 0 && yDivisibleTwo) {
                tilesAround.Add(country[coordX - 1, coordY + 1]);
            }

            if (coordX < tilesBySide - 1 && !yDivisibleTwo) {
                tilesAround.Add(country[coordX + 1, coordY + 1]);
            }
        }

        //Add coordY - 1 tiles
        if (coordY > 0) {
            tilesAround.Add(country[coordX, coordY - 1]);

            if (coordX > 0 && yDivisibleTwo) {
                tilesAround.Add(country[coordX - 1, coordY - 1]);
            }

            if (coordX < tilesBySide - 1 && !yDivisibleTwo) {
                tilesAround.Add(country[coordX + 1, coordY - 1]);
            }
        }

        foreach (Tile aTile in tilesAround) {
            if (aTile.HasBomb) {
                bombsAround++;
            }
        }

        tile.BombsAround = bombsAround;

        if (bombsAround == 0) {
            foreach (Tile aTile in tilesAround) {
                if (!aTile.IsMarked && !aTile.IsExposed) {
                    aTile.Expose();
                }
            }
        }

        OnExposeTile.Invoke(tile);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tile"></param>
    public void MarkTile(Tile tile) {
        player.Pickup();

        if (tile.IsMarked) {
            BombsCount = bombsCount - 1;

            if (bombsCount == 0) {
                Pause = true;

                CheckCountry();
                //StartCoroutine(ExposeAllTiles());
            }
        }
        else {
            BombsCount = bombsCount + 1;
        }
        
        OnMarkTile.Invoke(tile);
    }

    /// <summary>
    /// It explodes a bomb.
    /// </summary>
    /// <param name="bomb">The bomb.</param>
    public void ExplodeBomb(Bomb bomb) {
        bomb.TurnOffBomb(2.4f);

        Handheld.Vibrate();

        Invoke(nameof(StunPlayer), .5f);
        Invoke(nameof(CheckContinues), 2.5f);

        OnExplodeBomb.Invoke(bomb);
    }

    /// <summary>
    /// It starts the coroutine to check if any unmarked
    /// tile has a bomb.
    /// </summary>
    public void CheckCountry() {
        EnableInput(false);
        StopAllCoroutines();
        StartCoroutine(ExposeAllTiles());

        //playerControls.OnExitTile.RemoveListener(ResetPlayerPosition);
    }

    /// <summary>
    /// 
    /// </summary>
    public void LookForBombs() {
        pause = false;

        startlevelInfoBox.ShowInfo(false);
        inputObserver.OnStartPlayKeyUp.RemoveListener(LookForBombs);
    }

    /// <summary>
    /// It enables or disables the listeners of the InputObserver.
    /// </summary>
    /// <param name="enable">If true, it enables the listeners.</param>
    public void EnableInput(bool enable) {
        if (Application.isMobilePlatform) {
            virtualControls.gameObject.SetActive(enable);
        }

        if (enable) {
            inputObserver.OnChangeViewKeyUp.AddListener(delegate {
                AirView = !airView;
            });

            inputObserver.OnPauseKeyUp.AddListener(delegate {
                if (pause) {
                    Pause = false;

                    pauseWindow.Close();
                }
                else {
                    Pause = true;

                    pauseWindow.Open();
                }

            });
        }
        else {
            inputObserver.OnChangeViewKeyUp.RemoveAllListeners();
            inputObserver.OnPauseKeyUp.RemoveAllListeners();
        }
    }
    #endregion

    #region Protected methods
    /// <summary>
    /// It checks if the player has used all continues.
    /// </summary>
    protected void CheckContinues() {
        Pause = true;

        if (usedContinues < continues) {
            usedContinues++;

            continueWindow.Open();
        }
        else {
            CheckCountry();
        }
    }

    /// <summary>
    /// Checks if the player is out of the country and resets its position.
    /// </summary>
    protected void CheckPlayerPosition() {
        if (player.transform.localPosition.y < 0f) {
            player.transform.localPosition = initialPlayerPosition;
            player.transform.localRotation = Quaternion.identity;
        }
    }
    #endregion

    #region Private methods
    /// <summary>
    /// It stuns the palyer.
    /// </summary>
    private void StunPlayer() {
        player.Stun(true);
    }

    /// <summary>
    /// Continues the game after the interstitial ad is closed.
    /// </summary>
    private void ContinueAfterInterstitial() {
        adsManager.OnCloseInterstitial -= ContinueAfterInterstitial;
        
        gameManager.LastInterstitialTime = Time.time;

        endlevelInfoBox.ShowInfo(levelReport);
        OnShowEndLevelInfo.Invoke();
    }

    #endregion

    #region Coroutines
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected IEnumerator<WaitForSeconds> SetPlayScreen() {
        int bombs = bombsCount;
        Tile currentTile;
        TileTheme tileTheme = tileThemes[Random.Range(0, tileThemes.Length)];
        Tile[] tilePrefabs = tileTheme.TilePrefabs;
        Vector2 bombCoords = new Vector2(
            Random.Range(0, tilesBySide), Random.Range(0, tilesBySide));
        Vector3 currentTilePosition = new Vector3(
            initialTilePosition.x,
            initialTilePosition.y,
            initialTilePosition.z);

        RenderSettings.skybox = skyboxes[Random.Range(0, skyboxes.Length)];

        while (pause) {
            yield return null;
        }

        mainCamera.gameObject.SetActive(true);

        for (int y = 0; y < tilesBySide; y++) {
            currentTilePosition.x = (float)y % 2 / 2;

            for (int  x = 0; x < tilesBySide; x++) {
                currentTile = Instantiate(
                    tilePrefabs[Random.Range(0, tilePrefabs.Length)], stage);

                inputObserver.AddListener(currentTile);

                currentTile.OnExpose.AddListener(ExposeTile);
                currentTile.OnMark.AddListener(MarkTile);
                
                country[x, y] = currentTile;

                currentTile.AudioManager = audioManger;
                currentTile.transform.position = currentTilePosition;
                currentTile.Coords = new Vector2(x, y);
                currentTile.AirView = airView;

                OnExposeTile.Invoke(currentTile);

                yield return new WaitForSeconds(deltaLoadTime);

                currentTilePosition.x += tileSize;
            }

            currentTilePosition.z += tileSize;
        }

        for (int i = 0; i < bombs; i++) {
            currentTile = country[(int)bombCoords.x, (int)bombCoords.y];

            while (currentTile.HasBomb || bombCoords == Vector2.zero) {
                bombCoords.x = Random.Range(0, tilesBySide); 
                bombCoords.y = Random.Range(0, tilesBySide);

                currentTile = country[(int)bombCoords.x, (int)bombCoords.y];
            }

            currentTile.Bomb = Instantiate(bombPrefab, currentTile.transform);

            currentTile.Bomb.OnExplode.AddListener(ExplodeBomb);
            currentTile.Bomb.TurnOffBomb();

            if (bombsMarkCount > 0) {
                bombsMarkCount--;

                currentTile.Mark();
            }
        }

        Pause = false;

        EnableInput(true);
        
        player.Show(true);
        startlevelInfoBox.ShowInfo(false);
        playLevelInfoBox.SetActive(true);

        StartCoroutine(Play());

        OnStartLevel.Invoke();
    }

    /// <summary>
    /// Once per second it checks the player position.
    /// </summary>
    protected abstract IEnumerator Play();

    /// <summary>
    /// It exposes all unmarked tiles, if any unmarked tile
    /// has a bomb, the bomb will explode.
    /// </summary>
    /// <returns>deltaLoadTime</returns>
    protected IEnumerator<WaitForSeconds> ExposeAllTiles() {
        bool bombFound = false;

        foreach (Tile tile in country) {
            tile.HighLighted = false;

            if (!tile.IsMarked && tile.HasBomb) {
                if (bombFound == false) {
                    bombFound = true;
                }

                tile.Bomb.OnExplode.RemoveAllListeners();

                tile.Bomb.ExplodeBomb();

                OnExplodeBomb.Invoke(tile.Bomb);

                yield return new WaitForSeconds(deltaLoadTime);
            }
        }

        if (bombFound) {
            player.Stun(true);
        }
        else {
            player.Win();
        }

        foreach (Tile tile in country) {
            if (bombFound && !tile.HasBomb) {
                ExposeTile(tile);

                if (tile.IsMarked) {
                    tile.ShowBody(false);
                }
            }
            else if (!bombFound && !tile.IsMarked) {
                tile.ShowBody(true);
            }

            OnExposeTile.Invoke(tile);

            yield return new WaitForSeconds(deltaLoadTime);
        }

        EndPlay(!bombFound);
    }

    /// <summary>
    /// It destroys the tiles of the country. It also 
    /// resets the play data and the level report
    /// </summary>
    /// <param name="replayLevel">True if the player wants to replay the level after resetting it.</param>
    /// <returns>The wait time to destroy the next tile.</returns>
    private IEnumerator ResetPlayScreen(bool replayLevel) {
        yield return new WaitForSeconds(deltaLoadTime);

        pause = false;
        usedContinues = 0;
        bombsMarkCount = 0;
        
        BombsCount = 0;

        levelReport.levelSuccess = false;
        levelReport.totalReward = 0;
        levelReport.moneyCollected = 0;
        levelReport.levelEndReward = 0;

        if (!rewardedVideoButton.interactable) {
            rewardedVideoButton.interactable = true;
        }

        if (country != null) {
            for (int i = 0; i < tilesBySide; i++) {
                for (int j = 0; j < tilesBySide; j++) {
                    country[i, j].OnExpose.RemoveAllListeners();
                    country[i, j].OnMark.RemoveAllListeners();

                    if (country[i, j].HasBomb) {
                        country[i, j].Bomb.OnExplode.RemoveAllListeners();
                    }

                    country[i, j].AudioManager = null;

                    inputObserver.RemoveListener(country[i, j]);

                    OnExposeTile.Invoke(country[i, j]);

                    DestroyImmediate(country[i, j].gameObject);

                    yield return new WaitForSeconds(deltaLoadTime);
                }
            }
        }

        //Destroy(environment);

        if (replayLevel) {
            OnRequestReplay.Invoke();
        }
        else {
            OnHideEndLevelInfo.Invoke();
        }
    }
    #endregion
}