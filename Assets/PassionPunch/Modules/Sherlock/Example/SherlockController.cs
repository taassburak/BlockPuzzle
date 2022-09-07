using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PassionPunch.Sherlock;
public class SherlockController : MonoBehaviour
{

    public void InitializeSherlock()
    {
        Sherlock.Instance.Initialize();
    }

    public void TrackEvent(string my_Event_Name)
    {
#if PP_FIREBASE

        Sherlock.Instance.Firebase_TrackEvent(my_Event_Name);
#endif
#if PP_FACEBOOK
        Sherlock.Instance.FB_TrackEvent(my_Event_Name);
#endif
#if PP_ADJUST
        Sherlock.Instance.Adjust_TrackEvent(my_Event_Name);
        // if event code is defined in sherlockevents.asset
        Sherlock.Instance.Adjust_TrackEvent(Sherlock.Instance.settings.customEvents.GetEventCode(my_Event_Name));
#endif



    }
    //public void SendAdjustTrackEvent(string my_Event_Code_For_Adjust)
    //{
    //    // if event code is defined in adjustevents.asset
    //    Sherlock.Instance.AdjustTrackEvent(Sherlock.Instance.settings.customEvents.GetEventCode(my_Event_Code_For_Adjust));
    //    //if event code is direct event code
    //    Sherlock.Instance.AdjustTrackEvent(my_Event_Code_For_Adjust);
    //}
    //public void SendAdjustInappTrackEvent(string productIdentifierKey, float price, string transaction, string isoCurrencyCode)
    //{
    //    Sherlock.Instance.AdjustIAPEvents(productIdentifierKey, price, transaction, isoCurrencyCode);
    //}
}
