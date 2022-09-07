using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace PassionPunch.Dealer
{
    [CustomEditor(typeof(DealerSettings))]
    public class DealerSettingsEditor : Editor
    {
        DealerSettings dealerSettings;

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
            if (dealerSettings == null)
            {
                dealerSettings = (DealerSettings)target;
            }
            dealerSettings.isAdjustEnabled = allDefines.Contains("PP_ADJUST");
            dealerSettings.isAdmostEnabled = allDefines.Contains("PP_ADMOST");
            dealerSettings.isRevenueCatEnabled = allDefines.Contains("PP_REVENUECAT");
            dealerSettings.isSherlockEnabled = allDefines.Contains("PP_SHERLOCK");
            dealerSettings.isUnityIAPEnabled = allDefines.Contains("PP_UNITYIAP");
        }
        public override void OnInspectorGUI()
        {
            // Call base class method
            base.DrawDefaultInspector();

            // Custom form for Player Preferences
            dealerSettings = (DealerSettings)target;


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

            SetFlag("PP_ADJUST", dealerSettings.isAdjustEnabled);
            SetFlag("PP_ADMOST", dealerSettings.isAdmostEnabled);
            SetFlag("PP_REVENUECAT", dealerSettings.isRevenueCatEnabled);
            SetFlag("PP_SHERLOCK", dealerSettings.isSherlockEnabled);
            SetFlag("PP_UNITYIAP", dealerSettings.isUnityIAPEnabled);

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
}