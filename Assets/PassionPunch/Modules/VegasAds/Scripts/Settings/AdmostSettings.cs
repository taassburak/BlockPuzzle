using UnityEngine;

public class AdmostSettings : ScriptableObject, ISettings
{
    [Header("IOS")]
    public string ApplicationIdIOS;
    public string BannerIdIOS;
    public string InterstitialIdIOS;
    public string RewardedVideoIdIOS;

    [Header("Android")]
    public string ApplicationIdAndroid;
    public string BannerIdAndroid;
    public string InterstitialIdAndroid;
    public string RewardedVideoIdAndroid;
}
