using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepActionMono : MonoBehaviour
{
    public string name;
    private bool isTriggered;

    private Action startAction;
    private Action playingAction;
    private Func<bool> condition;
    private Action completeAction;

    private void Start()
    {
        startAction?.Invoke();
    }

    public void SetEvent(Action _startAction, Action _playingAction, Func<bool> _condition, Action _completeAction, float _interval)
    {
        startAction = _startAction;
        condition = _condition;
        playingAction = _playingAction;
        completeAction = _completeAction;
        StartCoroutine(StartIe(_interval));
    }

    IEnumerator StartIe(float interval)
    {
        while (!isTriggered)
        {
            yield return new WaitForSeconds(interval);

            if (condition())
            {
                playingAction?.Invoke();
            }

            else
            {
                completeAction?.Invoke();
                isTriggered = true;
            }
        }
    }
}