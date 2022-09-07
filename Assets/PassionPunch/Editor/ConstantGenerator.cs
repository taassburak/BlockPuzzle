using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using PassionPunch.Sherlock;

namespace PassionPunch
{
    public static class UnityConstantsGenerator
    {
        private static string FindPath(string filename, string extension)
        {
            string path = string.Empty;
            foreach (var file in Directory.GetFiles(Application.dataPath, "*."+extension, SearchOption.AllDirectories)) {
                if (Path.GetFileNameWithoutExtension(file) == filename) {
                    path = file;
                    break;
                }
            }

            return path;
        }
      
        private static Dictionary<string, CustomProperty> DictionaryFromType(object atype)
        {
            if (atype == null) return new Dictionary<string, CustomProperty>();
            Type t = atype.GetType();
            PropertyInfo[] props = t.GetProperties();
            Dictionary<string, CustomProperty> dict = new Dictionary<string, CustomProperty>();
            foreach (PropertyInfo prp in props)
            {
                object value = prp.GetValue(atype, new object[]{});
                object type = prp.PropertyType;
                
                dict.Add(prp.Name, new CustomProperty()
                {
                    Name = prp.Name,
                    Value = value,
                    Type = type
                });
            }
            return dict;
        }
        
        
        [MenuItem("PassionPunch/Generate ProjectConstants.cs")]
        public static void Generate()
        {
            // Refresh
            AssetDatabase.Refresh();
            
            // Try to find an existing file in the project called "ProjectConstants.cs"
            string filePath = FindPath("ProjectConstants", "cs");

            // If no such file exists already, use the save panel to get a folder in which the file will be placed.
            if (string.IsNullOrEmpty(filePath)) {
                string directory = EditorUtility.OpenFolderPanel("Choose location for file ProjectConstants.cs", Application.dataPath, "");

                // Canceled choose? Do nothing.
                if (string.IsNullOrEmpty(directory)) {
                    return;
                }

                filePath = Path.Combine(directory, "ProjectConstants.cs");
            }

            // Write out our file
            using (var writer = new StreamWriter(filePath)) {
                writer.WriteLine("// This file is auto-generated. Modifications are not saved.");
                writer.WriteLine();
                writer.WriteLine("namespace ProjectConstants");
                writer.WriteLine("{");

                // Write out the Enum
                writer.WriteLine("    public enum Scenes");
                writer.WriteLine("    {");
                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                    string scene = Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
                        writer.WriteLine("        {0},", scene);
                }
                writer.WriteLine("        END");
                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out the tags
                writer.WriteLine("    public static class Tags");
                writer.WriteLine("    {");
                foreach (var tag in UnityEditorInternal.InternalEditorUtility.tags) {
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// Name of tag '{0}'.", tag);
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(tag), tag);
                }
                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out sorting layers
                writer.WriteLine("    public static class SortingLayers");
                writer.WriteLine("    {");
                foreach (var layer in SortingLayer.layers) {
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// ID of sorting layer '{0}'.", layer.name);
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("        public const int {0} = {1};", MakeSafeForCode(layer.name), layer.id);
                }
                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out layers
                writer.WriteLine("    public static class Layers");
                writer.WriteLine("    {");
                for (int i = 0; i < 32; i++) {
                    string layer = UnityEditorInternal.InternalEditorUtility.GetLayerName(i);
                    if (!string.IsNullOrEmpty(layer)) {
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// Index of layer '{0}'.", layer);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const int {0} = {1};", MakeSafeForCode(layer), i);
                    }
                }
                writer.WriteLine();
                for (int i = 0; i < 32; i++) {
                    string layer = UnityEditorInternal.InternalEditorUtility.GetLayerName(i);
                    if (!string.IsNullOrEmpty(layer)) {
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// Bitmask of layer '{0}'.", layer);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const int {0}Mask = 1 << {1};", MakeSafeForCode(layer), i);
                    }
                }
                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out scenes' IDs
                writer.WriteLine("    public static class SceneIDs");
                writer.WriteLine("    {");
                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                    string scene = Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// ID of scene '{0}'.", scene);
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("        public const int {0} = {1};", MakeSafeForCode(scene), i);
                }
                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out scenes' names
                writer.WriteLine("    public static class SceneNames");
                writer.WriteLine("    {");
                writer.WriteLine("          public const string INVALID_SCENE = \"InvalidScene\";");
                // Scenes' names as fields
                //for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                //    string scene = Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
                //    writer.WriteLine("        /// <summary>");
                //    writer.WriteLine("        /// Name of scene '{0}'.", scene);
                //    writer.WriteLine("        /// </summary>");
                //    writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(scene), scene);
                //}
                // Scenes' names in an array
                writer.WriteLine("    public static readonly string[] ScenesNameArray = {");
                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++) {
                    string scene = Path.GetFileNameWithoutExtension(EditorBuildSettings.scenes[i].path);
                    if (i == EditorBuildSettings.scenes.Length - 1) {
                        writer.WriteLine("\t        \"{0}\"", scene);
                    } else {
                        writer.WriteLine("\t        \"{0}\",", scene);
                    }
                }
                writer.WriteLine("        };");

                //write method to get scene name from enum
                writer.WriteLine("        /// <summary>");
                writer.WriteLine("        /// Convert from enum to string");
                writer.WriteLine("        /// </summary>");
                writer.WriteLine("        public static string GetSceneName(Scenes scene) {");
                writer.WriteLine("              int index = (int)scene;");
                writer.WriteLine("              if(index > 0 && index < ScenesNameArray.Length) {");
                writer.WriteLine("                  return ScenesNameArray[index];");
                writer.WriteLine("              } else {");
                writer.WriteLine("                  return INVALID_SCENE;");
                writer.WriteLine("              }");
                writer.WriteLine("        }");

                writer.WriteLine("    }");
                writer.WriteLine();

                // Write out Input axes
                writer.WriteLine("    public static class Axes");
                writer.WriteLine("    {");
                var axes = new HashSet<string>();
                var inputManagerProp = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
                foreach (SerializedProperty axe in inputManagerProp.FindProperty("m_Axes")) {
                    var name = axe.FindPropertyRelative("m_Name").stringValue;
                    var variableName = MakeSafeForCode(name);
                    if (!axes.Contains(variableName)) {
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// Input axis '{0}'.", name);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const string {0} = \"{1}\";", variableName, name);
                        axes.Add(variableName);
                    }
                }
                writer.WriteLine("    }");
                
                // Write out PassionPunchSettings
                var ppSettingsPath = AssetsRelativePath(FindPath("PassionPunchSettings", "asset"));
                PassionPunchSettings passionPunchSettings = AssetDatabase.LoadAssetAtPath<PassionPunchSettings>(ppSettingsPath);
                var ppResult = DictionaryFromType(passionPunchSettings);
                writer.WriteLine("    public static class AppConstants");
                writer.WriteLine("    {");
                foreach (var res in ppResult)
                {
                    if (res.Value.Value.IsGenericList())
                    {
                        var list = res.Value.Value as List<string>;
                        foreach (string item in list)
                        {
                            writer.WriteLine("        /// <summary>");
                            writer.WriteLine("        /// Firebase Remote Config Keys '{0}'.", item);
                            writer.WriteLine("        /// </summary>");
                            writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(item), item);
                        }
                    }
                    else
                    {
                        writer.WriteLine("        /// <summary>");
                        writer.WriteLine("        /// Setting '{0}'.", res.Key);
                        writer.WriteLine("        /// </summary>");
                        writer.WriteLine("        public const string {0} = \"{1}\";", res.Value.Name, res.Value.Value);
                    }
                }
                writer.WriteLine("    }");
                
                // Write out PassionPunchSettings
                var eventsPath = AssetsRelativePath(FindPath("SherlockEvents", "asset"));
                SherlockEvents sherlockEvents = AssetDatabase.LoadAssetAtPath<SherlockEvents>(eventsPath);
                writer.WriteLine("    public static class Events");
                writer.WriteLine("    {");
                foreach (var fevent in sherlockEvents.FirebaseEvents)
                {
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// Firebase Event '{0}'.", fevent);
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(fevent), fevent);
                }
                foreach (var adjust in sherlockEvents.AdjustEvents)
                {
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// Adjust Event '{0}'.", adjust.Name);
                    writer.WriteLine("        /// </summary>"); 
                    writer.WriteLine("#if UNITY_IOS");
                    writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(adjust.Name), adjust.eventCodeIOS);
                    writer.WriteLine("#elif UNITY_ANDROID");
                    writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(adjust.Name), adjust.eventCodeAndroid);
                    writer.WriteLine("#endif");
                }
                
                foreach (var fb in sherlockEvents.FacebookEvents)
                {
                    writer.WriteLine("        /// <summary>");
                    writer.WriteLine("        /// Facebook Event '{0}'.", fb.Name);
                    writer.WriteLine("        /// </summary>");
                    writer.WriteLine("#if UNITY_IOS");
                    writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(fb.Name), fb.eventCodeIOS);
                    writer.WriteLine("#elif UNITY_ANDROID");
                    writer.WriteLine("        public const string {0} = \"{1}\";", MakeSafeForCode(fb.Name), fb.eventCodeAndroid);
                    writer.WriteLine("#endif");
                }
                
                writer.WriteLine("    }");
                

                writer.WriteLine();
                writer.WriteLine();
                // Write static function to get scene name string from enum
                writer.WriteLine("    public static class ExtentionHelpers {");
                writer.WriteLine("        /// <summary>");
                writer.WriteLine("        /// Shortcut to change enum to string");
                writer.WriteLine("        /// </summary>");
                writer.WriteLine("        public static string GetName(this Scenes scene) {");
                writer.WriteLine("              return SceneNames.GetSceneName(scene);");
                writer.WriteLine("        }");
                writer.WriteLine("    }");

                // End of namespace UnityConstants
                writer.WriteLine("}");
                writer.WriteLine();
            }

            // Refresh
            AssetDatabase.Refresh();

            Debug.Log("Project Constants successfully generated");
        }

        private static string MakeSafeForCode(string str)
        {
            str = Regex.Replace(str, "[^a-zA-Z0-9_]", "_", RegexOptions.Compiled);
            if (char.IsDigit(str[0])) {
                str = "_" + str;
            }
            return str;
        }
        
        private static string AssetsRelativePath(string absolutePath)
        {
            if (absolutePath.StartsWith(Application.dataPath))
            {
                return "Assets" + absolutePath.Substring(Application.dataPath.Length);
            }
            else
            {
                if (!string.IsNullOrEmpty(absolutePath))
                {
                    throw new System.ArgumentException("Full path does not contain the current project's Assets folder", "absolutePath");
                }
                return null;
                
            }
        }
        
        private struct CustomProperty
        {
            public string Name;
            public object Type;
            public object Value;
        }

    }
}
