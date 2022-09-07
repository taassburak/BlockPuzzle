using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PassionPunch.Stash
{
    public static class StashUtils
    {
        

        //public static void SaveFile(Hashtable hashtable, string filename)
        //{
        //    byte [] encrypted = ToEncryptedBytes(hashtable);
        //    string path = GetPersistentPath(filename);
        //    System.IO.File.WriteAllBytes(path, encrypted);
        //}

        public static byte [] Serialize<T>(T value)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            using(memoryStream)
            {
                formatter.Serialize(memoryStream, value);
                return memoryStream.GetBuffer();
            }
        }

        public static T Deserialize<T>(byte [] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(bytes);
            using(memoryStream)
            {
                object obj = formatter.Deserialize(memoryStream);
                return (T)obj;
            }
        }

        public static byte [] ReadFromDisk(string path)
        {
            bool exists = System.IO.File.Exists(path);
            if(exists)
            {
                return System.IO.File.ReadAllBytes(path);
            }

            return new byte [0];
        }

        public static void AddOrCreate<T>(this Hashtable hashtable, string key, T value)
        {
            if(!hashtable.ContainsKey(key))
            {
                hashtable.Add(key, value);
            }
        }

        public static void SafeInvoke(this System.Action action)
        {
            if(action != null)
            {
                action.Invoke();
            }
        }

        public static void SafeInvoke<T>(this System.Action<T> action, T value)
        {
            if(action != null)
            {
                action.Invoke(value);
            }
        }

        public static string GetPersistentPath(string filename)
        {
            return $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}{filename}.stash";
        }
    }
}
