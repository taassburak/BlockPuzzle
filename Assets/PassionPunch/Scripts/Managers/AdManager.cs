using System;
using System.Collections;
using System.Collections.Generic;
using PassionPunch;
using PassionPunch.Sherlock;
using PassionPunch.Vegas;
using UnityEngine;

public class AdManager  : MonoSingleton<AdManager>
{
	private List<AdType> adRequests = new List<AdType>();
	private Dictionary<AdType, bool> adStatus = new Dictionary<AdType, bool>();
	private enum AdType { Banner, Rewarded, Interstitial }
	
    public void Initialize()
    {
        
    }
	
    private void Start()
    {
	    adStatus.Add(AdType.Banner,false);
	    adStatus.Add(AdType.Interstitial,false);
	    adStatus.Add(AdType.Rewarded,false);
	    
        //Banner
        Vegas.Instance.VOnBannerReady += OnBannerReady;
        Vegas.Instance.VOnBannerFail += OnBannerFail;
        Vegas.Instance.VOnBannerClick += OnBannerClick;
        //Interstitial
        Vegas.Instance.VOnInterstitialReady += OnInterstitialReady;
        Vegas.Instance.VOnInterstitialFail += OnInterstitialFail;
        Vegas.Instance.VOnInterstitialShow += OnInterstitialShow;
        Vegas.Instance.VOnInterstitialFailToShow += OnInterstitialFailToShow;
        Vegas.Instance.VOnInterstitialClick += OnInterstitialClick;
        Vegas.Instance.VOnInterstitialDismiss += OnInterstitialDismiss;
        //Rewarded
        Vegas.Instance.VOnRewardedReady += OnVideoReady;
        Vegas.Instance.VOnRewardedFail += OnVideoFail;
        Vegas.Instance.VOnRewardedShow += OnVideoShow;
        Vegas.Instance.VOnRewardedClick += OnVideoClick;
        Vegas.Instance.VOnRewardedDismiss += OnVideoDismiss;
        Vegas.Instance.VOnRewardedComplete += OnVideoComplete;
        Vegas.Instance.VOnRewardedFailToShow += OnVideoFailToShow;

    }
    
    private void OnApplicationPause(bool pauseStatus)
    {
	    if (!pauseStatus)
	    {
		    //means game comes from background not first init
		    if (adStatus.Count > 0)
		    {
			    adStatus[AdType.Banner] = false;
			    adStatus[AdType.Interstitial] = Vegas.Instance.IsInterstitialReady();
			    adStatus[AdType.Rewarded] = Vegas.Instance.IsRewardedVideoReady();
		    }
	    }
    }
    
    #region AdMethods

	public void LoadBanner()
	{
		if (AppManager.Instance.SubsState.Equals(AppSubsState.SUBSCRIBED) || AppManager.Instance.ConsumableState.Equals(AppConsumableState.REMOVEADS))
		{
			return;
		}
		if (AppManager.Instance.State.Equals(AppState.ONLINE))
		{
			if (!AppSettings.BannerAdEnabled || adRequests.Contains(AdType.Banner))
			{
				return;
			}
			Vegas.Instance.LoadBanner();
			adRequests.Add(AdType.Banner);
		}
	}

	public void HideBanner()
	{
		Vegas.Instance.HideBanner();
		adRequests.Remove(AdType.Banner);
		adStatus[AdType.Banner] = false;
	}

	public void LoadInterstitial()
	{
		if (AppManager.Instance.SubsState.Equals(AppSubsState.SUBSCRIBED) || AppManager.Instance.ConsumableState.Equals(AppConsumableState.REMOVEADS))
		{
			return;
		}
		
		if (AppManager.Instance.State.Equals(AppState.ONLINE))
		{
			if (!AppSettings.InterstitialAdEnabled && adRequests.Contains(AdType.Interstitial))
			{
				return;
			}
			Vegas.Instance.LoadInterstitial();
			adRequests.Add(AdType.Interstitial);
		}

	}

	public void ShowInterstitial(Action<bool> callback)
	{
		if (AppManager.Instance.SubsState.Equals(AppSubsState.SUBSCRIBED) || AppManager.Instance.ConsumableState.Equals(AppConsumableState.REMOVEADS))
		{
			callback?.Invoke(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor);
		}
		else
		{
			if (adStatus[AdType.Interstitial])
			{
				Vegas.Instance.ShowInterstitial(callback);
			}
			else
			{
#if PP_FIREBASE
				Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Interstitial_Ready_Fail);
#endif
				this.Print("Show Interstitial - Not Ready - Return");
				callback?.Invoke(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor);
			}
		}
	}

	public void LoadRewardedVideo()
	{
		if (AppManager.Instance.State.Equals(AppState.ONLINE))
		{
			if (!AppSettings.RewardedAdEnabled && adRequests.Contains(AdType.Rewarded))
			{
				return;
			}
			Vegas.Instance.LoadRewardedVideo();
			adRequests.Add(AdType.Rewarded);
		}
	}

	public void ShowRewardedVideo(Action<bool> grantCallback, Action<bool> dismissCallback)
	{
		if (adStatus[AdType.Rewarded])
		{
			Vegas.Instance.ShowRewardedVideo(grantCallback,dismissCallback);
		}
		else
		{
#if PP_FIREBASE
			Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Rewarded_Ready_Fail);
#endif
			this.Print("Show RewardedVideo - Not Ready - Return");
			grantCallback?.Invoke(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor);
			dismissCallback?.Invoke(Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor);
		}
	}

#endregion



#region Banner CallBacks
	private void OnBannerReady(string networkName, double ecpm)
    {
	    adStatus[AdType.Banner] = true;
#if PP_FIREBASE
	    Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Banner_Show);
