using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using UnityEngine;

[Prefab("AdsManager", true)]
public class AdsManager : Singleton<AdsManager> {
#if UNITY_ANDROID
    #region Readonly fields
    //Test ad unit IDs for Android. Replace these with your own ad unit IDs when deploying your app.
    private readonly string 
		idBottomBanner = "ca-app-pub-3940256099942544/6300978111",
        idRewardedAd = "ca-app-pub-3940256099942544/1033173712",
        idInterstitialAd = "ca-app-pub-3940256099942544/1033173712";


    /*private readonly string
        idBottomBanner = "ca-app-pub-8955180310605823/4952283006";*/
    #endregion

    #region Fields
    //private bool isRewardEarned;
    //Indicates whether ads are enabled or disabled.
    private bool isAdsEnabled;
    //The bottom banner.
    private BannerView bottomBanner;
    //The interstitial ad.
    private InterstitialAd interstitial;
    //The rewarded ad.
    private RewardedAd rewardedAd;
    #endregion

    #region Properties
    #endregion

    #region Events
    //It is fired when the player earns a reward after watching a rewarded ad.
    public event Action OnEarnReward = delegate { };
    //It is fired when the interstitial ad is closed.
    public event Action OnCloseInterstitial = delegate { };
    #endregion

    #region Monobehaviour methods
    /// <summary>
    /// It initializes the Mobile Ads SDK and requests the bottom banner and rewarded ad.
    /// </summary>
    protected override void Awake() {
        base.Awake();

        MobileAds.Initialize(initStatus => {
            if (initStatus == null) {
                isAdsEnabled = false;
            }
            else {
                isAdsEnabled = true;

                RequestBottomBanner();
                RequestInterstitial();
                RequestRewardedAd();
            }
        });
    }

    /// <summary>
    /// Destroys the bottom banner and rewarded ad when the AdsManager is destroyed.
    /// </summary>
    protected override void OnDestroy() {
        base.OnDestroy();

        if (bottomBanner != null) {
            bottomBanner.Destroy();

            if (rewardedAd != null) {
                rewardedAd.Destroy();
            }
        }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Show the bottom banner.
    /// </summary>
    public void ShowBottomBanner() {
        if (bottomBanner != null) {
            bottomBanner.Show();
        }
    }

    /// <summary>
    /// Hide the bottom banner.
    /// </summary>
    public void HideBottomBanner() {
        if (bottomBanner != null) {
            bottomBanner.Hide();
        }
    }

    public void ShowInterstitial() {
        if (interstitial != null && interstitial.CanShowAd()) {
            interstitial.Show();

#if UNITY_EDITOR
            Time.timeScale = 1f;
#endif
        }
    }

    /// <summary>
    /// Show the rewarded ad.
    /// </summary>
    public void ShowRewardedAd() {
        if (rewardedAd != null && rewardedAd.CanShowAd()) {
            rewardedAd.Show((reward) => {
#if UNITY_EDITOR
            Time.timeScale = 1f;
#endif
            });
        }
        else {
            if (rewardedAd == null && isAdsEnabled) {
                RequestRewardedAd();
            }

            OnEarnReward.Invoke();
        }
    }
    #endregion

    #region Private methods
    /// <summary>
    /// Requests the bottom banner.
    /// </summary>
    private void RequestBottomBanner() {
        AdRequest request;

        bottomBanner = new BannerView(idBottomBanner, AdSize.Banner, AdPosition.BottomLeft);
        request = new AdRequest();

        bottomBanner.LoadAd(request);
    }

    /// <summary>
    /// Requests the interstitial ad.
    /// </summary>
    private void RequestInterstitial() {
        AdRequest request = new AdRequest();
        
        InterstitialAd.Load(idInterstitialAd, request, (ad, error) => {
            if (error == null) {
                interstitial = ad;

                interstitial.OnAdFullScreenContentClosed += CloseInterstitial;
            }
        });
    }

    /// <summary>
    /// Closes the interstitial ad and requests a new one.
    /// </summary>
    private void CloseInterstitial() {
        interstitial.OnAdFullScreenContentClosed -= CloseInterstitial;
        
        interstitial.Destroy();
        
        OnCloseInterstitial.Invoke();

        if (isAdsEnabled) {
            RequestInterstitial();
        }
    }

    /// <summary>
    /// Requests the rewarded ad.
    /// </summary>
    private void RequestRewardedAd() {
        AdRequest request = new AdRequest();

        RewardedAd.Load(idRewardedAd, request, (ad, error) => {
            if (error == null) {
                rewardedAd = ad;

                rewardedAd.OnAdFullScreenContentClosed += EarnReward;
            }
        });
    }

    /// <summary>
    /// Earns the reward when the rewarded ad is closed.
    /// </summary>
    private void EarnReward() {
        rewardedAd.OnAdFullScreenContentClosed -= EarnReward;
        rewardedAd.Destroy();

        OnEarnReward.Invoke();

        if (isAdsEnabled) {
            RequestRewardedAd();
        }
    }
    #endregion
#endif
}
