using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PassionPunch.Vegas
{
#if PP_APPLOVINMAX

    public class AppLovinMaxMediation : MonoBehaviour, IAdMediation
    {
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


        private AppLovinMaxSettings settings;

        private string InterstitialAdUnitId = "ENTER_INTERSTITIAL_AD_UNIT_ID_HERE";
        private string RewardedAdUnitId = "ENTER_REWARD_AD_UNIT_ID_HERE";
        private string BannerAdUnitId = "ENTER_BANNER_AD_UNIT_ID_HERE";
        private string RewardedInterstitialAdUnitId = "ENTER_REWARD_INTER_AD_UNIT_ID_HERE";
        private string MRecAdUnitId = "ENTER_MREC_AD_UNIT_ID_HERE";

        private void Start()
        {
#if UNITY_IOS
        InterstitialAdUnitId = settings.InterstitialIdIOS  ;
        RewardedAdUnitId = settings.RewardedVideoIdIOS;
        BannerAdUnitId = settings.BannerIdIOS;
#else
            InterstitialAdUnitId = settings.InterstitialIdAndroid;
            RewardedAdUnitId = settings.RewardedVideoIdAndroid;
            BannerAdUnitId = settings.BannerIdAndroid;
#endif
            InitializeMax();
        }

        public void SetConfig(ISettings settings)
        {
            settings = (AppLovinMaxSettings)settings;
        }

        private void InitializeMax()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
            {
            // AppLovin SDK is initialized, configure and start loading ads.
            Debug.Log("<MAX> AppLovin SDK is initialized");

                InitializeInterstitialAds();
                InitializeRewardedAds();
                InitializeBannerAds();
            };

            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {
                if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.Applies)
                {
                    // Show user consent dialog
                    VegasHelpers.IsGDPRApplicable = true;
                }
                else if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.DoesNotApply)
                {
                    // No need to show consent dialog, proceed with initialization
                }
                else
                {
                    // Consent dialog state is unknown. Proceed with initialization, but check if the consent
                    // dialog should be shown on the next application initialization
                }
            };

            MaxSdk.SetHasUserConsent(VegasHelpers.AdConsent);

            MaxSdk.SetSdkKey(settings.AppLovinSDKKey);
            MaxSdk.InitializeSdk();

        }

        public void LoadBanner()
        {
            Debug.Log("<MAX> Load Banner");
            MaxSdk.ShowBanner(BannerAdUnitId);
        }

        public void HideBanner()
        {
            Debug.Log("<MAX> Hide Banner");
            MaxSdk.HideBanner(BannerAdUnitId);
        }

        public bool IsInterstitialReady()
        {
            return MaxSdk.IsInterstitialReady(InterstitialAdUnitId);
        }

        public bool IsRewardedReady()
        {
            return MaxSdk.IsRewardedAdReady(RewardedAdUnitId);
        }


        public void LoadInterstitial()
        {
            Debug.Log("<MAX> LoadInterstitial");
            MaxSdk.LoadInterstitial(InterstitialAdUnitId);
        }

        public void ShowInterstitial()
        {
            Debug.Log("<MAX> ShowInterstitial");
            MaxSdk.ShowInterstitial(InterstitialAdUnitId);
        }

        public void LoadRewarded()
        {
            Debug.Log("<MAX> LoadRewarded");
            MaxSdk.LoadRewardedAd(RewardedAdUnitId);
        }

        public void ShowRewarded()
        {
            Debug.Log("<MAX> ShowRewarded");
            MaxSdk.ShowRewardedAd(RewardedAdUnitId);
        }

        public void SetAttributionData(string adId)
        {
            throw new NotImplementedException();
        }


        public void TrackIAPData(string transaction, decimal localizedPrice, string isoCurrencyCode, string[] tags)
        {
            throw new NotImplementedException();
        }



        #region Banner Ad Methods

        private void InitializeBannerAds()
        {
            // Banners are automatically sized to 320x50 on phones and 728x90 on tablets.
            // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments.
            MaxSdk.CreateBanner(BannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

            // Set background or background color for banners to be fully functional.
            MaxSdk.SetBannerBackgroundColor(BannerAdUnitId, Color.white);
        }

        #endregion

        #region Interstitial Ad Methods

        private void InitializeInterstitialAds()
        {
            // Attach callbacks
            MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

        }

        private void OnInterstitialLoadedEvent(string adUnitId)
        {
            Debug.Log("<MAX> OnInterstitialLoadedEvent: " + adUnitId);
            IOnInterstitialReady?.Invoke(string.Empty, -1);
        }

        private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
        {
            Debug.Log("<MAX> OnInterstitialFailedEvent: " + adUnitId);
            IOnInterstitialFail?.Invoke(errorCode.ToString());
        }

        private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
        {
            Debug.Log("<MAX> InterstitialFailedToDisplayEvent: " + errorCode.ToString());
            IOnInterstitialFailToShow?.Invoke(errorCode.ToString());
        }

        private void OnInterstitialDismissedEvent(string adUnitId)
        {
            Debug.Log("<MAX> OnInterstitialDismissedEvent: ");
            IOnInterstitialDismiss.Invoke();
        }

        #endregion

        #region Rewarded Ad Methods

        private void InitializeRewardedAds()
        {
            // Rewarded callbacks
            MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        }

        private void OnRewardedAdLoadedEvent(string adUnitId)
        {
            Debug.Log("<MAX> OnRewardedAdLoadedEvent ");
            IOnRewardedReady?.Invoke(string.Empty, -1);
        }

        private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
        {
            Debug.Log("<MAX> OnRewardedAdFailedEvent: " + errorCode.ToString());
            IOnRewardedFail?.Invoke(errorCode.ToString());
        }

        private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
        {
            Debug.Log("<MAX> OnRewardedAdFailedToDisplayEvent: " + errorCode.ToString());
            IOnRewardedFailToShow?.Invoke(errorCode.ToString());
        }

        private void OnRewardedAdDisplayedEvent(string adUnitId)
        {
            Debug.Log("<MAX> OnRewardedAdDisplayedEvent: ");
            IOnRewardedShow?.Invoke();
        }

        private void OnRewardedAdClickedEvent(string adUnitId)
        {
            Debug.Log("<MAX> OnRewardedAdClickedEvent: ");
            IOnRewardedClick?.Invoke(string.Empty);
        }

        private void OnRewardedAdDismissedEvent(string adUnitId)
        {
            Debug.Log("<MAX> OnRewardedAdDismissedEvent ");
            IOnRewardedDismiss?.Invoke();
        }

        private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
        {
            Debug.Log("<MAX> OnRewardedAdReceivedRewardEvent ");
            IOnRewardedComplete?.Invoke();
        }

        #endregion


    }
#endif
}