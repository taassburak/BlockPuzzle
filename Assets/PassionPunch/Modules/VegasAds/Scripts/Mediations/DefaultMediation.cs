using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PassionPunch.Vegas
{
    public class DefaultMediation : MonoBehaviour, IAdMediation
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

        public void HideBanner()
        {

        }

        public bool IsInterstitialReady()
        {
            return false;
        }

        public bool IsRewardedReady()
        {
            return false;
        }

        public void LoadBanner()
        {

        }

        public void LoadInterstitial()
        {

        }

        public void LoadRewarded()
        {

        }

        public void SetAttributionData(string adId)
        {

        }

        public void SetConfig(ISettings conf)
        {

        }

        public void ShowInterstitial()
        {

        }

        public void ShowRewarded()
        {

        }

        public void TrackIAPData(string transaction, decimal localizedPrice, string isoCurrencyCode, string[] tags)
        {

        }
    }
}