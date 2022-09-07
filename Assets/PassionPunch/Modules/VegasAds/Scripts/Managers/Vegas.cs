using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PassionPunch.Vegas
{
	public class Vegas : MonoBehaviour
	{
		public static Vegas Instance;

		public VegasSettings vegasSettings;

		//#if PP_ADMOST
		//	public AdmostSettings settings;
		//#elif PP_APPLOVINMAX
		//	public AppLovinMaxSettings settings;
		//#elif PP_IRONSOURCE
		//	public IronSourceSettings settings;
		//#elif PP_MOPUB
		//	public MoPubSettings settings;
		//#else
		//	public AdmostSettings settings;
		//#endif

		private IAdMediation adMediation;

		private Action<bool> interCallback;
		private Action<bool> rewardGrantedCallback;
		private Action<bool> rewardDismissCallback;


		private GameStateWhileAd gameState;

		public event Action<string, double> VOnBannerReady;
		public event Action<string> VOnBannerFail;
		public event Action<string> VOnBannerClick;
		public event Action<string, double> VOnInterstitialReady;
		public event Action<string> VOnInterstitialFail;
		public event Action VOnInterstitialShow;
		public event Action<string> VOnInterstitialFailToShow;
		public event Action<string> VOnInterstitialClick;
		public event Action VOnInterstitialDismiss;
		public event Action<int> VOnInterstitialStatusChange;
		public event Action<string, double> VOnRewardedReady;
		public event Action<string> VOnRewardedFail;
		public event Action VOnRewardedShow;
		public event Action<string> VOnRewardedFailToShow;
		public event Action<string> VOnRewardedClick;
		public event Action VOnRewardedComplete;
		public event Action VOnRewardedDismiss;


		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(Instance);
			}
			else
			{
				Destroy(gameObject);
			}
		}


		private void Start()
		{
			gameState = gameObject.AddComponent<GameStateWhileAd>();
			SetUpMediation();
		}

		private void SetUpMediation()
		{
			SetConfig();
			SetEvents();
		}
		private void SetConfig()
		{
#if PP_ADMOST
			adMediation = gameObject.AddComponent<AdmostMediation>();
			adMediation.SetConfig(vegasSettings.admostSettings);
#elif PP_APPLOVINMAX
		adMediation = gameObject.AddComponent<AppLovinMaxMediation>();
		adMediation.SetConfig(vegasSettings.appLovingSettings);
#elif PP_IRONSOURCE
			adMediation = gameObject.AddComponent<IronSourceMediation>();
			adMediation.SetConfig(vegasSettings.ironSourceSettings);
#elif PP_MOPUB
		adMediation = gameObject.AddComponent<MoPubMediation>();
		adMediation.SetConfig(vegasSettings.moPubSettings);
#else
		adMediation = gameObject.AddComponent<DefaultMediation>();
		adMediation.SetConfig(vegasSettings.admostSettings); 
#endif
		}

		private void SetEvents()
		{
			//Banner
			adMediation.IOnBannerReady += OnBannerReady;
			adMediation.IOnBannerFail += OnBannerFail;
			adMediation.IOnBannerClick += OnBannerClick;
			//Interstitial
			adMediation.IOnInterstitialReady += OnInterstitialReady;
			adMediation.IOnInterstitialFail += OnInterstitialFail;
			adMediation.IOnInterstitialShow += OnInterstitialShow;
			adMediation.IOnInterstitialFailToShow += OnInterstitialFailToShow;
			adMediation.IOnInterstitialClick += OnInterstitialClick;
			adMediation.IOnInterstitialDismiss += OnInterstitialDismiss;
			adMediation.IOnInterstitialStatusChange += OnInterstitialStatusChange;
			//Rewarded
			adMediation.IOnRewardedReady += OnVideoReady;
			adMediation.IOnRewardedFail += OnVideoFail;
			adMediation.IOnRewardedShow += OnVideoShow;
			adMediation.IOnRewardedClick += OnVideoClick;
			adMediation.IOnRewardedDismiss += OnVideoDismiss;
			adMediation.IOnRewardedComplete += OnVideoComplete;
			adMediation.IOnRewardedFailToShow += OnVideoFailToShow;
		}
		#region AdMethods

		public void LoadBanner()
		{
			adMediation.LoadBanner();
		}

		public void HideBanner()
		{
			adMediation.HideBanner();
		}

		public void LoadInterstitial()
		{
			adMediation.LoadInterstitial();
		}

		public void ShowInterstitial(Action<bool> callback)
		{

			interCallback = callback;
			adMediation.ShowInterstitial();
		}
		
		public bool IsInterstitialReady()
		{
			return adMediation.IsInterstitialReady();
		}

		public void LoadRewardedVideo()
		{
			adMediation.LoadRewarded();
		}

		public void ShowRewardedVideo(Action<bool> grantCallback,Action<bool> dismissCallback)
		{
			rewardGrantedCallback = grantCallback;
			rewardDismissCallback = dismissCallback;
			adMediation.ShowRewarded();
		}

		
		public bool IsRewardedVideoReady()
		{
			return adMediation.IsRewardedReady();
		}

		#endregion



		#region Banner CallBacks
		private void OnBannerReady(string networkName, double ecpm)
		{
			VOnBannerReady?.Invoke(networkName, ecpm);
		}

		public void OnBannerFail(string error)
		{
			VOnBannerFail?.Invoke(error);
		}

		public void OnBannerClick(string networkName)
		{
			VOnBannerClick?.Invoke(networkName);
		}
		#endregion


		#region Interstitial CallBacks

		public void OnInterstitialReady(string networkName, double ecpm)
		{
			VOnInterstitialReady?.Invoke(networkName, ecpm);
		}

		public void OnInterstitialFail(string error)
		{
			interCallback?.Invoke(false);
			VOnInterstitialFail?.Invoke(error);
		}

		public void OnInterstitialShow()
		{
			gameState.PauseGame();
			VOnInterstitialShow?.Invoke();
		}

		public void OnInterstitialFailToShow(string error)
		{
			gameState.ResumeGame();
			interCallback.Invoke(false);
			VOnInterstitialFailToShow?.Invoke(error);
		}

		public void OnInterstitialClick(string networkName)
		{
			VOnInterstitialClick?.Invoke(networkName);
		}

		public void OnInterstitialDismiss()
		{
			gameState.ResumeGame();
			interCallback.Invoke(true);
			VOnInterstitialDismiss?.Invoke();
		}

		private void OnInterstitialStatusChange(int status)
		{

		}

		#endregion


		#region Rewarded CallBacks

		public void OnVideoReady(string networkName, double ecpm)
		{
			VOnRewardedReady?.Invoke(networkName, ecpm);
		}

		public void OnVideoFail(string errorMessage)
		{
			rewardDismissCallback?.Invoke(false);
			VOnRewardedFail?.Invoke(errorMessage);
		}

		public void OnVideoShow()
		{
			gameState.PauseGame();
			VOnRewardedShow?.Invoke();
		}

		public void OnVideoFailToShow(string error)
		{
			gameState.ResumeGame();
			rewardDismissCallback?.Invoke(false);
			VOnRewardedFailToShow?.Invoke(error);
		}

		public void OnVideoClick(string networkName)
		{
			VOnRewardedClick?.Invoke(networkName);
		}

		public void OnVideoComplete()
		{
			rewardGrantedCallback?.Invoke(true);
			VOnRewardedComplete?.Invoke();
		}

		public void OnVideoDismiss()
		{
			gameState.ResumeGame();
			rewardDismissCallback?.Invoke(true);
			VOnRewardedDismiss?.Invoke();
		}

		#endregion


		#region Custom Analytic Method

		public void TrackIAPData(string transaction, decimal localizedPrice, string isoCurrencyCode, string[] tags)
		{
			try
			{
				this.Print(
					$"Transaction: {transaction} Price: {localizedPrice} Iso: {isoCurrencyCode} Tags: {string.Join(",", tags)}");
				adMediation.TrackIAPData(transaction, localizedPrice, isoCurrencyCode, tags);
			}
			catch (Exception ex)
			{
				this.Print(ex.Message);
			}
		}

		public void SetAttributionData(string adId)
		{
			try
			{
				adMediation.SetAttributionData(adId);
			}
			catch (Exception ex)
			{
				this.Print(ex.Message);
			}
		}

		#endregion
	}

}
