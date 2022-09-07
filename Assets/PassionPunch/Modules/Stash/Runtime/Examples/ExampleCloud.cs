// #define FIREBASE
#if FIREBASE
using Firebase.Storage;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExampleCloud : MonoBehaviour
{
    private Stash stash;
    private string stashId = "myCloud";

    async void Start()
    {
        // Create a stash with specified id and storage location
        var task = Stash.Cloud(stashId, OnStashError, DownloadStash);
        await task;
        Debug.Log(task == null);
        stash = task.Result;

        // Stash is ready I can operate
        //@ WRITE to stash
        stash.Set("someInteger", 5);
        GameConfig writeConfig = new GameConfig();
        writeConfig.someVersion = "5.5.5.5.5";
        stash.Set("gameConfig", writeConfig);

        //@ READ from stash
        int integer = stash.Get("someInteger", 0);
        GameConfig readConfig = stash.Get("gameConfig", new GameConfig());

        // save stash to persistentPath
        //@NOTE: if you dont call this method stash wont be saved!
        stash.Save();
    }

    // called when stash encounter a critical error
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

    async void DownloadStash(System.Action<byte []> initStash)
    {
#if FIREBASE

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference root = storage.RootReference;
        StorageReference bytes = storage.GetReference("bytes.dat");
        const long maxAllowedSize = 1 * 1024 * 1024;
        var task = bytes.GetBytesAsync(maxAllowedSize);
        await task;
        // overrides the current values of stash (make sure you didnt add any value before this method is processed)
        initStash(task.Result);
#endif

    }
}
