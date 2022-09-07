using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PassionPunchSettings))]
public class PassionPunchSettingsEditor : Editor
{
    private PassionPunchSettings passionPunchSettings;


    public void OnEnable()
    {
        Debug.Log("Im enabled");
    }
    public override void OnInspectorGUI()
    {
        var sherlockDefine = "PP_SHERLOCK";
        var adjustDefine = "PP_ADJUST";
        var firebaseDefine = "PP_FIREBASE";
        var admostDefine = "PP_ADMOST";
        var maxDefine = "PP_APPLOVINMAX";
        var ironsourceDefine = "PP_IRONSOURCE";
        var mopubDefine = "PP_MOPUB";
        var debugDefine = "PP_DEBUG";
        var unityiapDefine = "PP_UNITYIAP";
        var revenuecatDefine = "PP_REVENUECAT";
        var facebookDefine = "PP_FACEBOOK";
        
        // Call base class method
        base.DrawDefaultInspector();

        // Custom form for Player Preferences
        passionPunchSettings = (PassionPunchSettings)target;


        GUILayout.BeginHorizontal();

        GUILayout.Space(10);
        if (GUILayout.Button("Save Define Symbols"))
        {
            SetDefineSymbols(passionPunchSettings.AdjustEnabled, adjustDefine);
            SetDefineSymbols(passionPunchSettings.FirebaseEnabled, firebaseDefine);
            SetDefineSymbols(passionPunchSettings.AdmostMediationEnabled, admostDefine);
            SetDefineSymbols(passionPunchSettings.IronsourceMediationEnabled, ironsourceDefine);
            SetDefineSymbols(passionPunchSettings.MopubMediationEnabled, mopubDefine);
            SetDefineSymbols(passionPunchSettings.ApplovinmaxMediationEnabled, maxDefine);
            SetDefineSymbols(passionPunchSettings.SherlockEnabled, sherlockDefine);
            SetDefineSymbols(passionPunchSettings.UnityIAPEnabled, unityiapDefine);
            SetDefineSymbols(passionPunchSettings.RevenuecatEnabled, revenuecatDefine);
            SetDefineSymbols(passionPunchSettings.FacebookEnabled, facebookDefine);            
            SetDefineSymbols(passionPunchSettings.VerboseDebugLogEnabled, debugDefine);

            Debug.Log("Define Symbols Generated");
        }

        GUILayout.EndHorizontal();
        // Custom Button with Image as Thumbnail

    }

    private void SetDefineSymbols(bool status, string symbol)
    {
        if (status)
        {
            AddDefineSymbols(new[] {symbol});
        }
        else
        {
            RemoveSymbol(symbol);
        }
    }
    
    
    public static bool CheckDefineSymbol(string symbol)
    {
        List<string> allDefines = GetAllDefines();
        return allDefines.Contains(symbol);
    }

    public static List<string> GetAllDefines()
    {
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();
        return allDefines;
    }
    public static void AddDefineSymbols(string[] symbols)
    {
        var allDefines = GetAllDefines();
        allDefines.AddRange(symbols.Except(allDefines));
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    internal static void RemoveSymbol(string symbol)
    {
        var allDefines = GetAllDefines();
        if (allDefines.Contains(symbol))
        {
            allDefines.Remove(symbol);

            //Set again.
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", allDefines.ToArray()));
        }
    }

}
