using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MangoramaStudio.Scripts.Data
{
    public static class PlayerData
    {
        public static int CurrentLevelId
        {
            get => PlayerPrefs.GetInt("CurrentLevelId", 1); 

            set
            {
                PlayerPrefs.SetInt("CurrentLevelId", value);
            }
        }
    }
}