#endif
	    var ecpmBanner = ecpm > 0 ? ecpm : 0;
	    if (ecpmBanner != 0)
	    {
#if UNITY_IOS && PP_ADJUST
		    Sherlock.Instance.AdjustRevenueEvent(ProjectConstants.Events.Ads_Revenue_Banner, ecpmBanner);
#elif UNITY_ANDROID && PP_ADJUST
			Sherlock.Instance.AdjustRevenueEvent(ProjectConstants.Events.Ads_Revenue_Banner, ecpmBanner);
#endif
		}

	}

	private void OnBannerFail(string error)
    {
	    adStatus[AdType.Banner] = false;
#if PP_FIREBASE
	    Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Banner_Show_Fail);
		if (error != string.Empty)
        {
	        Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Banner_Fail, ProjectConstants.Events.Error, error.Replace(' ', '_').ToLower());
        }
#endif
    }

    private void OnBannerClick(string networkName)
    {
#if PP_FIREBASE
	    Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Banner_Click);
        if (networkName != string.Empty)
        {
	        Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Banner_Clicked_Network, ProjectConstants.Events.Network, networkName.ToLower());
        }
#endif

    }
#endregion


#region Interstitial CallBacks

	private void OnInterstitialReady(string networkName, double ecpm)
	{
		adStatus[AdType.Interstitial] = true;
		var ecpmInter = ecpm > 0 ? ecpm : 0;
		if (ecpmInter != 0)
		{
#if UNITY_IOS && PP_ADJUST
			Sherlock.Instance.AdjustRevenueEvent(ProjectConstants.Events.Ads_Revenue_Inter, ecpmInter);
#elif UNITY_ANDROID && PP_ADJUST
			Sherlock.Instance.AdjustRevenueEvent(ProjectConstants.Events.Ads_Revenue_Inter, ecpmInter);
#endif
		}
	}

	private void OnInterstitialFail(string error)
	{
		adStatus[AdType.Interstitial] = true;
		adRequests.Remove(AdType.Interstitial);
#if PP_FIREBASE
		Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Interstitial_Sdk_Fail);
		if (error != string.Empty)
		{
			Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Interstitial_Fail, ProjectConstants.Events.Error, error.Replace(' ', '_').ToLower());
		}
#endif
	}

	private void OnInterstitialShow()
	{
#if PP_FIREBASE
		Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Interstitial_Show);
#endif
	}

	private void OnInterstitialFailToShow(string error)
	{
		adStatus[AdType.Interstitial] = Vegas.Instance.IsInterstitialReady();
		adRequests.Remove(AdType.Interstitial);
#if PP_FIREBASE
		Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Interstitial_Show_fail);
		if(error != string.Empty)
        {
	        Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Interstitial_Fail, ProjectConstants.Events.Error, error.Replace(' ', '_').ToLower());
        }
#endif
	}

	private void OnInterstitialClick(string networkName)
	{
#if PP_FIREBASE
		Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Interstitial_Click);
		if (networkName != string.Empty)
		{
			Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Interstitial_Click_Network, ProjectConstants.Events.Network, networkName.ToLower());
		}
#endif
	}

	private void OnInterstitialDismiss()
	{
		adStatus[AdType.Interstitial] = false;
		adRequests.Remove(AdType.Interstitial);
	}
	

#endregion


#region Rewarded CallBacks

	private void OnVideoReady(string networkName, double ecpm)
	{
		adStatus[AdType.Rewarded] = true;
		var ecpmRewarded = ecpm > 0 ? ecpm : 0;
		if (ecpmRewarded != 0)
		{
#if UNITY_IOS && PP_ADJUST
			Sherlock.Instance.AdjustRevenueEvent(ProjectConstants.Events.Ads_Revenue_Rewarded, ecpmRewarded);
#elif UNITY_ANDROID && PP_ADJUST
			Sherlock.Instance.AdjustRevenueEvent(ProjectConstants.Events.Ads_Revenue_Rewarded, ecpmRewarded);
#endif
		}
	}

	private void OnVideoFail(string errorMessage)
	{
		adStatus[AdType.Rewarded] = false;
		adRequests.Remove(AdType.Rewarded);
#if PP_FIREBASE
		Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Rewarded_Sdk_Fail);
		if (errorMessage != string.Empty)
        {
	        Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Rewarded_Fail, ProjectConstants.Events.Error, errorMessage.Replace(' ', '_').ToLower());
        }
#endif
	}

	private void OnVideoShow()
	{
#if PP_FIREBASE
		Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Rewarded_Show);
#endif
	}

	private void OnVideoFailToShow(string error)
	{
		adStatus[AdType.Rewarded] = false;
		adRequests.Remove(AdType.Rewarded);
#if PP_FIREBASE
		Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Rewarded_Show_Fail);
		if (error != string.Empty)
		{
			Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Rewarded_Fail, ProjectConstants.Events.Error, error.Replace(' ', '_').ToLower());
		}
#endif
	}

	private void OnVideoClick(string networkName)
	{
#if PP_FIREBASE
		Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Rewarded_Click);
		if (networkName != string.Empty)
        {
	        //Sherlock.Instance.Firebase_TrackEvent(ProjectConstants.Events.Rewarded_Click_Network , ProjectConstants.Events.Network, networkName.ToLower());
        }
#endif
	}

	private void OnVideoComplete()
	{
		adRequests.Remove(AdType.Rewarded);
	}

	private void OnVideoDismiss()
	{
		adStatus[AdType.Rewarded] = false;
		adRequests.Remove(AdType.Rewarded);
		LoadRewardedVideo();
	}

#endregion


#region Custom Analytic Method
	public void SetAttributionData(string adId)
    {
		try
		{
			Vegas.Instance.SetAttributionData(adId);
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

#endregion
	
}
