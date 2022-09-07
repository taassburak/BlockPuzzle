using System;
using System.Collections.Generic;
#if PP_FIREBASE
using Firebase.Crashlytics;
using Firebase.Extensions;
#endif
using MEC;
using ProjectConstants;
using UnityEngine;
using System.Threading.Tasks;
using PassionPunch;

public class AppManager : MonoSingleton<AppManager>
{
    public AppState State = AppState.ONLINE;
    public static Stash Config;

    private bool firebaseInitialized;

    private AppSubsState m_state = AppSubsState.EXPIRED;
    public event Action SubsStateChanged;
    public AppSubsState SubsState
    {
        get
        {
            return m_state;
        }
        set
        {
            if (value != m_state)
            {
                AppSubsState oldState = m_state;
                m_state = value;

                SubsStateChanged?.Invoke();
            }
        }
    }

    private AppConsumableState m_consState = AppConsumableState.NONE;
    public event Action ConsumableStateChanged;
    public AppConsumableState ConsumableState
    {
        get
        {
            return m_consState;
        }
        set
        {
            if (value != m_consState)
            {
                m_consState = value;
                ConsumableStateChanged?.Invoke();
            }
        }
    }

    
    public void Initialize()
    {
        this.Print("Application manager initialized");
        Config = Stash.PersistentPath("AppSettings", OnStashError);        
        AppSettings.BannerAdEnabled = Config.Get(AppConstants.Banner_Ads_Enabled, true);
        AppSettings.RewardedAdEnabled = Config.Get(AppConstants.Rewarded_Ads_Enabled, true);
        AppSettings.InterstitialAdEnabled = Config.Get(AppConstants.Interstitial_Ads_Enabled, true);
        AppSettings.PurchaseEnabled = Config.Get(AppConstants.Purchase_Enabled, true);
    }
    
    private void OnStashError(StashError error)
    {
        Debug.Log(error);
    }

    #region Application Lifecycle Operations
    private void Start()
    {
        InvokeRepeating(nameof(CheckNetworkState), 2f, 2f);
    }

    // on application killed
    private void OnApplicationQuit()
    {
        Config.Save();
    }

    // on application suspended (home button)
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Config.Save();
        }
    }
    #endregion

    #region Firebase Operations

