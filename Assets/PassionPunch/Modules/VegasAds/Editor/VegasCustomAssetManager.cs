using UnityEngine;
using UnityEditor;

public class VegasCustomAssetManager
{
    [MenuItem("PassionPunch/Vegas/Create/VegasSettings")]
    public static void CreateVegasSettings()
    {
        VegasSettings vegasSettings = ScriptableObject.CreateInstance<VegasSettings>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/Settings/VegasSettings" + ".asset");
        AssetDatabase.CreateAsset(vegasSettings, assetPathAndName);
        MutualExecutions();
        Selection.activeObject = vegasSettings;
    }
    [MenuItem("PassionPunch/Vegas/Create/AdmostSettings")]
    public static void CreateAdmostSettings()
    {
        AdmostSettings vegasSettings = ScriptableObject.CreateInstance<AdmostSettings>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/VegasAds/AdmostSettings" + ".asset");
        AssetDatabase.CreateAsset(vegasSettings, assetPathAndName);
        MutualExecutions();
        Selection.activeObject = vegasSettings;
    }

    [MenuItem("PassionPunch/Vegas/Create/IronSourceSettings")]
    public static void CreateIronSourceSettings()
    {
        IronSourceSettings vegasSettings = ScriptableObject.CreateInstance<IronSourceSettings>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/VegasAds/IronSourceSettings" + ".asset");
        AssetDatabase.CreateAsset(vegasSettings, assetPathAndName);
        MutualExecutions();
        Selection.activeObject = vegasSettings;
    }

    [MenuItem("PassionPunch/Vegas/Create/MoPubSettings")]
    public static void CreateMoPubSettings()
    {
        MoPubSettings vegasSettings = ScriptableObject.CreateInstance<MoPubSettings>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/VegasAds/MoPubSettings" + ".asset");
        AssetDatabase.CreateAsset(vegasSettings, assetPathAndName);
        MutualExecutions();
        Selection.activeObject = vegasSettings;
    }

    [MenuItem("PassionPunch/Vegas/Create/AppLovinMaxSettings")]
    public static void CreateAppLovinMaxSettings()
    {
        AppLovinMaxSettings vegasSettings = ScriptableObject.CreateInstance<AppLovinMaxSettings>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/VegasAds/AppLovinMaxSettings" + ".asset");
        AssetDatabase.CreateAsset(vegasSettings, assetPathAndName);
        MutualExecutions();
        Selection.activeObject = vegasSettings;
    }

    private static void MutualExecutions()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
    }
}
