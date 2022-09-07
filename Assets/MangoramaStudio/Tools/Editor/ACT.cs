using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public static class ACT
{
    [MenuItem("GameObject/ACT &q", false, 0)]
    static void Init()
    {
        foreach (var item in Selection.gameObjects)
        {
            item.SetActive(!item.activeSelf);
        }
    }

    [MenuItem("GameObject/Point &%n", false, 0)]
    static void CreatePoint()
    {
        // GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject go = new GameObject();
        go.name = "GO";
        go.transform.position = Vector3.zero;
        // go.transform.localScale = new Vector3(.1f, .1f, .1f);
        DrawIcon(go, 1);
    }


    private static void DrawIcon(GameObject gameObject, int idx)
    {
        var largeIcons = GetTextures("sv_label_", string.Empty, 0, 8);
        var icon = largeIcons[idx];
        var egu = typeof(EditorGUIUtility);
        var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
        var args = new object[] {gameObject, icon.image};
        var setIcon = egu.GetMethod("SetIconForObject", flags, null, new Type[] {typeof(UnityEngine.Object), typeof(Texture2D)}, null);
        setIcon.Invoke(null, args);
    }

    private static GUIContent[] GetTextures(string baseName, string postFix, int startIndex, int count)
    {
        GUIContent[] array = new GUIContent[count];
        for (int i = 0; i < count; i++)
        {
            array[i] = EditorGUIUtility.IconContent(baseName + (startIndex + i) + postFix);
        }

        return array;
    }
}