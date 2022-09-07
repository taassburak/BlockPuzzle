using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PassionPunch.Vegas
{
#if PP_ADMOST
	public class AdmostMediation : MonoBehaviour, IAdMediation
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

		private AdmostSettings settings;


		void Start()
		{
			InitializeAdmost();

		}

		public void SetConfig(ISettings conf)
		{
			settings = (AdmostSettings)conf;
		}

		private void InitializeAdmost()
		{
			AMR.AMRSdkConfig config = new AMR.AMRSdkConfig();

			/* Set zone ids */
			config.ApplicationIdAndroid = settings.ApplicationIdAndroid;
			config.ApplicationIdIOS = settings.ApplicationIdIOS;

			config.BannerIdAndroid = settings.BannerIdAndroid;
			config.BannerIdIOS = settings.BannerIdIOS;

			config.InterstitialIdAndroid = settings.InterstitialIdAndroid;
			config.InterstitialIdIOS = settings.InterstitialIdIOS;

			config.RewardedVideoIdAndroid = settings.RewardedVideoIdAndroid;
			config.RewardedVideoIdIOS = settings.RewardedVideoIdIOS;

			//config.OfferWallIdAndroid = "fa1072e4-afcf-49b6-a919-1ab1ab1b0aa9";
			//config.OfferWallIdIOS = "1cadca08-33f9-4da7-969e-ef116d4e7d0e";

			//GDPR COMPLIANCE
			//config.UserConsent = "1";
			//config.SubjectToGDPR = "1";

			config.SubjectToGDPR = VegasHelpers.IsGDPRApplicable ? "1" : "0";
			config.UserConsent = VegasHelpers.AdConsent ? "1" : "0";
			AMR.AMRSDK.startWithConfig(config);


			/* Banner Callbacks - Optional */
			AMR.AMRSDK.setOnBannerReady(OnBannerReady);
			AMR.AMRSDK.setOnBannerFail(OnBannerFail);
			AMR.AMRSDK.setOnBannerClick(OnBannerClick);

			/* Interstitial Callbacks - Optional */
			AMR.AMRSDK.setOnInterstitialReady(OnInterstitialReady);
			AMR.AMRSDK.setOnInterstitialFail(OnInterstitialFail);
			AMR.AMRSDK.setOnInterstitialShow(OnInterstitialShow);
			AMR.AMRSDK.setOnInterstitialFailToShow(OnInterstitialFailToShow);
			AMR.AMRSDK.setOnInterstitialClick(OnInterstitialClick);
			AMR.AMRSDK.setOnInterstitialDismiss(OnInterstitialDismiss);
			AMR.AMRSDK.setOnInterstitialStatusChange(OnInterstitialStatusChange);

			/* Rewarded Video Callbacks - Optional */
			AMR.AMRSDK.setOnRewardedVideoReady(OnVideoReady);
			AMR.AMRSDK.setOnRewardedVideoFail(OnVideoFail);
			AMR.AMRSDK.setOnRewardedVideoShow(OnVideoShow);
			AMR.AMRSDK.setOnRewardedVideoFailToShow(OnVideoFailToShow);
			AMR.AMRSDK.setOnRewardedVideoClick(OnVideoClick);
			AMR.AMRSDK.setOnRewardedVideoComplete(OnVideoComplete);
			AMR.AMRSDK.setOnRewardedVideoDismiss(OnVideoDismiss);

			AMR.AMRSDK.setGDPRIsApplicable(isGDPRApplicable);

		}

		void OnApplicationPause(Boolean paused)
		{
			// IMPORTANT FOR CHARTBOOST, DO NOT FORGET
			if (!AMR.AMRSDK.initialized())
				return;

			if (paused)
			{
				AMR.AMRSDK.onPause();
				AMR.AMRSDK.onStop();
			}
			else
			{
				AMR.AMRSDK.onStart();
				AMR.AMRSDK.onResume();
			}
		}

		void OnApplicationQuit()
		{
			AMR.AMRSDK.onDestroy();
		}


		#region AdMethods

		public void LoadBanner()
		{
			this.Print("<AMRSDK> Load Banner");
			AMR.AMRSDK.loadBanner(AMR.Enums.AMRSDKBannerPosition.BannerPositionBottom, true); //autoshow disabled
		}

		public void HideBanner()
		{
			this.Print("<AMRSDK> Hide Banner");
			AMR.AMRSDK.hideBanner();
		}

		public void LoadInterstitial()
		{
			this.Print("<AMRSDK> Load Interstitial");
			AMR.AMRSDK.loadInterstitial();
		}

		public bool IsInterstitialReady()
		{
			return AMR.AMRSDK.isInterstitialReady();
		}

		public void ShowInterstitial()
		{
			this.Print("<AMRSDK> Show Interstitial");
			AMR.AMRSDK.showInterstitial();

		}

		public void LoadRewarded()
		{
			this.Print("<AMRSDK> Load RewardedVideo");
			AMR.AMRSDK.loadRewardedVideo();
		}

		public bool IsRewardedReady()
		{
			return AMR.AMRSDK.isRewardedVideoReady();
		}

		public void ShowRewarded()
		{
			this.Print("<AMRSDK> Show RewardedVideo");
			AMR.AMRSDK.showRewardedVideo();
		}

		#endregion



		#region Banner

		public void OnBannerReady(string networkName, double ecpm)
		{
			this.Print("<AMRSDK> OnBannerReady: " + networkName + " with ecpm: " + ecpm);
			IOnBannerReady?.Invoke(networkName, ecpm);
			AMR.AMRSDK.showBanner();
		}

		public void OnBannerFail(string error)
		{
			this.Print("<AMRSDK> OnBannerFail: " + error);
			IOnBannerFail?.Invoke(error);
		}

		public void OnBannerClick(string networkName)
		{
			this.Print("<AMRSDK> OnBannerClick: " + networkName);
			IOnBannerClick?.Invoke(networkName);
		}

		#endregion


		#region Interstitial

		public void OnInterstitialReady(string networkName, double ecpm)
		{
			this.Print("<AMRSDK> OnInterstitialReady: " + networkName + " with ecpm: " + ecpm);
			IOnInterstitialReady?.Invoke(networkName, ecpm);
		}

		public void OnInterstitialFail(string error)
		{
			this.Print("<AMRSDK> OnInterstitialFail: " + error);
			IOnInterstitialFail?.Invoke(error);
		}

		public void OnInterstitialShow()
		{
			this.Print("<AMRSDK> OnInterstitialShow");
			IOnInterstitialShow?.Invoke();
		}

		public void OnInterstitialFailToShow()
		{
			this.Print("<AMRSDK> OnInterstitialFailToShow");
			IOnInterstitialFailToShow?.Invoke(string.Empty);
		}

		public void OnInterstitialClick(string networkName)
		{
			this.Print("<AMRSDK> OnInterstitialClick: " + networkName);
			IOnInterstitialClick?.Invoke(networkName);
		}

		public void OnInterstitialDismiss()
		{
			this.Print("<AMRSDK> OnInterstitialDismiss");
			IOnInterstitialDismiss?.Invoke();
		}

		private void OnInterstitialStatusChange(int status)
		{
			this.Print("<AMRSDK> OnInterstitialStatusChange");
			IOnInterstitialStatusChange?.Invoke(status);
		}

		#endregion


		#region Rewarded CallBacks

		public void OnVideoReady(string networkName, double ecpm)
		{
			this.Print("<AMRSDK> OnVideoReady: " + networkName + " with ecpm: " + ecpm);
			IOnRewardedReady?.Invoke(networkName, ecpm);
		}

		public void OnVideoFail(string errorMessage)
		{
			this.Print("<AMRSDK> OnVideoFail called and reason is: " + errorMessage);
			IOnRewardedFail?.Invoke(errorMessage);
		}

		public void OnVideoShow()
		{
			this.Print("<AMRSDK> OnVideoShow");
			IOnRewardedShow?.Invoke();
		}

		public void OnVideoFailToShow()
		{
			this.Print("<AMRSDK> OnVideoFailToShow");
			IOnRewardedFailToShow?.Invoke(string.Empty);
		}

		public void OnVideoClick(string networkName)
		{
			this.Print("<AMRSDK> OnVideoClick: " + networkName);
			IOnRewardedClick?.Invoke(networkName);
		}

		public void OnVideoComplete()
		{
			this.Print("<AMRSDK> OnVideoComplete");
			IOnRewardedComplete?.Invoke();
		}

		public void OnVideoDismiss()
		{
			this.Print("<AMRSDK> OnVideoDismiss");
			IOnRewardedDismiss?.Invoke();
		}

		#endregion


		public void isGDPRApplicable(bool isGDPRApplicable)
		{
			this.Print("<AMRSDK> isGDPRApplicable : " + isGDPRApplicable);

			VegasHelpers.IsGDPRApplicable = isGDPRApplicable;
		}


		public void TrackIAPData(string transaction, decimal localizedPrice, string isoCurrencyCode, string[] tags)
		{
			if (transaction != string.Empty)
			{
#if UNITY_IOS
			AMR.AMRSDK.trackIAPForIOS(transaction, localizedPrice, isoCurrencyCode, tags);
#elif UNITY_ANDROID
				AMR.AMRSDK.trackIAPForAndroid(transaction, localizedPrice, isoCurrencyCode, tags);
#endif
			}
		}

		public void SetAttributionData(string adId)
		{
			AMR.AMRSDK.setAdjustUserId(adId);
		}


	}
#endif
}