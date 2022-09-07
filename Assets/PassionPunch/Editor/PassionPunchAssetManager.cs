using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PassionPunchAssetManager 
{
    [MenuItem("PassionPunch/SDKSettings/Create")]
    public static void CreateAsset()
    {
        PassionPunchSettings settings = ScriptableObject.CreateInstance<PassionPunchSettings>();
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/PassionPunch/Settings/PassionPunchSettings.asset");
        AssetDatabase.CreateAsset(settings, assetPathAndName);
        MutualExecutions();
        Selection.activeObject = settings;
    }
    
    private static void MutualExecutions()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
    }
}
