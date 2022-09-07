using UnityEngine;

public class AppLovinMaxSettings : ScriptableObject, ISettings
{
    public string AppLovinSDKKey;

    [Header("IOS")]
    public string BannerIdIOS;
    public string InterstitialIdIOS;
    public string RewardedVideoIdIOS;

    [Header("Android")]
    public string BannerIdAndroid;
    public string InterstitialIdAndroid;
    public string RewardedVideoIdAndroid;
}
