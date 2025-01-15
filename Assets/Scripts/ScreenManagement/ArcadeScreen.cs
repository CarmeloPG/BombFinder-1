using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


/// <summary>
/// Play Screen to arcade mode.
/// 
/// The game begins at level 1 with a field of 
/// (minTilesBySide X TilesBySide) tiles and a bomb density 
/// of minBombDensity. When a level is succed, the bomb density
/// increases by deltaBombDensity until it reaches a maximum of
/// maxBombDensity. When the maximum bomb density is reached, 
/// the density returns to minBombDensity and a tile is added
/// to the field. Will be added one tile per side up to a 
/// maximum of (maxTilesBySide X maxTilesBySide). In this way, 
/// a tile is added every 
/// (maxBombDensity - minBombDensity) / deltaBombDensity
/// levels. 
/// 
/// When the field has its maximum size and its maximum bomb 
/// density, mobile bombs (enemies) begin to appear. These bombs
/// appear in random places on the field and run forward until 
/// they fall into the void or collide with the player avatar.
/// When an enemy collides with the player's avatar, all the 
/// bombs on the field explode and the level will be failed. 
/// Enemy spawn frequency starts at one enemy every 
/// minSpawnEnemiesPeriod and increases by deltaSpawnEnemiesPeriod seconds 
/// per level until it reaches one enemy every minSpawnEnemiesperiod seconds.
/// </summary>
public class ArcadeScreen : PlayScreen {
    #region Readonly fields
    //When the player uses a power-up it will mark bombsCount * powerUpMultiplier bombs.
    private readonly float powerUpMultiplier = .2f;
    #endregion

    #region Serialize fields
    [Header("Arcade mode")]
    [SerializeField]
    [Tooltip("the mobile bombs prefab.")]
    private MobileBomb mobileBombPrefab;
    [SerializeField]
    [Tooltip("Increased bomb density per level. When the bombs density is higher than maxBombDensity, it will be set to minBombDensity.")]
    private float deltaBombDensity = .02f;
    [SerializeField]
    [Tooltip("The level to start to spawn mobile bombs.")]
    private int spawnEnemiesLevel = 5;
    [SerializeField]
    [Tooltip("The minimum period, in seconds, to spawn enemies. The minimum period represents the maximum frecuency.")]
    private float minSpawnEnemiesPeriod = 10f;
    [SerializeField]
    [Tooltip("The maximum period, in seconds, to spawn enemies. The maximum period represents the minimum frecuency.")]
    private float maxSpawnEnemiesPeriod = 60f;
    [SerializeField]
    [Tooltip("Decreased the period of spawn enemies per level. It decreases the period so increases the frecuency.")]
    private float deltaSpawnEnemiesPeriod = .5f;
    [SerializeField]
    [Tooltip("A shop to buy bomb marks.")]
    private Window powerUpShopWindow;
    [SerializeField]
    [Tooltip("The number bomb marks that it can be bought in the powerUpShopWindow.")]
    private TextMeshProUGUI marksCountText;
    [SerializeField]
    [Tooltip("It does the same actions as the replayButton but it changes the label.")]
    private Button playNextLevelButton;
    [SerializeField]
    [Tooltip("It will be played when a new enemy is spawned.")]
    private AudioClip spawnEnemySound;
    #endregion

    #region Private fields
    //A list with the mobileBombsSpawned
    private List<MobileBomb> mobileBombs;
    private int
        //The current level.
        currentLevel,
        //Every few leveles a tile is added.
        addTileLevel;
        //The level to start to spawn mobile bombs.
        //spawnEnemiesLevel;
    #endregion

    #region Properties
    /// <summary>
    /// It gets and sets the currentLevel value.
    /// </summary>
    public int CurrentLevel {
        get => currentLevel;
        set => currentLevel = value;
    }


