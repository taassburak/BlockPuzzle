using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Utilities
{
    public static void Open(this CanvasGroup canvas)
    {
        canvas.alpha = 1;
        canvas.blocksRaycasts = true;
        canvas.interactable = true;
    }

    public static void Close(this CanvasGroup canvas)
    {
        canvas.alpha = 0;
        canvas.blocksRaycasts = false;
        canvas.interactable = false;
    }
    
    public static void SetTransparecy(this UnityEngine.UI.Image image, float transparency)
    {
        if(image != null)
        {
            Color tempColor = image.color;
            tempColor.a = transparency;
            image.color = tempColor;
        }
    }

    public static bool SameWith<T>(this List<T> list, List<T> targetList)
    {
        if ((list == null && targetList != null) || (targetList == null && targetList != null))
            return false;

        if (list == null && targetList == null) return true;

        if (list.Count != targetList.Count) return false;

        HashSet<T> temp = new HashSet<T>(targetList);
        return list.TrueForAll(x => temp.Contains(x));

    }

    public static Vector2 RandomPointOnCircle(this Vector2 position, float radius)
    {
        float angle = UnityEngine.Random.Range (0f, Mathf.PI * 2);
        float x = Mathf.Sin (angle) * radius;
        float y = Mathf.Cos (angle) * radius;

        return new Vector2 (x, y);
    }

    public static List<Vector2> RandomPointOnRange(this Vector2 position, float radius, int waveCount){
        float distance =360/waveCount;

        List<Vector2> tempAngleAnchor = new List<Vector2>();

        for (int i = 0; i < waveCount; i++) 
        {
            float angle = UnityEngine.Random.Range ((distance * i)+10, (distance * (i + 1))-10);
            float x = Mathf.Sin (angle*Mathf.Deg2Rad) * radius;
            float y = Mathf.Cos (angle*Mathf.Deg2Rad) * radius;
            tempAngleAnchor.Add (new Vector2(x,y));
        }

        return tempAngleAnchor;
    }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            System.Random r = new System.Random();
            var list = enumerable as IList<T> ?? enumerable.ToList();
            return list.ElementAt(r.Next(0, list.Count()));
        }
}
