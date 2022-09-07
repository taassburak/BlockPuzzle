// This file is auto-generated. Modifications are not saved.

namespace ProjectConstants
{
    public enum Scenes
    {
        Init,
        Game,
        END
    }

    public static class Tags
    {
        /// <summary>
        /// Name of tag 'Untagged'.
        /// </summary>
        public const string Untagged = "Untagged";
        /// <summary>
        /// Name of tag 'Respawn'.
        /// </summary>
        public const string Respawn = "Respawn";
        /// <summary>
        /// Name of tag 'Finish'.
        /// </summary>
        public const string Finish = "Finish";
        /// <summary>
        /// Name of tag 'EditorOnly'.
        /// </summary>
        public const string EditorOnly = "EditorOnly";
        /// <summary>
        /// Name of tag 'MainCamera'.
        /// </summary>
        public const string MainCamera = "MainCamera";
        /// <summary>
        /// Name of tag 'Player'.
        /// </summary>
        public const string Player = "Player";
        /// <summary>
        /// Name of tag 'GameController'.
        /// </summary>
        public const string GameController = "GameController";
    }

    public static class SortingLayers
    {
        /// <summary>
        /// ID of sorting layer 'Default'.
        /// </summary>
        public const int Default = 0;
    }

    public static class Layers
    {
        /// <summary>
        /// Index of layer 'Default'.
        /// </summary>
        public const int Default = 0;
        /// <summary>
        /// Index of layer 'TransparentFX'.
        /// </summary>
        public const int TransparentFX = 1;
        /// <summary>
        /// Index of layer 'Ignore Raycast'.
        /// </summary>
        public const int Ignore_Raycast = 2;
        /// <summary>
        /// Index of layer 'Water'.
        /// </summary>
        public const int Water = 4;
        /// <summary>
        /// Index of layer 'UI'.
        /// </summary>
        public const int UI = 5;

        /// <summary>
        /// Bitmask of layer 'Default'.
        /// </summary>
        public const int DefaultMask = 1 << 0;
        /// <summary>
        /// Bitmask of layer 'TransparentFX'.
        /// </summary>
        public const int TransparentFXMask = 1 << 1;
        /// <summary>
        /// Bitmask of layer 'Ignore Raycast'.
        /// </summary>
        public const int Ignore_RaycastMask = 1 << 2;
        /// <summary>
        /// Bitmask of layer 'Water'.
        /// </summary>
        public const int WaterMask = 1 << 4;
        /// <summary>
        /// Bitmask of layer 'UI'.
        /// </summary>
        public const int UIMask = 1 << 5;
    }

    public static class SceneIDs
    {
        /// <summary>
        /// ID of scene 'Init'.
        /// </summary>
        public const int Init = 0;
        /// <summary>
        /// ID of scene 'Game'.
        /// </summary>
        public const int Game = 1;
    }

    public static class SceneNames
    {
          public const string INVALID_SCENE = "InvalidScene";
    public static readonly string[] ScenesNameArray = {
	        "Init",
	        "Game"
        };
        /// <summary>
        /// Convert from enum to string
        /// </summary>
        public static string GetSceneName(Scenes scene) {
              int index = (int)scene;
              if(index > 0 && index < ScenesNameArray.Length) {
                  return ScenesNameArray[index];
              } else {
                  return INVALID_SCENE;
              }
        }
    }

