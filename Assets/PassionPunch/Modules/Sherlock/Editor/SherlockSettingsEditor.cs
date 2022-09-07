using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace PassionPunch.Sherlock
{
    [CustomEditor(typeof(SherlockSettings))]
    public class SherlockSettingsEditor : Editor
    {
        SherlockSettings sherlockSettings;

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
            if (sherlockSettings == null)
            {
                sherlockSettings = (SherlockSettings)target;
            }
            sherlockSettings.isFirebaseEnabled = allDefines.Contains("PP_FIREBASE");
            sherlockSettings.isAdjustEnabled = allDefines.Contains("PP_ADJUST");
            sherlockSettings.isAdmostEnabled = allDefines.Contains("PP_ADMOST");
            sherlockSettings.isFBAnalyticsEnabled = allDefines.Contains("PP_FACEBOOK");
        }
        public override void OnInspectorGUI()
        {
            // Call base class method
            base.DrawDefaultInspector();

            // Custom form for Player Preferences
            sherlockSettings = (SherlockSettings)target;


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
            SetFlag("PP_FIREBASE", sherlockSettings.isFirebaseEnabled);
            SetFlag("PP_ADJUST", sherlockSettings.isAdjustEnabled);
            SetFlag("PP_ADMOST", sherlockSettings.isAdmostEnabled);
            SetFlag("PP_FACEBOOK", sherlockSettings.isFBAnalyticsEnabled);
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