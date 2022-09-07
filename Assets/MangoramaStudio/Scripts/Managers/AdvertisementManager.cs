using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PassionPunch;
using System;

public class AdvertisementManager : CustomBehaviour
{

    [SerializeField] private bool _isBannerActivated;
    [SerializeField] private bool _isInterstitialActivated;
    [SerializeField] private bool _isRewardedActivated;

    #region Initialize

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        if (_isBannerActivated)
        {
            AdManager.Instance.LoadBanner();
        }

        if (_isInterstitialActivated)
        {
            AdManager.Instance.LoadRewardedVideo();
        }

        if (_isRewardedActivated)
        {
            AdManager.Instance.LoadInterstitial();
        }
    }

    private void OnDestroy()
    {
    }

    #endregion


    public void LoadAndEnableBannerAd()
    {
        AdManager.Instance.LoadBanner();
    }

    public void HideBannerAd()
    {
        AdManager.Instance.HideBanner();
    }

    public void ShowRewardedVideo(Action GiveRewardCallback)
    {
        AdManager.Instance.ShowRewardedVideo((success) =>
        {
            if (success)
            {
                Debug.Log("Reward granted...");
                GiveRewardCallback?.Invoke();
            }
            else
            {
                Debug.Log("No Reward...");
            }
        }, (dismiss) => { });
    }

    public void ShowInterstitial()
    {
        AdManager.Instance.ShowInterstitial((success) =>
        {
            //TODO add game logic after ad
            if (success) // Interstitial shown successfully
            {
                AdManager.Instance.LoadInterstitial();
            }
            else // Interstitial didnt show
            {

            }
        });
    }
}
