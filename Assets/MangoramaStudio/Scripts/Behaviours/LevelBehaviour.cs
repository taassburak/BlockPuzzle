using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBehaviour : MonoBehaviour
{
    public float WinPanelDelayTime => _winPanelDelayTime;
    public float LosePanelDelayTime => _losePanelDelayTime;

    [SerializeField] private float _winPanelDelayTime;
    [SerializeField] private float _losePanelDelayTime;

    private GameManager _gameManager;

    private bool _isLevelEnded;

    public void Initialize(GameManager gameManager, bool isRestart = false)
    {
        _gameManager = gameManager;

        if (!isRestart)
        {
            _gameManager.EventManager.StartLevel();
        }
        else
        {
            _gameManager.EventManager.RestartGame();
        }        
    }

    private void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            LevelCompleted();
        }

        if (Input.GetKeyDown("f"))
        {
            LevelFailed();
        }
    }

    private void OnDestroy()
    {
        
    }

    private void LevelCompleted()
    {
        if (_isLevelEnded) return;

        _gameManager.EventManager.LevelCompleted();
        InputController.IsInputDeactivated = true;
        _isLevelEnded = true;
    }

    private void LevelFailed()
    {
        if (_isLevelEnded) return;

        _gameManager.EventManager.LevelFailed();
        InputController.IsInputDeactivated = true;
        _isLevelEnded = true;
    }
}
