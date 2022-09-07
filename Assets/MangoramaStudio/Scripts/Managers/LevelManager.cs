using MangoramaStudio.Scripts.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : CustomBehaviour
{
    [SerializeField] private LevelBehaviour _forcedPlayLevel;

    [SerializeField] private int _startLevelCountAfterLoop;

    private LevelBehaviour _currentLevel;

    private int _totalLevelCount;

    #region Initialize

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        GameManager.EventManager.OnStartGame += StartGame;

        SROptions.OnLevelInvoked += RetryCurrentLevel;

        _totalLevelCount = Resources.LoadAll<LevelBehaviour>("Levels").Length;
    }

    private void OnDestroy()
    {
        GameManager.EventManager.OnStartGame -= StartGame;

        SROptions.OnLevelInvoked -= RetryCurrentLevel;
    }

    #endregion

    private void StartGame()
    {
        ClearLevel();

        Resources.UnloadUnusedAssets();

        InputController.IsInputDeactivated = false;

        var levelCount = PlayerData.CurrentLevelId - 1;

        var lapValue = levelCount / (_totalLevelCount);

        if (levelCount >= _totalLevelCount)
        {
            levelCount -= (_totalLevelCount * lapValue);
        }

        levelCount = lapValue >= 1 ? levelCount + _startLevelCountAfterLoop : levelCount;

        //var levelBehaviourPrefab = _forcedPlayLevel == null ? Resources.Load<LevelBehaviour>("Levels/Level" + levelCount) : _forcedPlayLevel;

        //var levelBehaviour = Instantiate(levelBehaviourPrefab);

        //levelBehaviour.Initialize(GameManager, _currentLevel != null && _currentLevel.name == levelBehaviour.name);

        //_currentLevel = levelBehaviour;
    }

    private void ClearLevel()
    {
        if (_currentLevel != null)
        {
            Destroy(_currentLevel.gameObject);
        }
    }

    public void ContinueToNextLevel() // For button
    {
        PlayerData.CurrentLevelId += 1;
        StartGame();
    }

    public void RetryCurrentLevel() // For button
    {
        StartGame();
    }
}

