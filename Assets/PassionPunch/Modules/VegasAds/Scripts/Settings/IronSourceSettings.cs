using UnityEngine;

public class IronSourceSettings : ScriptableObject, ISettings
{
    [Header("IOS")]
    public string ApplicationIdIOS;

    [Header("Android")]
    public string ApplicationIdAndroid;
}

