
using System;
using UnityEngine;
using UnityEngine.Events;
using CrazyGames;


/// <summary>
/// Adversiting policy:
/// 
/// A single banner will be displayed when the end of screen
/// information is displayed. The rest of the timeReward it will be
/// hidden. The banner will be placed in a place where the player 
/// cannot press it by mistake. The banner is updated:
///     - When the player presses it.
///     - When the end level information is shown.
///     
/// The player will be offered the possibility of viewing a 
/// rewarded advertisement:
///     - At the beginning a level, to get some improvement.
///     - When a level is finished, to multiply the reward.
///     
/// A midgame ad will occasionally be displayed when the player 
/// finishes a level. The following criteria will be 
/// followed to display the ad:
///     - The first ad will be shown after the player 
///       has completed 5 matches without seeing any rewarded ads.
///     - When the player sees a rewarded ad, the number of games increases by 1. 
///       In this way, if the player sees a rewarded ad, 
///       a midgame will not be shown until after 6 games without seeing rewarded ads. 
///       When the player sees another rewarded, the number of games increases to 7,Etc.
///     - When the ad is shown, the number of games without seeing rewarded ads increases 
///       by one so that the second migame is shown after 6 games without seeing rewarded ads,
///       the third after 7, etc. This sum is accumulated with that of the previous step.
///     - If the player sees rewarded ads often, 
///       they will be able to play without seeing any midgame ads.
/// </summary>
public class AdsManager : MonoBehaviour
{
	#region Readonly fields
	#endregion

	#region Serialize fields
	[SerializeField]
	private CrazyBanner banner;
    [SerializeField]
    [Tooltip("Number of levels without show a midgame ad.")]
    private int playsBetweenAds = 5;
    #endregion

    #region Private fields
    //Number of levels played withou show a rewarded ad.
    private int playsWithoutAds;
    #endregion

    #region Properties
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("It sends it when a banner is closed.")]
    public UnityEvent OnCloseBanner;
    [Tooltip("It sends it when an insterstitial is closed.")]
    public UnityEvent OnCloseMidGameAd;
    [Tooltip("It sends it when the Player get a reward from a rewarded Ad.")]
    public UnityEvent OnEarnReward;
    [Tooltip("It sends it when the load of a rewarded ad has failed.")]
    public UnityEvent OnFailLoadRewardAd;
    [Tooltip("It sends it when a rewarded ad is loaded.")]
    public UnityEvent OnLoadRewardedAd;
    [Tooltip("An addblock is detected.")]
    public UnityEvent OnDetectAdBlock;
	#endregion

	#region Unity methods
	#endregion

	#region Public methods
	public void InitializeAds() {
        playsWithoutAds = 0;

		CrazySDK.Ad.HasAdblock((hasAdblock) => {
			if (hasAdblock) {
				OnDetectAdBlock.Invoke();
			}
		});

        //CrazySDK.Banner.RegisterBanner(banner);

        //ShowBanner();
	}

    public void ShowBanner() {
        CrazySDK.Banner.RefreshBanners();
    }

    /*public void HideBanner() {
    }*/

    public void ShowMidGame() {
        if (playsWithoutAds >= playsBetweenAds) {
            CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded,
            //Ad Started
            () => {
                UpdateMidGameOptions();
            },
            //Ad error
            null,
            //Ad Finised
            null);
        }
        else {
            playsWithoutAds++;
        }
    }

    public void ShowRewardedAd() {
        UpdateMidGameOptions();

        CrazySDK.Ad.RequestAd(CrazyAdType.Rewarded,
            //Ad Started
            null,
            //Ad error
            null,
            //Ad Finised
            () => {
                OnEarnReward.Invoke();
            });
    }
    #endregion

    #region Private methods

    private void UpdateMidGameOptions() {
        playsBetweenAds++;
        playsWithoutAds = 0;
    }

	private void OnMidGameClosed(object sender, EventArgs args) {
		UpdateMidGameOptions();
	}
	#endregion
}
