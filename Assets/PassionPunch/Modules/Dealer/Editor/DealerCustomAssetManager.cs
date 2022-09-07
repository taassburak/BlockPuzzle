using UnityEngine;
using UnityEditor;
namespace PassionPunch.Dealer
{
    public class DealerCustomAssetManager
    {
        [MenuItem("PassionPunch/Dealer/Create/SingleInappItem")]
        public static void CreateInappItemAsset()
        {
            SingleInAppItem inappItem = new SingleInAppItem();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/Dealer/InappItem.asset");
            AssetDatabase.CreateAsset(inappItem, assetPathAndName);
            MutualExecutions();
            Selection.activeObject = inappItem;
        }
        [MenuItem("PassionPunch/Dealer/DealerSettings")]
        public static void CreateDealerSettings()
        {
            DealerSettings dealerSettings = new DealerSettings();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/Settings/DealerSettings" + ".asset");
            AssetDatabase.CreateAsset(dealerSettings, assetPathAndName);
            MutualExecutions();
            Selection.activeObject = dealerSettings;
        }
        private static void MutualExecutions()
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }
}