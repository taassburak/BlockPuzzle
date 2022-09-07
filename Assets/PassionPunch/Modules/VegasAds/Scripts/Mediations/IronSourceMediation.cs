using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace PassionPunch.Vegas
{
    #if PP_IRONSOURCE
    public class IronSourceMediation : MonoBehaviour, IAdMediation
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

        private IronSourceSettings settings;

        struct GDPR
        {
            public bool is_request_in_eea_or_unknown;
        }
        private void Start()
        {
            StartCoroutine(CheckGDPR("http://adservice.google.com/getconfig/pubvendors"));
            InitializeIronSource();
        }

        IEnumerator CheckGDPR(string uri)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.isNetworkError)
                {
                }
                else
                {
                    try
                    {
                        GDPR result = JsonUtility.FromJson<GDPR>(webRequest.downloadHandler.text);
                        VegasHelpers.IsGDPRApplicable = result.is_request_in_eea_or_unknown;
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        public void SetConfig(ISettings settings)
        {
            settings = (IronSourceSettings)settings;
        }

        private void InitializeIronSource()
        {
    #if UNITY_ANDROID
            string appKey = settings.ApplicationIdAndroid;
    #elif UNITY_IOS
            string appKey = settings.ApplicationIdIOS;
    #else
            string appKey = "unexpected_platform";
    #endif
            IronSource.Agent.validateIntegration();
            IronSource.Agent.setConsent(VegasHelpers.AdConsent);
            IronSource.Agent.shouldTrackNetworkState(true);
            IronSource.Agent.init(appKey);
        }

        void OnEnable()
        {
            // Add Banner Events
            IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
            IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;
            IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent;
            IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent;
            IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
            IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;

            // Add Interstitial Events
            IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
            IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
            IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
            IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
            IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
            IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
            IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;

            //Add Rewarded Video Events
            IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
            IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
            IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
            IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
            IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
            IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
            IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
            IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickedEvent;
        }


        public void LoadBanner()
        {
            Debug.Log("<IRONSRC> LoadBanner");
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
        }

        public void HideBanner()
        {
            Debug.Log("<IRONSRC> HideBanner");
            IronSource.Agent.destroyBanner();
        }

        public void LoadInterstitial()
        {
            Debug.Log("<IRONSRC> LoadInterstitial");
            IronSource.Agent.loadInterstitial();
        }

        public void ShowInterstitial()
        {
            Debug.Log("<IRONSRC> ShowInterstitial");
            IronSource.Agent.showInterstitial();
        }

        public bool IsInterstitialReady()
        {
            return IronSource.Agent.isInterstitialReady();
        }

        public bool IsRewardedReady()
        {
            return IronSource.Agent.isRewardedVideoAvailable();
        }


        public void LoadRewarded()
        {
            Debug.Log("<IRONSRC> LoadRewarded");
            IronSource.Agent.isRewardedVideoAvailable();
        }

        public void ShowRewarded()
        {
            Debug.Log("<IRONSRC> ShowRewarded");
            IronSource.Agent.showRewardedVideo();
        }


        public void SetAttributionData(string adId)
        {
            throw new NotImplementedException();
        }

        public void TrackIAPData(string transaction, decimal localizedPrice, string isoCurrencyCode, string[] tags)
        {
            throw new NotImplementedException();
        }

        void OnApplicationPause(bool isPaused)
        {
            IronSource.Agent.onApplicationPause(isPaused);
        }

        #region Banner callback handlers

        void BannerAdLoadedEvent()
        {
            Debug.Log("<IRONSRC> BannerAdLoadedEvent");
            IOnBannerReady?.Invoke(string.Empty, -1);
        }

        void BannerAdLoadFailedEvent(IronSourceError error)
        {
            //Debug.Log("unity-script: I got BannerAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
            Debug.Log("<IRONSRC> BannerAdLoadFailedEvent");
            IOnBannerFail?.Invoke(error.getCode().ToString());
        }

        void BannerAdClickedEvent()
        {
            Debug.Log("<IRONSRC> BannerAdClickedEvent");
            IOnBannerClick?.Invoke(string.Empty);
        }

        void BannerAdScreenPresentedEvent()
        {
            Debug.Log("<IRONSRC> BannerAdScreenPresentedEvent");
        }

        void BannerAdScreenDismissedEvent()
        {
            Debug.Log("<IRONSRC> BannerAdScreenDismissedEvent");
        }

        void BannerAdLeftApplicationEvent()
        {
            Debug.Log("<IRONSRC> BannerAdLeftApplicationEvent");
        }

        #endregion

        #region Interstitial callback handlers

        void InterstitialAdReadyEvent()
        {
            Debug.Log("<IRONSRC> InterstitialAdReadyEvent");
            IOnInterstitialReady?.Invoke(string.Empty, -1);
        }

        void InterstitialAdLoadFailedEvent(IronSourceError error)
        {
            //Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
            Debug.Log("<IRONSRC> InterstitialAdLoadFailedEvent");
            IOnInterstitialFail?.Invoke(error.getCode().ToString());
        }

        void InterstitialAdShowSucceededEvent()
        {
            Debug.Log("<IRONSRC> InterstitialAdShowSucceededEvent");
            IOnInterstitialShow?.Invoke();
        }

        void InterstitialAdShowFailedEvent(IronSourceError error)
        {
            //Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
            Debug.Log("<IRONSRC> InterstitialAdShowFailedEvent");
            IOnInterstitialFailToShow?.Invoke(error.getCode().ToString());
        }

        void InterstitialAdClickedEvent()
        {
            Debug.Log("<IRONSRC> InterstitialAdClickedEvent");
            IOnInterstitialClick?.Invoke(string.Empty);
        }

        void InterstitialAdOpenedEvent()
        {
            Debug.Log("<IRONSRC> InterstitialAdOpenedEvent");
        }

        void InterstitialAdClosedEvent()
        {
            Debug.Log("<IRONSRC> InterstitialAdClosedEvent");
            IOnInterstitialDismiss?.Invoke();
        }

        #endregion


        #region RewardedAd callback handlers

        void RewardedVideoAvailabilityChangedEvent(bool canShowAd)
        {
            if (canShowAd)
            {
                Debug.Log("<IRONSRC> RewardedVideoAvailabilityChangedEvent Ready");
                IOnRewardedReady?.Invoke(string.Empty,-1);
            }
            else
            {
                Debug.Log("<IRONSRC> RewardedVideoAvailabilityChangedEvent Fail");
            }
        }

        void RewardedVideoAdOpenedEvent()
        {
            Debug.Log("<IRONSRC> RewardedVideoAdOpenedEvent");
            IOnRewardedShow?.Invoke();
        }

        void RewardedVideoAdRewardedEvent(IronSourcePlacement ssp)
        {
            //Debug.Log("unity-script: I got RewardedVideoAdRewardedEvent, amount = " + ssp.getRewardAmount() + " name = " + ssp.getRewardName());
            Debug.Log("<IRONSRC> RewardedVideoAdRewardedEvent");
            IOnRewardedComplete?.Invoke();
        }

        void RewardedVideoAdClosedEvent()
        {
            Debug.Log("<IRONSRC> RewardedVideoAdClosedEvent");
            IOnRewardedDismiss?.Invoke();
        }

        void RewardedVideoAdStartedEvent()
        {
            Debug.Log("<IRONSRC> RewardedVideoAdStartedEvent");
        
        }

        void RewardedVideoAdEndedEvent()
        {
            Debug.Log("<IRONSRC> RewardedVideoAdEndedEvent");
        }

        void RewardedVideoAdShowFailedEvent(IronSourceError error)
        {
            //Debug.Log("unity-script: I got RewardedVideoAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
            Debug.Log("<IRONSRC> RewardedVideoAdShowFailedEvent");
            IOnRewardedFailToShow?.Invoke(error.getCode().ToString());
        }

        void RewardedVideoAdClickedEvent(IronSourcePlacement ssp)
        {
            Debug.Log("<IRONSRC> RewardedVideoAdStartedEvent");
            IOnRewardedClick?.Invoke(ssp.getRewardName().Replace(' ', '_'));
        }

        #endregion


    }
    #endif
}