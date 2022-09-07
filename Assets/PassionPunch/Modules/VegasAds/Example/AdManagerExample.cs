using UnityEngine;
using UnityEngine.UI;
using PassionPunch.Vegas;


public class AdManagerExample : MonoBehaviour
{
    [SerializeField] private Toggle bannerToggle;
    private AppState State;
    private void Start()
    {
        InvokeRepeating(nameof(CheckNetworkState), 2f, 2f);
        LoadInterstitial();
        LoadRewardedVideo();
        bannerToggle.onValueChanged.AddListener(delegate{ ShowBanner(bannerToggle); });;
    }

    #region Ad Methods
    public void ShowBanner(Toggle change)
    {
        if (State.Equals(AppState.OFFLINE))
        {
            return;
        }
        if (change.isOn)
        {
            Vegas.Instance.LoadBanner();
        }
        else
        {
            Vegas.Instance.HideBanner();
        }
    }
    
    public void LoadInterstitial()
    {
        if (State.Equals(AppState.OFFLINE))
        {
            return;
        }
        Vegas.Instance.LoadInterstitial();
    }

    public void ShowInterstitial()
    {
        if (Vegas.Instance.IsInterstitialReady())
        {
            Vegas.Instance.ShowInterstitial((success) =>
            {
                if (success)
                {
                    Debug.Log("Interstitial showed successfully!");
                }
            });
        }
        else
        {
            Debug.Log("Interstitial not ready!");
        }
    }

    public void LoadRewardedVideo()
    {
        if (State.Equals(AppState.OFFLINE))
        {
            return;
        }
        Vegas.Instance.LoadRewardedVideo();
    }

    public void ShowRewardedVideo()
    {
        if (Vegas.Instance.IsRewardedVideoReady())
        {
            Vegas.Instance.ShowRewardedVideo((success) =>
            {
                Debug.Log("RewardedVideo showed successfully!");
            },(dismiss)=>{});
        }
        else
        {
            Debug.Log("RewardedVideo not ready!");
        }
    }
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
    
    private enum AppState
    {
        OFFLINE,
        ONLINE
    }
    #endregion
}

