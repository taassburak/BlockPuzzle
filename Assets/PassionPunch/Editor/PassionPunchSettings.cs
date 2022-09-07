using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PassionPunchSettings : ScriptableObject
{
    [Header("Analytic Settings")]
    [SerializeField] private bool sherlockEnabled;
    public bool SherlockEnabled => sherlockEnabled;
    [SerializeField] private bool adjustEnabled;
    public bool AdjustEnabled => adjustEnabled;
    [SerializeField] private bool firebaseEnabled;
    public bool FirebaseEnabled => firebaseEnabled;
    
    [SerializeField] private bool facebookEnabled;
    public bool FacebookEnabled => facebookEnabled;

    [Space(10)] [Header("Firebase Remote Config Keys")] 
    
    [SerializeField] List<string> remoteConfigKeys;
    public List<string> RemoteConfigKeys => remoteConfigKeys;
    
    [Space(10)]
    [Header("Vegas Ad Mediation Settings")]
    [SerializeField] private bool admostMediationEnabled;
    public bool AdmostMediationEnabled => admostMediationEnabled;
    [SerializeField] private bool applovinmaxMediationEnabled;
    public bool ApplovinmaxMediationEnabled => applovinmaxMediationEnabled;
    [SerializeField] private bool mopubMediationEnabled;
    public bool MopubMediationEnabled => mopubMediationEnabled;
    [SerializeField] private bool ironsourceMediationEnabled;
    public bool IronsourceMediationEnabled => ironsourceMediationEnabled;
    [Space(10)]
    [Header("IAP Settings")]
    [SerializeField] private bool unityIAPEnabled;
    public bool UnityIAPEnabled => unityIAPEnabled;
    [SerializeField] private bool revenuecatEnabled;
    public bool RevenuecatEnabled => revenuecatEnabled;
    [Space(10)]
    [Header("Other Settings")]
    [SerializeField] private bool verboseDebugLogEnabled;
    public bool VerboseDebugLogEnabled => verboseDebugLogEnabled;
    
}
