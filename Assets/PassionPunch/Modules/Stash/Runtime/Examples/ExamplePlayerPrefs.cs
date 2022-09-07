using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePlayerPrefs : MonoBehaviour
{
    private Stash stash;
    private string stashId = "myPrefs";

    void Start()
    {
        // PlayerPreferences Example : 
        // Create a stash with specified id and storage location
        stash = Stash.PlayerPrefs(stashId , OnStashError);

        //@ WRITE to stash
        stash.Set("someInteger", 5);
        GameConfig writeConfig = new GameConfig();
        writeConfig.someVersion = "5.5.5.5.5";
        stash.Set("gameConfig", writeConfig);

        //@ READ from stash
        int integer = stash.Get("someInteger" , 0);
        GameConfig readConfig = stash.Get("gameConfig", new GameConfig());

        // save stash to playerprefs
        //@NOTE: if you dont call this method stash wont be saved!
        stash.Save();
    }

    private void OnStashError(StashError error)
    {
        Debug.LogError(error);
    }

    // on application killed
    private void OnApplicationQuit()
    {
        stash.Save();
    }

    // on application suspended (home button)
    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            stash.Save();
        }
    }
}

[System.Serializable]
public class GameConfig
{
    public int showNotification = 5;
    public string someVersion = "1.0.5.4";
    public int maxHealth = 15;
    public Dictionary<int, int> dictionary = new Dictionary<int, int>()
    {
        { 5, 6 },
        { 10, 11 }
    };
}