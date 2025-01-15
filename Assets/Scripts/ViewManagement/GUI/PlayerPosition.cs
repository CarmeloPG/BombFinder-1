using UnityEngine;
using System.Collections;
using CarterGames.Assets.LeaderboardManager;
using CarterGames.Assets.LeaderboardManager.Serialization;
using CrazyGames;
using System;

/// <summary>
/// It shows the player position in a leaderboard.
/// </summary>
public class PlayerPosition : MonoBehaviour {
	#region Readonly fields
	#endregion

	#region Serialize fields
	[SerializeField]
	private LeaderboardDisplayTime leaderboardDisplay;
	[SerializeField]
	private LeaderboardEntryDisplayTMP display;
    [SerializeField]
    private LeaderboardDisplayCustomisations customisations;// = new LeaderboardDisplayCustomisations();
    #endregion

    #region Private fields
    private Leaderboard leaderboard;
    private PortalUser player;
    #endregion

    #region Properties
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    /// <summary>
    /// On awake it checks if the player is logged.
    /// </summary>
    private void Awake() {
        //display.ResetDisplay();

        CrazySDK.User.GetUser(user => {
            player = user;

            if (player == null) {
                leaderboard = null;
            }
            else {
                leaderboard = 
                    LeaderboardManager.GetLeaderboard(leaderboardDisplay.BoardId);
            }
        });
    }


    /// <summary>
    /// On enable, it shows the player position in the leaderboard.
    /// </summary>
    private void OnEnable() {
        if (player != null) {
            LeaderboardEntry playerEntry = leaderboard.GetEntry(player.username);

            if (playerEntry == null) {
                display.ResetDisplay();
            }
            else {
                int playerIndex = 
                        Array.IndexOf(leaderboard.GetAllAscending(), playerEntry);

                display.UpdateDisplay(playerEntry, playerIndex, customisations);
            }
        }
    }
    #endregion

    #region Public methods
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}