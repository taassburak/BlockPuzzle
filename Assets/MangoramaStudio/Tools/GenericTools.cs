using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using Iso;


public static class GenericTools
{
    public static void FullStep(GameObject gameObject, Action start, Action playing, Func<bool> condition, Action complete, float interval)
    {
        StepActionMono stepActionMono = gameObject.AddComponent<StepActionMono>();
        stepActionMono.SetEvent(start, playing, condition, complete, interval);
    }

    public static void StartWaitComplete(GameObject gameObject, Action start, float waitDuration, Action complete)
    {
        float passedTime = 0;


        void Play_S0()
        {
            passedTime += Time.deltaTime;
        }

        bool Condition_S0()
        {
            return passedTime < waitDuration;
        }

        StepActionMono stepActionMono = gameObject.AddComponent<StepActionMono>();
        stepActionMono.SetEvent(start, Play_S0, Condition_S0, complete, 0);
    }

    public static void WaitComplete(GameObject gameObject, float waitDuration, Action complete)
    {
        float passedTime = 0;

        void Start_S0()
        {
        }

        void Play_S0()
        {
            passedTime += Time.deltaTime;
        }

        bool Condition_S0()
        {
            return passedTime < waitDuration;
        }

        StepActionMono stepActionMono = gameObject.AddComponent<StepActionMono>();
        stepActionMono.SetEvent(Start_S0, Play_S0, Condition_S0, complete, 0);
    }


    public static int RandomLength(this int _length)
    {
        return Random.Range(0, _length);
    }

    public static T RandomElement<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static T RandomElement<T>(this T[] list)
    {
        return list[Random.Range(0, list.Length)];
    }

    public static void Delay(Action onComplete, float duration)
    {
        var y = 0f;
        DOTween.To(() => y, k => y = k, 1, duration).OnStart(() => y = 0).OnComplete(() => onComplete());
    }

    public static bool IsExist<T>(T element, List<T> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            var item = list[i];
            if (item.Equals(element)) return true;
        }

        return false;
    }

    public static Material ColorfulMat => Resources.Load("Colorful", typeof(Material)) as Material;
    public static Material GrayScaleMat => Resources.Load("GrayScale", typeof(Material)) as Material;
}