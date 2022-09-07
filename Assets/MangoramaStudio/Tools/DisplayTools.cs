using System;
using UnityEditor;
using UnityEngine;

public static class DisplayTools
{
    // Start is called before the first frame update
	public static void Draw(Vector3 position, string text)
	{
#if UNITY_EDITOR
		Handles.color = Color.white;
		Handles.Label(position, text);
#endif
	}


	public static void DrawSphere(Vector3 position, Color color)
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(position, .05f);
	}


	public static void DisplayPolygon(Vector3[] vertices, Color color)
	{
		Gizmos.color = color;

        //Draw the polygons vertices
		float vertexSize = 0.01f;

		for (int i = 0; i < vertices.Length; i++)
		{
			Gizmos.DrawSphere(vertices[i], vertexSize);
		}

        // var myv = new MyVector2(1, 1);
        //Draw the polygons outlines
		for (int i = 0; i < vertices.Length; i++)
		{
			int iPlusOne = Habrador_MathUtility.ClampListIndex(i + 1, vertices.Length);

			Gizmos.DrawLine(vertices[i], vertices[iPlusOne]);
		}
	}
}