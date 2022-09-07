using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using PassionPunch.Stash;

/*
 Template Method Pattern
 Stash represents block of data (a file)
 can be written or read
 can be syncronize
     */

public abstract class Stash
{
    public StashLocation location = StashLocation.PersistentPath;
    private LinkedList<System.Action<StashError>> onError = new LinkedList<Action<StashError>>();
    public Hashtable hashtable = new Hashtable();
    public string id;
    private TaskCompletionSource<bool> isCloudInitialized = new TaskCompletionSource<bool>();

    public virtual void Set<T>(string key, T value)
    {
        if(!hashtable.ContainsKey(key))
        {
            hashtable.Add(key, null);
        }

        hashtable [key] = value;
    }

    public virtual T Get<T>(string key, T defaultValue)
    {
        if(!hashtable.ContainsKey(key))
        {
            return defaultValue;
        }

        return (T)hashtable [key];
    }

    public virtual T Get<T>(string key)
    {
        return Get<T>(key, default);
    }

    public abstract void Save(); // sync stash with disk
    public abstract void SaveAsync(System.Action<Stash> onCompleted); // sync stash with disk

    private void AddErrorListener(System.Action<StashError> action)
    {
        onError.AddLast(action);
    }

    public Stash(string id, StashLocation location)
    {
        this.id = id;
        this.location = location;
    }

    public static async Task<Stash> Cloud(string id, System.Action<StashError> onError, System.Action<System.Action<byte []>> OnDownload)
    {
        var stash = new StashPersistentPath(id, StashLocation.Cloud);
        stash.AddErrorListener(onError);
        string path = StashUtils.GetPersistentPath(id);
        byte [] bytes = StashUtils.ReadFromDisk(path);

        if(bytes.Length < 1)
        {
            stash.hashtable = new Hashtable();
            OnDownload.SafeInvoke(stash.InitializeCloud);
            Debug.Log("Stash : No cached file found. Invoking download stash function.");
        }
        else
        {
            stash.hashtable = stash.FromDecryptedBytes<Hashtable>(bytes);
            stash.isCloudInitialized.SetResult(true);
            Debug.Log("Stash : Cached file found. Hastable is set.");
        }

        await stash.isCloudInitialized.Task;
        return stash;
    }

    private void InitializeCloud(byte [] bytes)
    {
        hashtable = StashUtils.Deserialize<Hashtable>(bytes);
        Debug.Log("Hashtable downloaded, initialized. Please call stash.Save() to make sure cache the stash.");
        isCloudInitialized.SetResult(true);
    }

    public static Stash PlayerPrefs(string id , System.Action<StashError> onError)
    {
        var stash = new StashPlayerPrefs(id, StashLocation.PlayerPreferences);
        stash.AddErrorListener(onError);
        string strBytes = UnityEngine.PlayerPrefs.GetString(id, string.Empty);

        if(string.IsNullOrEmpty(strBytes))
        {
            Debug.Log("Stash : No save file found. Creating new one.");
            stash.hashtable = new Hashtable();
        }
        else
        {
            byte [] bytes = Convert.FromBase64String(strBytes);
            stash.hashtable = stash.FromDecryptedBytes<Hashtable>(bytes);
        }

        return stash;
    }

    public static Stash PersistentPath(string id , System.Action<StashError> onError )
    {
        var stash = new StashPersistentPath(id, StashLocation.PersistentPath);
        string path = StashUtils.GetPersistentPath(id);
        Debug.Log(path);
        byte [] bytes = StashUtils.ReadFromDisk(path);
        stash.AddErrorListener(onError);

        if(bytes.Length < 1)
        {
            Debug.Log("Stash : No save file found. Creating new one.");
            stash.hashtable = new Hashtable();
        }
        else
        {
            stash.hashtable = stash.FromDecryptedBytes<Hashtable>(bytes);
        }

        return stash;
    }

    // take crypted bytes decrypt them
    // and deserialize by binary formatter
    // return given T object
    public T FromDecryptedBytes<T>(byte [] bytes) where T : new()
    {
        try
        {
            return StashUtils.Deserialize<T>(Encryptor.Decrypt(bytes));
        }
        catch
        {
            RaiseError(StashError.CorruptedFile);
            return new T(); // return empty hastable
        }
    }

    // take the object serialize by binary formatter
    // encrypt the bytes and return it
    public byte [] ToEncryptedBytes<T>(T value)
    {
        try
        {
            return Encryptor.Encrypt(StashUtils.Serialize(value));
        }
        catch
        {
            RaiseError(StashError.CorruptedFile);
            return new byte [0];
        }
    }

    private void RaiseError(StashError stashError)
    {
        foreach(var element in onError)
        {
            element.SafeInvoke(stashError);
        }
    }
}

public enum StashLocation
{
    PlayerPreferences = 0,
    PersistentPath = 1,
    Cloud = 2
}

public enum StashError
{
    Unknown = 0,
    CorruptedFile = 1
}