#if PP_FIREBASE
    

    public IEnumerator<float> C_InitializeFirebaseWithRemoteConfig()
    {
        if (State.Equals(AppState.ONLINE))
        {
            try
            {
                Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
                {
                    Firebase.DependencyStatus dependencyStatus = task.Result;
                    if (dependencyStatus == Firebase.DependencyStatus.Available)
                    {
                        InitializeFirebaseComponents();
                    }
                    else
                    {
                        this.Print("Could not resolve all Firebase dependencies: " + dependencyStatus);
                    }
                });
            }
            catch (Exception ex)
            {
                State = AppState.OFFLINE;
                this.Print(ex.Message);
            }


            float threshold = 0f;
            while (!firebaseInitialized && threshold < 7f) //wait enough time otherwise start offline 
            {
                threshold += 0.01f;
                yield return Timing.WaitForSeconds(0.01f);
            }
            if (!firebaseInitialized)
            {
                State = AppState.OFFLINE;
            }
            else
            {
                try
                {
                    // Remote Config data has been fetched, so this applies it for this play session:
                    Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync();

                    //TODO Configure other firebase Components

                    Firebase.AppOptions ops = new Firebase.AppOptions();
                    Firebase.FirebaseApp.Create(ops);
                    SetRemoteConfigValues();
                }
                catch (Exception ex)
                {
                    State = AppState.OFFLINE;
                    this.Print(ex.Message);
                }
            }
        }
        yield return Timing.WaitForSeconds(0.01f);

    }

    void InitializeFirebaseComponents()
    {
        this.Print("Setting default values for remote config..");
        Dictionary<string, object> remoteConfigDefaults = PrepareRemoteConfigDefaults();
        Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(remoteConfigDefaults).ContinueWithOnMainThread(task => {
            FetchDataAsync();
        });
    }

    private Task FetchDataAsync() {
        this.Print("Fetching remote config...");
        System.Threading.Tasks.Task fetchTask =
            Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(
                TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask) {
        firebaseInitialized = true;
        if (fetchTask.IsCanceled) {
            this.Print("Fetch remote config canceled.");
        } else if (fetchTask.IsFaulted) {
            this.Print("Fetch remote config encountered an error.");
        } else if (fetchTask.IsCompleted) {
            this.Print("Fetch remote config completed successfully!");
        }

        var info = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.Info;
        switch (info.LastFetchStatus) {
            case Firebase.RemoteConfig.LastFetchStatus.Success:
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(task => {
                        this.Print($"Remote data loaded and ready (last fetch time {info.FetchTime}).");
                    });

                break;
            case Firebase.RemoteConfig.LastFetchStatus.Failure:
                switch (info.LastFetchFailureReason) {
                    case Firebase.RemoteConfig.FetchFailureReason.Error:
                        this.Print("Fetch remote config failed for unknown reason");
                        break;
                    case Firebase.RemoteConfig.FetchFailureReason.Throttled:
                        this.Print("Fetch remote config throttled until " + info.ThrottledEndTime);
                        break;
                }
                break;
            case Firebase.RemoteConfig.LastFetchStatus.Pending:
                this.Print("Latest Fetch remote config call still pending.");
                break;
        }
    }


    private Dictionary<string, object> PrepareRemoteConfigDefaults()
    {
        Dictionary<string, object> defaults = new Dictionary<string, object>();
        defaults.Add(AppConstants.Rewarded_Ads_Enabled, AppSettings.RewardedAdEnabled);
        defaults.Add(AppConstants.Banner_Ads_Enabled, AppSettings.BannerAdEnabled);
        defaults.Add(AppConstants.Interstitial_Ads_Enabled, AppSettings.InterstitialAdEnabled);
        defaults.Add(AppConstants.Purchase_Enabled, AppSettings.PurchaseEnabled);
        return defaults;
    }

    private void SetRemoteConfigValues()
    {
        if (State.Equals(AppState.ONLINE))
        {
            try
            {
                AppSettings.InterstitialAdEnabled = GetRemoteConfigBoolValue(AppConstants.Interstitial_Ads_Enabled);
                AppSettings.BannerAdEnabled = GetRemoteConfigBoolValue(AppConstants.Banner_Ads_Enabled);
                AppSettings.RewardedAdEnabled = GetRemoteConfigBoolValue(AppConstants.Rewarded_Ads_Enabled);
                AppSettings.PurchaseEnabled = GetRemoteConfigBoolValue(AppConstants.Purchase_Enabled);

                Config.Set(AppConstants.Interstitial_Ads_Enabled, AppSettings.InterstitialAdEnabled);
                Config.Set(AppConstants.Rewarded_Ads_Enabled, AppSettings.RewardedAdEnabled);
                Config.Set(AppConstants.Banner_Ads_Enabled, AppSettings.BannerAdEnabled);
                Config.Set(AppConstants.Purchase_Enabled, AppSettings.PurchaseEnabled);
            }
            catch (Exception ex)
            {
                this.Print(ex.Message);
                Crashlytics.LogException(ex);
            }
            
        }

    }

    private string GetRemoteConfigStringValue(string key)
    {
        Firebase.RemoteConfig.ConfigValue remote = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key);
        return remote.StringValue;
    }
    private bool GetRemoteConfigBoolValue(string key)
    {
        Firebase.RemoteConfig.ConfigValue remote = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key);
        return remote.BooleanValue;
    }
    
#endif
    #endregion
    
    #region Utilities
    private void CheckNetworkState()
    {
        State = !IsHaveInternetConnection() ? AppState.OFFLINE : AppState.ONLINE;
    }

    private static bool IsHaveInternetConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    #endregion

}

