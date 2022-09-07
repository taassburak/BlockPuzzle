using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PassionPunch.Dealer
{
    public class DealerSettings : ScriptableObject
    {
        public bool isRevenueCatEnabled = false;
        public bool isUnityIAPEnabled = false;
        public bool isAdjustEnabled = false;
        public bool isSherlockEnabled = false;
        public bool isAdmostEnabled = false;

        public List<SingleInAppItem> inappProducts;
        public string revenueCatAPIKey;
    }
}