    /// <summary>
    /// The level to start to spawn mobile bombs.
    /// </summary>
    public int SpawnEnemiesLevel {
        get => spawnEnemiesLevel;
    }

    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /// <summary>
    /// On awake, it sets the level at witch a tile is added.
    /// </summary>
    protected override void Awake() {
        base.Awake();

        mobileBombs = new List<MobileBomb>();

        addTileLevel = Mathf.FloorToInt(
            1 + ((maxBombDensity - minBombDensity) / deltaBombDensity));

        //spawnEnemiesLevel = 
            //(maxTilesBySide - minTilesBySide) * addTileLevel;

        Debug.Log($"Add tile level: {addTileLevel}.");
    }
    #endregion

    #region Public methods
    /// <summary>
    /// It starts the game.
    /// </summary>
    /// <param name="thePlayer">The player.</param>
    public override void Play(PlayObject thePlayer) {
        float bombsDensity = Mathf.Min(
            minBombDensity + (currentLevel % addTileLevel * deltaBombDensity),
            maxBombDensity);

        base.Play(thePlayer);

        TilesBySide = Mathf.Min(
            minTilesBySide + Mathf.FloorToInt(currentLevel / addTileLevel),
            maxTilesBySide);

        BombsCount = Mathf.FloorToInt(tilesCount * bombsDensity);

        country = new Tile[tilesBySide, tilesBySide];

        levelReport.levelPlayed = currentLevel;
        marksCountText.text =
            Mathf.FloorToInt(bombsCount * powerUpMultiplier).ToString();

        powerUpShopWindow.Open();
        startlevelInfoBox.ShowInfo(this, false);

        StartCoroutine(SetPlayScreen());
    }

    /// <summary>
    /// It enebles the replayLevelButton or the playNextLevelButton.
    /// </summary>
    /// <param name="levelSuccess">If true, it enables the playNextButton</param>
    public override void EndPlay(bool levelSuccess) {
        base.EndPlay(levelSuccess);

        replayLevelButton.gameObject.SetActive(!levelSuccess);
        playNextLevelButton.gameObject.SetActive(levelSuccess);
    }

    /// <summary>
    /// It uses a power up.
    /// </summary>
    /// <param name="powerUp">The power up.</param>
    public void UsePowerUp(PlayObject powerUp) {
        bombsMarkCount = Mathf.FloorToInt(bombsCount * powerUpMultiplier);
        pause = false;

        Destroy(powerUp.gameObject);

        powerUpShopWindow.Close();
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// It spawns an enemie.
    /// </summary>
    public void SpawnEnemy() {
        MobileBomb enemie = mobileBombs.FindLast(bomb => bomb.EnableBomb == false);
        Vector3 direction = Vector3.zero;
        //First(bomb => bomb.EnableBomb == false);

        Vector3 position = new Vector3( 
            Random.Range(0, tilesBySide), .2f, Random.Range(0, tilesBySide));

        while (direction == Vector3.zero) {
            direction = new Vector3(
                Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        }

        if (enemie == null) {
            enemie = Instantiate(mobileBombPrefab, stage);

            enemie.Bomb.OnExplode.AddListener(ExplodeBomb);

            mobileBombs.Add(enemie);
        }

        enemie.EnableBomb = true;
        enemie.transform.position = position;

        enemie.GoTo(direction);

        audioManger.PlayEffect(spawnEnemySound);
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Once per second it checks the player position.
    /// 
    /// If is playing a level with enemies, it checks the 
    /// time to spawn a new enemie.
    /// </summary>
    /// <returns>Wait for 1 second.</returns>
    protected override IEnumerator Play() {
        float 
            elapsedTime = 0,
            timeToSpawnEnemie = currentLevel < spawnEnemiesLevel? 0 :
                Mathf.Max(
                    maxSpawnEnemiesPeriod - ((currentLevel - spawnEnemiesLevel) * deltaSpawnEnemiesPeriod), 
                    minSpawnEnemiesPeriod);

        while (!pause) {
            CheckPlayerPosition();

            if (currentLevel >= spawnEnemiesLevel) {
                elapsedTime++;

                if (elapsedTime > timeToSpawnEnemie ) {
                    elapsedTime = 0;

                    SpawnEnemy();
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }
    #endregion
}