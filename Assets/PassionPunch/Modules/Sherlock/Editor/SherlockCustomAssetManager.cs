using UnityEngine;
using UnityEditor;
namespace PassionPunch.Sherlock
{
    public class SherlockCustomAssetManager
    {
        [MenuItem("PassionPunch/Sherlock/Create/SherlockEvents")]
        public static void CreateCustomAdjustEvent()
        {
            SherlockEvents sherlockEvents = ScriptableObject.CreateInstance<SherlockEvents>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/Sherlock/SherlockEvents" + ".asset");
            AssetDatabase.CreateAsset(sherlockEvents, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = sherlockEvents;
        }
        [MenuItem("PassionPunch/Sherlock/SherlockSettings")]
        public static void CreateSherlockSettings()
        {
            SherlockSettings sherlockSettings = ScriptableObject.CreateInstance<SherlockSettings>();
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/Settings/SherlockSettings" + ".asset");
            AssetDatabase.CreateAsset(sherlockSettings, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = sherlockSettings;
        }

    }

}