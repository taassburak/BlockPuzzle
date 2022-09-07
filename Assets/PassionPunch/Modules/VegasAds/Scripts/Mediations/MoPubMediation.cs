using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PassionPunch.Vegas
{ 
    #if PP_MOPUB
    public class MoPubMediation : MonoBehaviour, IAdMediation
    {
        private string[] bannerAdUnits;
        private string[] interstitialAdUnits;
        private  string[] rewardedAdUnits;
        private MoPubSettings settings;
    
        public event Action<string, double> IOnBannerReady;
        public event Action<string> IOnBannerFail;
        public event Action<string> IOnBannerClick;
        public event Action<string, double> IOnInterstitialReady;
        public event Action<string> IOnInterstitialFail;
        public event Action IOnInterstitialShow;
        public event Action<string> IOnInterstitialFailToShow;
        public event Action<string> IOnInterstitialClick;
        public event Action IOnInterstitialDismiss;
        public event Action<int> IOnInterstitialStatusChange;
        public event Action<string, double> IOnRewardedReady;
        public event Action<string> IOnRewardedFail;
        public event Action IOnRewardedShow;
        public event Action<string> IOnRewardedFailToShow;
        public event Action<string> IOnRewardedClick;
        public event Action IOnRewardedComplete;
        public event Action IOnRewardedDismiss;

        private void Start()
        {
            //use MoPub auto consent popup 
            VegasHelpers.IsGDPRApplicable = false; // (bool)MoPub.IsGdprApplicable;
    #if UNITY_ANDROID
            bannerAdUnits = settings.bannerIdAndroid;
            interstitialAdUnits = settings.interstitialIdAndroid;
            rewardedAdUnits = settings.rewardedVideoIdAndroid;
    #elif UNITY_IOS
            bannerAdUnits = settings.bannerIdIOS;
            interstitialAdUnits = settings.interstitialIdIOS;
            rewardedAdUnits = settings.rewardedVideoIdIOS;
    #endif
        
            InitializeMoPub();
        }
        public void SetConfig(ISettings settings)
        {
            settings = (MoPubSettings)settings;
        }

        private void InitializeMoPub()
        {
            // The SdkInitialize() call is handled by the MoPubManager prefab now. Please see:
            // https://developers.mopub.com/publishers/unity/initialize/#option-1-use-the-mopub-manager-recommended

            MoPub.LoadBannerPluginsForAdUnits(bannerAdUnits);
            MoPub.LoadInterstitialPluginsForAdUnits(interstitialAdUnits);
            MoPub.LoadRewardedVideoPluginsForAdUnits(rewardedAdUnits);

            //create banner on first load
            MoPub.RequestBanner(bannerAdUnits[0], MoPub.AdPosition.BottomCenter, MoPub.MaxAdSize.Width320Height50);
        }

        private void OnEnable()
        {
            MoPubManager.OnAdLoadedEvent += OnAdLoadedEvent;
            MoPubManager.OnAdFailedEvent += OnAdFailedEvent;

            MoPubManager.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
            MoPubManager.OnInterstitialFailedEvent += OnInterstitialFailedEvent;
            MoPubManager.OnInterstitialDismissedEvent += OnInterstitialDismissedEvent;

            MoPubManager.OnRewardedVideoLoadedEvent += OnRewardedVideoLoadedEvent;
            MoPubManager.OnRewardedVideoFailedEvent += OnRewardedVideoFailedEvent;
            MoPubManager.OnRewardedVideoFailedToPlayEvent += OnRewardedVideoFailedToPlayEvent;
            MoPubManager.OnRewardedVideoClosedEvent += OnRewardedVideoClosedEvent;

            MoPubManager.OnImpressionTrackedEvent += OnImpressionTrackedEvent;
        }


        private void OnDisable()
        {
            // Remove all event handlers
            MoPubManager.OnAdLoadedEvent -= OnAdLoadedEvent;
            MoPubManager.OnAdFailedEvent -= OnAdFailedEvent;

            MoPubManager.OnInterstitialLoadedEvent -= OnInterstitialLoadedEvent;
            MoPubManager.OnInterstitialFailedEvent -= OnInterstitialFailedEvent;
            MoPubManager.OnInterstitialDismissedEvent -= OnInterstitialDismissedEvent;

            MoPubManager.OnRewardedVideoLoadedEvent -= OnRewardedVideoLoadedEvent;
            MoPubManager.OnRewardedVideoFailedEvent -= OnRewardedVideoFailedEvent;
            MoPubManager.OnRewardedVideoFailedToPlayEvent -= OnRewardedVideoFailedToPlayEvent;
            MoPubManager.OnRewardedVideoClosedEvent -= OnRewardedVideoClosedEvent;

            MoPubManager.OnImpressionTrackedEvent -= OnImpressionTrackedEvent;
        }



        public void LoadBanner()
        {
            Debug.Log("<MoPub> LoadBanner ");
            MoPub.ShowBanner(bannerAdUnits[0], true);
        }

        public void HideBanner()
        {
            Debug.Log("<MoPub> HideBanner ");
            MoPub.ShowBanner(bannerAdUnits[0], false);
        }

        public void LoadInterstitial()
        {
            Debug.Log("<MoPub> LoadInterstitial ");
            MoPub.RequestInterstitialAd(interstitialAdUnits[0]);
        }

        public bool IsInterstitialReady()
        {
            return MoPub.IsInterstitialReady(interstitialAdUnits[0]);
        }

        public void ShowInterstitial()
        {
            Debug.Log("<MoPub> ShowInterstitial ");
            MoPub.ShowInterstitialAd(interstitialAdUnits[0]);
        }

        public void LoadRewarded()
        {
            Debug.Log("<MoPub> LoadRewarded");
            MoPub.RequestRewardedVideo(rewardedAdUnits[0]);
        }

        public bool IsRewardedReady()
        {
            return MoPub.HasRewardedVideo(rewardedAdUnits[0]);
        }

        public void ShowRewarded()
        {
            Debug.Log("<MoPub> ShowRewarded");
            MoPub.ShowRewardedVideo(rewardedAdUnits[0]);
        }

        public void SetAttributionData(string adId)
        {
            throw new NotImplementedException();
        }

        public void TrackIAPData(string transaction, decimal localizedPrice, string isoCurrencyCode, string[] tags)
        {
            throw new NotImplementedException();
        }

        private void OnImpressionTrackedEvent(string adUnitId, MoPub.ImpressionData impressionData)
        {
    #if PP_ADJUST
            // Pass impression data JSON to Adjust SDK.
            com.adjust.sdk.Adjust.trackAdRevenue(com.adjust.sdk.AdjustConfig.AdjustAdRevenueSourceMopub, impressionData.JsonRepresentation);
    #endif
        }

    #region Banner Events

        private void OnAdLoadedEvent(string adUnitId, float height)
        {
            Debug.Log("<MoPub> OnAdLoadedEvent "+ adUnitId);
            IOnBannerReady?.Invoke(string.Empty, -1);
        }


        private void OnAdFailedEvent(string adUnitId, string error)
        {
            Debug.Log("<MoPub> OnAdFailedEvent "+ adUnitId);
            IOnBannerFail?.Invoke(error);
        }

    #endregion

    #region Interstitial Events

        private void OnInterstitialLoadedEvent(string adUnitId)
        {
            Debug.Log("<MoPub> OnInterstitialLoadedEvent " + adUnitId);
            IOnInterstitialShow?.Invoke();
        }


        private void OnInterstitialFailedEvent(string adUnitId, string error)
        {
            Debug.Log("<MoPub> OnInterstitialFailedEvent " + adUnitId);
            IOnInterstitialFail?.Invoke(error);
        }


        private void OnInterstitialDismissedEvent(string adUnitId)
        {
            Debug.Log("<MoPub> OnInterstitialDismissedEvent " + adUnitId);
            IOnInterstitialDismiss?.Invoke();
        }
    #endregion

    #region Rewarded Video Events

        private void OnRewardedVideoLoadedEvent(string adUnitId)
        {
            var availableRewards = MoPub.GetAvailableRewards(adUnitId);
            Debug.Log("<MoPub> OnRewardedVideoLoadedEvent " + adUnitId);
            IOnRewardedShow?.Invoke();
        }


        private void OnRewardedVideoFailedEvent(string adUnitId, string error)
        {
            Debug.Log("<MoPub> OnRewardedVideoFailedEvent " + adUnitId);
            IOnRewardedFail?.Invoke(error);
        }


        private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
        {
            Debug.Log("<MoPub> OnRewardedVideoFailedToPlayEvent " + adUnitId);
            IOnRewardedFailToShow?.Invoke(error);
        }


        private void OnRewardedVideoClosedEvent(string adUnitId)
        {
            Debug.Log("<MoPub> OnRewardedVideoClosedEvent " + adUnitId);
            IOnRewardedComplete?.Invoke();
            IOnRewardedDismiss?.Invoke();
        }
    #endregion
    }
    #endif
}