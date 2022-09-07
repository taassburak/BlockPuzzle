using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using PassionPunch.Stash;

//Computer\HKEY_CURRENT_USER\SOFTWARE\Unity\UnityEditor\DefaultCompany\ConfigManager
public class StashPlayerPrefs : Stash
{
    public StashPlayerPrefs(string id, StashLocation stashLocation) : base(id, stashLocation)
    {
    }

    public override void Save()
    {
        // From string to byte array
        // utf-8 throws error 64 seems to be okay.
        byte [] encrypted = ToEncryptedBytes(hashtable);

        if(encrypted.Length < 1) // error already raised.
        {
            return;
        }

        string value = Convert.ToBase64String(encrypted);
        UnityEngine.PlayerPrefs.SetString(id, value);
        UnityEngine.PlayerPrefs.Save(); // force to save
    }

    public override void SaveAsync(System.Action<Stash> onCompleted)
    {
        Save();
        onCompleted.SafeInvoke(this);
    }
}
