using System.Collections;
using System.Collections.Generic;
using MEC;
using PassionPunch;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppInitializer : MonoBehaviour
{
    private enum InitializeState
    {
        GameConfigs,
        UserData,
        GameStoreData
    }

    [SerializeField] private GameObject passionPunchSDKPrefab;
    
    private const int NUM_INITIALIZE_STEP = 3;
    private bool[] gamestepInitialized;


    private void Start()
    {
        this.Print("Initializing game...");
        InitializeGame();
    }

    /// <summary>
    /// Start the initialization of the game
    /// </summary>
    private void InitializeGame()
    {
        gamestepInitialized = new bool[NUM_INITIALIZE_STEP];

        AppManager.Instance.Initialize();

        Timing.RunCoroutine(C_InitializeGameSteps());

        Timing.RunCoroutine(C_EnterGameWhenReady());
    }

    private IEnumerator<float> C_InitializeGameSteps()
    {
        int stepGameConfigs = (int)InitializeState.GameConfigs;
        if (gamestepInitialized[stepGameConfigs] != true)
        {
            Timing.RunCoroutine(C_InitializeGameConfig());
        }
        while (gamestepInitialized[stepGameConfigs] != true)
        {
            yield return Timing.WaitForSeconds(0.01f);
        }


        int stepUserData = (int)InitializeState.UserData;
        if (gamestepInitialized[stepUserData] != true)
        {
            Timing.RunCoroutine(C_InitializeUserData());
        }
        while (gamestepInitialized[stepUserData] != true)
        {
            yield return Timing.WaitForSeconds(0.01f);
        }

        //initialize steps that can be run parallel
        int stepStoreData = (int)InitializeState.GameStoreData;
        if (gamestepInitialized[stepStoreData] != true)
        {
            Timing.RunCoroutine(C_InitializeStoreData());
        }

        yield return 0;
    }


    private IEnumerator<float> C_InitializeUserData()
    {
        //TODO Initialize User Data
        OnUserDataInitialized(true);

        yield return Timing.WaitForOneFrame;

    }

    private void OnUserDataInitialized(bool success)
    {
        if (success)
        {
            //TODO Do actions OnUserDataInitialized
            gamestepInitialized[(int)InitializeState.UserData] = true;
            this.Print("3. User Data initialized!");
        }
        else
        {
            this.Print("Error loading user data");
        }
    }

    /// <summary>
    /// Prepare latest available game config for this game
    /// </summary>
    private IEnumerator<float> C_InitializeGameConfig()
    {
        #if PP_FIREBASE
        this.Print("Initializing game config...");
        CoroutineHandle firebaseHandle = Timing.RunCoroutine(AppManager.Instance.C_InitializeFirebaseWithRemoteConfig());
        yield return Timing.WaitUntilDone(firebaseHandle);
        #endif
        this.Print("Initializing Passion Punch SDK...");
        GameObject ppsdk = Instantiate(passionPunchSDKPrefab);
        DontDestroyOnLoad(ppsdk);
        
        this.Print("Initializing ads...");
        AdManager.Instance.Initialize();

        gamestepInitialized[(int)InitializeState.GameConfigs] = true;
        this.Print("1. Game configs initialized!");
        yield return Timing.WaitForOneFrame;
    }


    /// <summary>
    /// Prepare latest available store data for this game
    /// </summary>
    private IEnumerator<float> C_InitializeStoreData()
    {
        //TODO Initialize Store Data
        gamestepInitialized[(int)InitializeState.GameStoreData] = true;
        this.Print("2. Store Data initialized!");
        yield return Timing.WaitForOneFrame;

    }

    /// <summary>
    /// The sequence to wait and start game as necessary
    /// </summary>
    private IEnumerator<float> C_EnterGameWhenReady()
    {

        int i, count;
        bool shouldWait = true;
        while (shouldWait)
        {
            count = 0;
            //check and count all initialized step of the game 
            for (i = 0; i < NUM_INITIALIZE_STEP; i++)
            {
                if (gamestepInitialized[i] == true)
                {
                    ++count;
                }
            }

            //if number of initialized step is equal to total step, meaning the game has been initialized, no need to wait any more
            if (count == NUM_INITIALIZE_STEP)
            {
                //TODO Remove this wait - demo purpose only
                yield return Timing.WaitForSeconds(2f);
                
                shouldWait = false;
                this.Print("Entering Game...");

                SceneManager.LoadSceneAsync(ProjectConstants.SceneNames.GetSceneName(ProjectConstants.Scenes.Game));
            }

            yield return Timing.WaitForSeconds(0.01f);
        }
    }
}


public enum AppState
{
    OFFLINE,
    ONLINE
}

public enum AppSubsState
{
    EXPIRED,
    SUBSCRIBED
}

public enum AppConsumableState
{
    NONE,
    REMOVEADS
}
