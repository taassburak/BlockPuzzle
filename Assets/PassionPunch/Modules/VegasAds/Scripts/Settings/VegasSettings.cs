using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegasSettings : ScriptableObject
{
    public bool isAdmostEnabled = false;

    public AdmostSettings admostSettings;
    public AppLovinMaxSettings appLovingSettings;
    public IronSourceSettings ironSourceSettings;
    public MoPubSettings moPubSettings;
}
