# Stash Quick Start

@NOTE: for custom classes make sure you added System.Serializable attribute 
```csharp 
[System.Serializable]public class Foo 
{
   public int bar;
}
```
## Local Stash - PlayerPrefs

#### Initialize
initialize local stash object with given **unique** identifier.
<br></br>
register a method for error handling.
```csharp
 stash = Stash.PlayerPrefs(stashId, OnStashError);

 void OnStashError(StashError error)
 {
     Debug.LogError(error);
 }
```
#### Set 
You can save any object marked as [Serializable]
<br></br>
 `Set<T>(string key, T value)` creates or updates given key-value pair 
```csharp 
 stash.Set("someInteger", 5);
 GameConfig writeConfig = new GameConfig();
 stash.Set("gameConfig", writeConfig);
```
#### Get 
For fail safe `Get<T>(string key, T defaultValue)` method requires defaultValue<br>
If given key-value pair is not exist then defaultValue will be returned
```csharp 
 int integer = stash.Get("someInteger" , 0);
 GameConfig readConfig = stash.Get("gameConfig", new GameConfig());
```
#### Save Stash 
Stash is not saved to disk until you call `stash.Save()` method
<br></br>
stash.Save() blocks UIThread until operation is completed.
<br></br>
NOTE: Dont save huge files(10 mb or more) when processing exit message (OS) may suspend your save operation
<br></br>
you may end up with broken save file.
```csharp 
 stash.Save(); // can be called any time during the game.
  // on application killed - make sure stash is saved.
 private void OnApplicationQuit()
 {
    stash.Save();
 }
 // on application suspended (home button) - make sure stash is saved.
 private void OnApplicationPause(bool pause)
 {
    if(pause)
    {
        stash.Save();
    }
 }
```
#### Save Stash - Async Non Blocking
PlayerPrefs is not supports `SaveAsync()`
<br></br>
if you call this method by mistake it will call `Save()` method.

## Cloud Stash - Persistent
Cloud stash build on top of `StashPersistentPath` with additional initialization step
<br></br>
`System.Action<System.Action<byte []>> OnDownload` method is procs if
<br></br>
there is no file with given **stashId**.
#### Initialize
initialize local stash object with given **unique** identifier.
<br></br>
pass methods for error handling and downstream operation.
```csharp
 public Stash stash;

 async void Start()
 {
    Task<Stash> task = Stash.Cloud(stashId, OnStashError, DownloadStash);
    await task;
    stash = task.Result;
 }

 void OnStashError(StashError error)
 {
    Debug.LogError(error);
 }

 // this is just an example you can work with any backend service.
 async void DownloadStash(System.Action<byte []> initStash)
 {
    FirebaseStorage storage = FirebaseStorage.DefaultInstance;
    StorageReference root = storage.RootReference;
    StorageReference bytes = storage.GetReference("bytes.dat");
    const long maxAllowedSize = 1 * 1024 * 1024;
    var downloadTask = bytes.GetBytesAsync(maxAllowedSize);
    await downloadTask;
    // overrides the current values of stash (make sure you did
    initStash(downloadTask.Result);
 }

```

## Local Stash - Persistent

#### Initialize
initialize local stash object with given **unique** identifier.
<br></br>
register a method for error handling.
```csharp
 stash = Stash.PersistentPath(stashId, OnStashError);

 void OnStashError(StashError error)
 {
     Debug.LogError(error);
 }
```
#### Set 
You can save any object marked as [Serializable]
<br></br>
 `Set<T>(string key, T value)` creates or updates given key-value pair 
```csharp 
 stash.Set("someInteger", 5);
 GameConfig writeConfig = new GameConfig();
 stash.Set("gameConfig", writeConfig);
```
#### Get 
For fail safe `Get<T>(string key, T defaultValue)` method requires defaultValue<br>
If given key-value pair is not exist then defaultValue will be returned
```csharp 
 int integer = stash.Get("someInteger" , 0);
 GameConfig readConfig = stash.Get("gameConfig", new GameConfig());
```
#### Save Stash 
Stash is not saved to disk until you call `stash.Save()` method
<br></br>
stash.Save() blocks UIThread until operation is completed.
<br></br>
NOTE: Dont save huge files(10 mb or more) when processing exit message (OS) may suspend your save operation
<br></br>
you may end up with broken save file.
```csharp 
 stash.Save(); // can be called any time during the game.
  // on application killed - make sure stash is saved.
 private void OnApplicationQuit()
 {
    stash.Save();
 }
 // on application suspended (home button) - make sure stash is saved.
 private void OnApplicationPause(bool pause)
 {
    if(pause)
    {
        stash.Save();
    }
 }
```
#### Save Stash - Async Non Blocking
If the game get suspended or killed during async save operation
<br></br>
your file may be **corrupted**. I suggest `stash.Save()` whenever possible.
```csharp 
 stash.SaveAsync(OnSaved);

 void OnSaved(Stash stash)
 {
    Debug.Log(stash.id);    
 }
```
