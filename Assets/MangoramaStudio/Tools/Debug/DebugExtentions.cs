using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugExtentions
{
    public static void DrawGreenCube(Vector3 pos)
    {
        GizmosFromEveryWhereManager.gizmosDrawingEvents.AddListener(() =>
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(pos, new Vector3(.1f, .1f, .1f));
        });
    }

    public static void DrawRedCube(Vector3 pos)
    {
        if (!GizmosFromEveryWhereManager._instance)
        {
            GameObject go = new GameObject();
            go.AddComponent<GizmosFromEveryWhereManager>();
        }

        GizmosFromEveryWhereManager.gizmosDrawingEvents.AddListener(() =>
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(pos, new Vector3(.1f, .1f, .1f));
        });
    }
}