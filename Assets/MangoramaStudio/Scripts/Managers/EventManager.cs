using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : CustomBehaviour
{
    public event Action<int> OnChangeDiamondAmount;
    public event Action<int, bool, bool> OnAddDiamond;

    public event Action OnStartGame;
    public event Action<bool> OnLevelFinished;
    public event Action OnLevelStarted;
    public event Action OnLevelRestarted;
    public event Action<int> OnEarnPoint;

    #region Initialize

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    private void OnDestroy()
    {

    }

    #endregion

    /****************************************************************************************/

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void StartLevel()
    {
        OnLevelStarted?.Invoke();
    }

    public void LevelFailed()
    {
        OnLevelFinished?.Invoke(false);
    }

    public void RestartGame()
    {
        OnLevelRestarted?.Invoke();
    }

    public void LevelCompleted()
    {
        OnLevelFinished?.Invoke(true);
    }

    public void ChangeDiamondAmount(int amount)
    {
        OnChangeDiamondAmount?.Invoke(amount);
    }

    public void AddDiamond(int value, bool withTextAnim, bool withIconAnim)
    {
        OnAddDiamond?.Invoke(value, withTextAnim, withIconAnim);
    }

    public void EarnPoint(int value)
    {
        OnEarnPoint?.Invoke(value);
    }
}