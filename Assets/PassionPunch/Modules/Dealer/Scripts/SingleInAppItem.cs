using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PassionPunch.Dealer
{
    public enum IAPType { subscription, consumable, nonConsumable }
    public class SingleInAppItem : ScriptableObject
    {
        public IAPType inappType;
        public string inappProduct;
        [Tooltip("Just a reminder for price. Not related with real world")]
        public string price;
        public string androidID;
        public string iosID;

        public string GetProductID()
        {
#if UNITY_ANDROID
            return androidID;
#elif UNITY_IOS
        return iosID;
#else
        return string.Empty;
#endif
        }
    }
}
