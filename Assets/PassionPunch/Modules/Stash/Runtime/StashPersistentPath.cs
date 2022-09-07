using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using PassionPunch.Stash;

public class StashPersistentPath : Stash
{

    public StashPersistentPath(string id, StashLocation stashLocation) : base(id, stashLocation)
    {
    }
    public override void Save()
    {
        byte [] encrypted = ToEncryptedBytes(hashtable);
        if(encrypted.Length < 1)// error already raised
        {
            return;
        }
        string path = StashUtils.GetPersistentPath(id);
        System.IO.File.WriteAllBytes(path, encrypted);
    }

    public override async void SaveAsync(System.Action<Stash> onComplete)
    {
        byte [] encrypted = ToEncryptedBytes(hashtable);
        if(encrypted.Length < 1) // error already raised
        {
            return;
        }

        string path = StashUtils.GetPersistentPath(id);

        using(FileStream SourceStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, bufferSize: encrypted.Length, useAsync: true))
        {
            await SourceStream.WriteAsync(encrypted, 0, encrypted.Length);
        }

        onComplete.SafeInvoke(this);
    }
}