    public static class Axes
    {
        /// <summary>
        /// Input axis 'Horizontal'.
        /// </summary>
        public const string Horizontal = "Horizontal";
        /// <summary>
        /// Input axis 'Vertical'.
        /// </summary>
        public const string Vertical = "Vertical";
        /// <summary>
        /// Input axis 'Fire1'.
        /// </summary>
        public const string Fire1 = "Fire1";
        /// <summary>
        /// Input axis 'Fire2'.
        /// </summary>
        public const string Fire2 = "Fire2";
        /// <summary>
        /// Input axis 'Fire3'.
        /// </summary>
        public const string Fire3 = "Fire3";
        /// <summary>
        /// Input axis 'Jump'.
        /// </summary>
        public const string Jump = "Jump";
        /// <summary>
        /// Input axis 'Mouse X'.
        /// </summary>
        public const string Mouse_X = "Mouse X";
        /// <summary>
        /// Input axis 'Mouse Y'.
        /// </summary>
        public const string Mouse_Y = "Mouse Y";
        /// <summary>
        /// Input axis 'Mouse ScrollWheel'.
        /// </summary>
        public const string Mouse_ScrollWheel = "Mouse ScrollWheel";
        /// <summary>
        /// Input axis 'Submit'.
        /// </summary>
        public const string Submit = "Submit";
        /// <summary>
        /// Input axis 'Cancel'.
        /// </summary>
        public const string Cancel = "Cancel";
    }
    public static class AppConstants
    {
        /// <summary>
        /// Setting 'SherlockEnabled'.
        /// </summary>
        public const string SherlockEnabled = "True";
        /// <summary>
        /// Setting 'AdjustEnabled'.
        /// </summary>
        public const string AdjustEnabled = "True";
        /// <summary>
        /// Setting 'FirebaseEnabled'.
        /// </summary>
        public const string FirebaseEnabled = "True";
        /// <summary>
        /// Setting 'FacebookEnabled'.
        /// </summary>
        public const string FacebookEnabled = "False";
        /// <summary>
        /// Firebase Remote Config Keys 'Rewarded_Ads_Enabled'.
        /// </summary>
        public const string Rewarded_Ads_Enabled = "Rewarded_Ads_Enabled";
        /// <summary>
        /// Firebase Remote Config Keys 'Interstitial_Ads_Enabled'.
        /// </summary>
        public const string Interstitial_Ads_Enabled = "Interstitial_Ads_Enabled";
        /// <summary>
        /// Firebase Remote Config Keys 'Banner_Ads_Enabled'.
        /// </summary>
        public const string Banner_Ads_Enabled = "Banner_Ads_Enabled";
        /// <summary>
        /// Firebase Remote Config Keys 'Purchase_Enabled'.
        /// </summary>
        public const string Purchase_Enabled = "Purchase_Enabled";
        /// <summary>
        /// Setting 'AdmostMediationEnabled'.
        /// </summary>
        public const string AdmostMediationEnabled = "True";
        /// <summary>
        /// Setting 'ApplovinmaxMediationEnabled'.
        /// </summary>
        public const string ApplovinmaxMediationEnabled = "False";
        /// <summary>
        /// Setting 'MopubMediationEnabled'.
        /// </summary>
        public const string MopubMediationEnabled = "False";
        /// <summary>
        /// Setting 'IronsourceMediationEnabled'.
        /// </summary>
        public const string IronsourceMediationEnabled = "False";
        /// <summary>
        /// Setting 'UnityIAPEnabled'.
        /// </summary>
        public const string UnityIAPEnabled = "False";
        /// <summary>
        /// Setting 'RevenuecatEnabled'.
        /// </summary>
        public const string RevenuecatEnabled = "False";
        /// <summary>
        /// Setting 'VerboseDebugLogEnabled'.
        /// </summary>
        public const string VerboseDebugLogEnabled = "True";
        /// <summary>
        /// Setting 'name'.
        /// </summary>
        public const string name = "PassionPunchSettings";
        /// <summary>
        /// Setting 'hideFlags'.
        /// </summary>
        public const string hideFlags = "None";
    }
    public static class Events
    {
        /// <summary>
        /// Firebase Event 'Rewarded_Ready_Fail'.
        /// </summary>
        public const string Rewarded_Ready_Fail = "Rewarded_Ready_Fail";
        /// <summary>
        /// Firebase Event 'Interstitial_Ready_Fail'.
        /// </summary>
        public const string Interstitial_Ready_Fail = "Interstitial_Ready_Fail";
        /// <summary>
        /// Firebase Event 'Banner_Show'.
        /// </summary>
        public const string Banner_Show = "Banner_Show";
        /// <summary>
        /// Firebase Event 'Banner_Show_Fail'.
        /// </summary>
        public const string Banner_Show_Fail = "Banner_Show_Fail";
        /// <summary>
        /// Firebase Event 'Banner_Fail'.
        /// </summary>
        public const string Banner_Fail = "Banner_Fail";
        /// <summary>
        /// Firebase Event 'Banner_Click'.
        /// </summary>
        public const string Banner_Click = "Banner_Click";
        /// <summary>
        /// Firebase Event 'Error'.
        /// </summary>
        public const string Error = "Error";
        /// <summary>
        /// Firebase Event 'Network'.
        /// </summary>
        public const string Network = "Network";
        /// <summary>
        /// Firebase Event 'Banner_Clicked_Network'.
        /// </summary>
        public const string Banner_Clicked_Network = "Banner_Clicked_Network";
        /// <summary>
        /// Firebase Event 'Interstitial_Sdk_Fail'.
        /// </summary>
        public const string Interstitial_Sdk_Fail = "Interstitial_Sdk_Fail";
        /// <summary>
        /// Firebase Event 'Interstitial_Fail'.
        /// </summary>
        public const string Interstitial_Fail = "Interstitial_Fail";
        /// <summary>
        /// Firebase Event 'Interstitial_Show'.
        /// </summary>
        public const string Interstitial_Show = "Interstitial_Show";
        /// <summary>
        /// Firebase Event 'Interstitial_Show_fail'.
        /// </summary>
        public const string Interstitial_Show_fail = "Interstitial_Show_fail";
        /// <summary>
        /// Firebase Event 'Interstitial_Click'.
        /// </summary>
        public const string Interstitial_Click = "Interstitial_Click";
        /// <summary>
        /// Firebase Event 'Interstitial_Click_Network'.
        /// </summary>
        public const string Interstitial_Click_Network = "Interstitial_Click_Network";
        /// <summary>
        /// Firebase Event 'Rewarded_Sdk_Fail'.
        /// </summary>
        public const string Rewarded_Sdk_Fail = "Rewarded_Sdk_Fail";
        /// <summary>
        /// Firebase Event 'Rewarded_Fail'.
        /// </summary>
        public const string Rewarded_Fail = "Rewarded_Fail";
        /// <summary>
        /// Firebase Event 'Rewarded_Show'.
        /// </summary>
        public const string Rewarded_Show = "Rewarded_Show";
        /// <summary>
        /// Firebase Event 'Rewarded_Show_Fail'.
        /// </summary>
        public const string Rewarded_Show_Fail = "Rewarded_Show_Fail";
        /// <summary>
        /// Firebase Event 'Rewarded_Click'.
        /// </summary>
        public const string Rewarded_Click = "Rewarded_Click";
        /// <summary>
        /// Firebase Event 'Rewarded_Click_Network'.
        /// </summary>
        public const string Rewarded_Click_Network = "Rewarded_Click_Network";
        /// <summary>
        /// Adjust Event 'Ads_Revenue_Banner'.
        /// </summary>
#if UNITY_IOS
        public const string Ads_Revenue_Banner = "123456";
#elif UNITY_ANDROID
        public const string Ads_Revenue_Banner = "123456";
#endif
        /// <summary>
        /// Adjust Event 'Ads_Revenue_Inter'.
        /// </summary>
#if UNITY_IOS
        public const string Ads_Revenue_Inter = "123456";
#elif UNITY_ANDROID
        public const string Ads_Revenue_Inter = "123456";
#endif
        /// <summary>
        /// Adjust Event 'Ads_Revenue_Rewarded'.
        /// </summary>
#if UNITY_IOS
        public const string Ads_Revenue_Rewarded = "123456";
#elif UNITY_ANDROID
        public const string Ads_Revenue_Rewarded = "123456";
#endif
        /// <summary>
        /// Facebook Event 'Level_Fail'.
        /// </summary>
#if UNITY_IOS
        public const string Level_Fail = "234234";
#elif UNITY_ANDROID
        public const string Level_Fail = "234234";
#endif
    }


    public static class ExtentionHelpers {
        /// <summary>
        /// Shortcut to change enum to string
        /// </summary>
        public static string GetName(this Scenes scene) {
              return SceneNames.GetSceneName(scene);
        }
    }
}

