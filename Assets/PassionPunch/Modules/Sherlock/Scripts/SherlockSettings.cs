using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PassionPunch.Sherlock
{
    public class SherlockSettings : ScriptableObject
    {
        public bool isFirebaseEnabled = false;
        public bool isAdjustEnabled = false;
        public bool isAdmostEnabled = false;
        public bool isFBAnalyticsEnabled = false;

        public SherlockEvents customEvents;
        public string adjustIOSAppToken = "";
        public string adjustAndroidAppToken = "";
    }

}
