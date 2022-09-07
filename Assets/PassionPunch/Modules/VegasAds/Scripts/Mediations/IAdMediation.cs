using System;
namespace PassionPunch.Vegas
{
	public interface IAdMediation
	{
		//Ad Methods
		void LoadBanner();
		void HideBanner();
		void LoadInterstitial();
		bool IsInterstitialReady();
		void ShowInterstitial();
		void LoadRewarded();
		bool IsRewardedReady();
		void ShowRewarded();

		//Banner
		event Action<string, double> IOnBannerReady;
		event Action<string> IOnBannerFail;
		event Action<string> IOnBannerClick;

		//Interstial
		event Action<string, double> IOnInterstitialReady;
		event Action<string> IOnInterstitialFail;
		event Action IOnInterstitialShow;
		event Action<string> IOnInterstitialFailToShow;
		event Action<string> IOnInterstitialClick;
		event Action IOnInterstitialDismiss;
		event Action<int> IOnInterstitialStatusChange;

		//Rewarded
		event Action<string, double> IOnRewardedReady;
		event Action<string> IOnRewardedFail;
		event Action IOnRewardedShow;
		event Action<string> IOnRewardedFailToShow;
		event Action<string> IOnRewardedClick;
		event Action IOnRewardedComplete;
		event Action IOnRewardedDismiss;

		//Analytic
		void TrackIAPData(string transaction, decimal localizedPrice, string isoCurrencyCode, string[] tags);
		void SetAttributionData(string adId);
		void SetConfig(ISettings conf);
	}
}
