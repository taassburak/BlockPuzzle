using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(VegasSettings))]
public class VegasSettingsEditor : Editor
{
    VegasSettings vegasSettings;

    public string definesString;
    public List<string> allDefines;
    private void OnEnable()
    {
        InitEditorTogglesFromCurrentFlags();
    }

    private void InitEditorTogglesFromCurrentFlags()
    {
        definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        allDefines = definesString.Split(';').ToList();
        if (vegasSettings == null)
        {
            vegasSettings = (VegasSettings)target;
        }
        vegasSettings.isAdmostEnabled = allDefines.Contains("PP_ADMOST");
    }
    public override void OnInspectorGUI()
    {
        // Call base class method
        base.DrawDefaultInspector();

        // Custom form for Player Preferences
        vegasSettings = (VegasSettings)target;


        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save Define Symbols"))
        {
            SetFlags();
            Debug.Log("Define Symbols Generated");
        }

        GUILayout.EndHorizontal();
        // Custom Button with Image as Thumbnail

    }
    void SetFlags()
    {
        AddDefineSymbols();
    }
    void AddDefineSymbols()
    {
        definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        allDefines = definesString.Split(';').ToList();

        SetFlag("PP_ADMOST", vegasSettings.isAdmostEnabled);
       
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }
    private void SetFlag(string flagName, bool isAdd)
    {
        if (isAdd)
        {
            if (!allDefines.Contains(flagName))
            {
                allDefines.Add(flagName);
            }
        }
        else
        {
            if (allDefines.Contains(flagName))
            {
                allDefines.Remove(flagName);
            }
        }
    }
}
