using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExamplePersistentPath : MonoBehaviour
{
    private Stash stash;
    private string stashId = "myPersistentPath";

    void Start()
    {
        // StashType.PersistentPath Example : 
        // Initialize a stash with specified id and storage location
        stash = Stash.PersistentPath(stashId , OnStashError);
        //stash.Save();

        //@ WRITE to stash
        stash.Set("someInteger", 5);
        GameConfig writeConfig = new GameConfig();
        writeConfig.someVersion = "5.5.5.5.5";
        stash.Set("gameConfig", writeConfig);

        //@ READ from stash
        int integer = stash.Get("someInteger", 0);
        GameConfig readConfig = stash.Get("gameConfig", new GameConfig());
        Debug.Log(integer);

        // save stash to persistentPath
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
