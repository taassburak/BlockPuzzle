using System;
using System.Collections;
using System.Collections.Generic;
using PassionPunch;
using UnityEngine;
using UnityEngine.UI;

public class AdControls : MonoBehaviour
{
    private bool showBanner = true;
    private void Start()
    {
        this.Print("Requesting banner ad...");
        AdManager.Instance.LoadBanner();
        this.Print("Requesting rewarded ad...");
        AdManager.Instance.LoadRewardedVideo();
        this.Print("Requesting interstitial ad...");
        AdManager.Instance.LoadInterstitial();
    }

    public void ToggleBannerAd()
    {
        showBanner = !showBanner;
        if (showBanner)
        {
            AdManager.Instance.LoadBanner();
        }
        else
        {
            AdManager.Instance.HideBanner();
        }
    }
    
    public void ShowRewardedVideo()
    {
        this.Print("Showing Rewarded Ad...");
        AdManager.Instance.ShowRewardedVideo((success) =>
        {
            if (success)
            {
                Debug.Log("Reward granted...");
            }
            else
            {
                Debug.Log("No Reward...");
            }
        },(dismiss)=>{});
    }
    
    public void ShowInterstitial()
    {
        this.Print("Showing Interstitial Ad...");
        AdManager.Instance.ShowInterstitial((success) =>
        {
            //TODO add game logic after ad
            if (success)
            {
                this.Print("Requesting new interstitial ad...");
                AdManager.Instance.LoadInterstitial();
            }
        });
    }
}
