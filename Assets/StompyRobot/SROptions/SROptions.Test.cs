//#define ENABLE_TEST_SROPTIONS

using System;
using System.ComponentModel;
using System.Diagnostics;
using MangoramaStudio.Scripts.Data;
using SRDebugger;
using SRDebugger.Services;
using SRF;
using SRF.Service;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public partial class SROptions
{
    public static event Action OnLevelInvoked;

    private int _afterSeconds = 10;

    
    [Category("Level")]
    public int Level
    {
        get
        {
            return PlayerData.CurrentLevelId;
        }

        set
        {
            PlayerData.CurrentLevelId = value;
        }
    }

    // Updates the CurrentLevelID and restarts game.
    [Category("Level")]
    public void InvokeLevel()
    {
        OnLevelInvoked?.Invoke();
    }
}
