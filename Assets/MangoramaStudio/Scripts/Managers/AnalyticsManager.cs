using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PassionPunch.Sherlock;
using MangoramaStudio.Scripts.Data;

public class AnalyticsManager : CustomBehaviour
{
    #region Initialize

    private Dictionary<string, string> _eventTokens;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        InitTokens();

        GameManager.EventManager.OnStartGame += GameStarted;
        GameManager.EventManager.OnLevelStarted += LevelStarted;
        GameManager.EventManager.OnLevelFinished += LevelFinished;
        GameManager.EventManager.OnLevelRestarted += LevelRestarted;
    }

    private void OnDestroy()
    {
        GameManager.EventManager.OnStartGame -= GameStarted;
        GameManager.EventManager.OnLevelStarted -= LevelStarted;
        GameManager.EventManager.OnLevelFinished -= LevelFinished;
        GameManager.EventManager.OnLevelRestarted -= LevelRestarted;
    }

    private void InitTokens()
    {
        _eventTokens = new Dictionary<string, string>();

        // add adjust tokens below here by hand

        _eventTokens.Add("level_1_Complete", "dummyToken");
        _eventTokens.Add("level_1_Fail", "aaaaaa");
        _eventTokens.Add("level_1_Restart", "bbbbbb");
        _eventTokens.Add("level_1_Start", "cccccc");
    }

    #endregion

    public void TrackEvent(string eventName)
    {
#if PP_FIREBASE && PP_SHERLOCK
        //Sherlock.Instance.Firebase_TrackEvent(eventName);

        Sherlock.Instance.Firebase_TrackEvent(eventName, new Parameter[]
        {
            new Parameter("Level",PlayerData.CurrentLevelId.ToString())
        });

#endif
#if PP_FACEBOOK && PP_SHERLOCK
        //Sherlock.Instance.FB_TrackEvent(eventName);
#endif
#if PP_ADJUST && PP_SHERLOCK
        string token;

        if (_eventTokens.TryGetValue(eventName, out token))
        {
            Sherlock.Instance.Adjust_TrackEvent(token);
        }

        else
        {
            Debug.Log("Token not found");
        }

        
        // if event code is defined in sherlockevents.asset
        //Sherlock.Instance.Adjust_TrackEvent(Sherlock.Instance.settings.customEvents.GetEventCode(eventName));
#endif
    }

    public void TrackIAPEvent(string productIdentifierKey, float price, string transaction, string isoCurrencyCode)
    {
#if PP_SHERLOCK && UNITY_IAP
        string token;

        if (_eventTokens.TryGetValue(productIdentifierKey, out token))
        {
            Sherlock.Instance.AdjustIAPEvents(token, price, transaction, isoCurrencyCode);
        }
#endif
    }

    private void GameStarted()
    {
        TrackEvent("Game_Started");
    }

    private void LevelStarted()
    {
        TrackEvent("level_" + PlayerData.CurrentLevelId.ToString() + "_Start");
    }

    public void LevelRestarted()
    {
        TrackEvent("level_" + PlayerData.CurrentLevelId.ToString() + "_Restart");

    }

    private void LevelFinished(bool isSuccess)
    {
        if (isSuccess)
        {
            TrackEvent("level_" + PlayerData.CurrentLevelId.ToString() + "_Completed");

        }
        else
        {
            TrackEvent("level_" + PlayerData.CurrentLevelId.ToString() + "_Failed");
        }
    }

}
