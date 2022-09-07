using UnityEngine;
using UnityEngine.Serialization;

public class MoPubSettings : ScriptableObject, ISettings
{
    [Header("iOS")]
    public string[] bannerIdIOS;
    public string[] interstitialIdIOS;
    public string[] rewardedVideoIdIOS;

    [Header("Android")]
    public string[] bannerIdAndroid;
    public string[] interstitialIdAndroid;
    public string[] rewardedVideoIdAndroid;
}
