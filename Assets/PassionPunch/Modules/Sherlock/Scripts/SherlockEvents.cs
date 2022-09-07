using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PassionPunch.Sherlock
{
    public class SherlockEvents : ScriptableObject
    {
        [Header("Firebase Events")]
        [SerializeField] private List<string> firebaseEvents;
        public List<string> FirebaseEvents => firebaseEvents;

        [Header("Adjust Events")]
        [SerializeField] private List<CustomEvent> adjustEvents;
        public List<CustomEvent> AdjustEvents => adjustEvents;

        [Header("Facebook Events")]
        [SerializeField] private List<CustomEvent> facebookEvents;
        public List<CustomEvent> FacebookEvents => facebookEvents;


        public string GetEventCode(string currentEvent)
        {
            for (int i = 0; i < adjustEvents.Count; i++)
            {
                if (currentEvent == adjustEvents[i].Name)
                {
#if UNITY_IOS
                return adjustEvents[i].eventCodeIOS;
#elif UNITY_ANDROID
                    return adjustEvents[i].eventCodeAndroid;
#endif
                }
            }
            return null;
        }
    }
    [System.Serializable]
    public struct CustomEvent
    {
        public string Name;
        public string eventCodeIOS;
        public string eventCodeAndroid;
    }

}


