using System;
using System.Collections;
using System.Collections.Generic;
using Iso;
using UnityEngine;

public static class UiTools
{
    public static Material GrayScaleMat => Resources.Load("GrayScale", typeof(Material)) as Material;
    public static Material SpriteDefault => Resources.Load("SpriteDefault", typeof(Material)) as Material;

    public static void Activate(this CustomButton customButton)
    {
        customButton.gameObject.SetActive(true);
    }

    public static void SetState(this CustomButton customButton, bool isEnabled)
    {
        customButton.SetEnabled(isEnabled);
        customButton.Image.material = isEnabled ? null : GrayScaleMat;
    }

    public static void SetStatusOnFiredEvent(this CustomButton customButton, Action<Action<int>> _eventManager, Action<int> action)
    {
        _eventManager(action);
    }
}