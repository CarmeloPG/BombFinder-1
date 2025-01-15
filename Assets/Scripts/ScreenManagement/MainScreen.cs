using CrazyGames;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Inherit from GameScreen. It manages the actions 
/// of the player in the main screen.
/// </summary>
public class MainScreen : GameScreen {
    #region Readonly fields
    #endregion

    #region Serialize fields
    [Header("Main screen")]
    [SerializeField]
    [Tooltip("Button for play the daily challenge.")]
    private Button playChallengeButton;
    [SerializeField]
    [Tooltip("Button to play the classic (competition) button.")]
    private Button playClassicButton;
    /*[SerializeField]
    [Tooltip("Button to open the requeest logg in window. ")]
    private Button requestLoggInButton;*/
    [SerializeField]
    [Tooltip("The requeest logg in window. ")]
    private Window requestLoggInWindow;
    [SerializeField]
    [Tooltip("Text for show the next level to play.")]
    private TextMeshProUGUI levelText;
    [SerializeField]
    [Tooltip("It shows the money of the player.")]
    private MoneyInfoBox playerMoneyInfo;
    [SerializeField]
    [Tooltip("Tranform parent of the player avatar in the main scene.")]
    private Transform playerAvatarPlace;
    [Tooltip("Ground on which the avatar stands.")]
    [SerializeField]
    private BoxCollider playerAvatarGround;
    [Tooltip("The skybox in the main scene.")]
    [SerializeField]
    private Material skybox;
    /*[Tooltip("The music of the main scene.")]
    [SerializeField]
    private AudioClip screenMusic;*/
    [Tooltip("It sounds when the avatar is changed.")]
    [SerializeField]
    private AudioClip changeAvatarSound;
    #endregion

    #region Private fields
    //The player avatar. This avatar is a copy of the real player avatar.
    private GameObject playerAvatar;
    private AudioManager audioManager;
    #endregion

    #region Properties
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When a play object is bougth, sends the data of the bought PlayObject.")]
    public UnityEvent<PlayObjectData> OnBuyPlayObject;
    [Tooltip("When a play object is equiped, sends the data of the equiped PlayObject.")]
    public UnityEvent<PlayObjectData> OnEquipPlayObject;
    //[Tooltip("When the player is logged in, it sends the player's data.")]
    //public UnityEvent<PortalUser> OnAuthenticateUser;
    #endregion

    #region Unity methods
    private void OnEnable() {
        RenderSettings.skybox = skybox;

        if (audioManager == null) {
            audioManager = AudioManager.Instance;
        }

        //audioManager.PlayMusic(screenMusic);
    }

    private void OnDisable() {
        audioManager.StopMusic();
    }

    /* private void OnTriggerExit(Collider other) {
         playerAvatarGround.isTrigger = false;

         Destroy(other.gameObject, 1f);
         Debug.Log("Change avatar.");
         playerAvatar.SetActive(true);
     }*/
    #endregion

    #region Public methods
    /// <summary>
    /// When the game status is loaded, it checks if 
    /// the daily challenge is available.
    /// </summary>
    /// <param name="status">The last status of the game.</param>
    public void LoadStatus(GameStatus status) {
        //DateTime now = DateTime.Now;

        levelText.text = status.currentLevel.ToString();
        playChallengeButton.
            interactable = !status.challengeData.overcome;

        /*if (status.isLoggedIn) {
            playClassicButton.interactable = true;
            //ShowClassicButton(true);
        }
        else {
            playClassicButton.interactable = false;

            RequestAuthentication();
            //ShowClassicButton(true);
        }*/
    }

    /// <summary>
    /// It request to the player logg in Crazy Games account.
    /// </summary>
    public void RequestAuthentication() {
        requestLoggInWindow.Open();

        CrazySDK.User.ShowAuthPrompt((error, user) => {
            if (error == null || error.code.Equals("userAlreadySignedIn")) {
                playClassicButton.interactable = true;
                //ShowClassicButton(true);
            }
            else {
            //if (error != null && error.code.Equals("userCancelled")) {
                playClassicButton.interactable = false;
                //ShowClassicButton(false);
            }
        });
    }

    /// <summary>
    /// When a level is successful, it updates the
    /// current level text.
    /// </summary>
    /// <param name="levelReport"></param>
    public void UpdateLevel(LevelReport levelReport) {
        if (levelReport.levelSuccess) {
            int newLevel = levelReport.levelPlayed + 1;

            levelText.text = newLevel.ToString();
        }
    }

    /// <summary>
    /// When a challenge is successful, it disables
    /// the play challenge button.
    /// </summary>
    /// <param name="levelReport">The report of the level result.</param>
    public void EndChallenge(LevelReport levelReport) {
        playChallengeButton.interactable = !levelReport.levelSuccess;
    }

    /// <summary>
    /// When a play object is bought, it resends the
    /// event to the upper level managers.
    /// </summary>
    /// <param name="playObjectData">The play object bought.</param>
    public void BuyPlayObject(PlayObjectData playObjectData) {
        OnBuyPlayObject.Invoke(playObjectData);
    }

    /// <summary>
    /// When the player chages the avatar, it creates
    /// a copy of new avatar and puts it in the avatar transform.
    /// </summary>
    /// <param name="playObjectData">The data of the new avatar.</param>
    public void ChangePlayerAvatar(PlayObjectData playObjectData) {
        StartCoroutine(ChangeAvatar(playObjectData));
    }


    /// <summary>
    /// When the money of the player changes, it shows the 
    /// new money amount.
    /// </summary>
    /// <param name="moneyType"></param>
    /// <param name="newAmount"></param>
    public void ChangePlayerMoney(MoneyType moneyType, int newAmount) {
        playerMoneyInfo.ShowMoneyAmount(moneyType, newAmount);
    }
    #endregion

    #region Protected methods
    #endregion

    /// <summary>
    /// When it shows the classic button, it hides the 
    /// request loggin button.
    /// </summary>
    /// <param name="show">If false, it shows the request logg in button.</param>
    #region Private methods
    /*private void ShowClassicButton(bool show) {
        playClassicButton.gameObject.SetActive(show);
        requestLoggInButton.gameObject.SetActive(!show);
    }*/
    #endregion

    #region Coroutines
    /// <summary>
    /// 
    /// </summary>
    /// <param name="avatarData"></param>
    /// <returns></returns>
    private IEnumerator ChangeAvatar(PlayObjectData avatarData) {
        GameObject avatar = avatarData.GameObject;
        Transform avatarTransform = avatar.transform;
        string avatarName = avatarData.Identifier;

        if (playerAvatar != null && !playerAvatar.name.Equals(avatarName)) {
            playerAvatarGround.isTrigger = true;

            audioManager.PlayEffect(changeAvatarSound);

            yield return new WaitForSeconds(.5f);

            playerAvatarGround.isTrigger = false;

            DestroyImmediate(playerAvatar);
        }

        if (playerAvatar == null) {
            playerAvatar = Instantiate(avatar, playerAvatarPlace);

            playerAvatar.name = avatarName;
            playerAvatar.tag = avatar.tag;

            avatarTransform.position = Vector3.zero;
            avatarTransform.eulerAngles = Vector3.zero;
            avatarTransform.localScale = Vector3.one;

            playerAvatar.SetActive(true);
            playerAvatar.
                GetComponent<SimpleCharacterControl>().MoveSpeed = 0f;

            yield return new WaitForSeconds(.5f);

            audioManager.PlayEffect(changeAvatarSound);

            OnEquipPlayObject.Invoke(avatarData);
        }
    }
    #endregion
}