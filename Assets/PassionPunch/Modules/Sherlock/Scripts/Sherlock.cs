using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;
#endif
#if PP_ADJUST
using com.adjust.sdk;
#endif
#if PP_FIREBASE
using Firebase.Analytics;
#endif
#if PP_FACEBOOK
using Facebook.Unity;
#endif
namespace PassionPunch.Sherlock
{
    public class Sherlock : MonoBehaviour
    {
        public static Sherlock Instance;
        public SherlockSettings settings;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        
        public void Initialize(){}
        
        private void Start()
        {
            //Adjust Sdk Configurations
#if PP_ADJUST
            Adjust_Initialize();
#endif

#if PP_FIREBASE
            Firebase_Initialize();
#endif
#if PP_FACEBOOK
            FB_Initialize();
#endif
        }


        #region Adjust Analytic Methods
#if PP_ADJUST
        public AdjustAttribution AdjustData;

        //ADJUST
        public void Adjust_Initialize()
        {
            string appToken = Sherlock.Instance.settings.adjustIOSAppToken;

#if UNITY_ANDROID
            appToken = Sherlock.Instance.settings.adjustAndroidAppToken;
#endif
            AdjustEnvironment environment = AdjustEnvironment.Production;
            AdjustConfig config = new AdjustConfig(appToken, environment, true);

            config.setLogLevel(AdjustLogLevel.Suppress);

            config.setSendInBackground(true);
            config.setLaunchDeferredDeeplink(true);

            config.setLogDelegate(msg => this.Print(msg));
            config.setEventSuccessDelegate(EventSuccessCallback);
            config.setEventFailureDelegate(EventFailureCallback);

            config.setDeferredDeeplinkDelegate(DeferredDeeplinkCallback);
            config.setAttributionChangedDelegate(SetAdjustIdToAdmost);
            Adjust.start(config);

            StartCoroutine(SendTokenForUninstallTrack());
        }

        public void EventSuccessCallback(AdjustEventSuccess eventSuccessData)
        {
            this.Print("Event tracked successfully!");

            if (eventSuccessData.Message != null)
            {
                this.Print("Message: " + eventSuccessData.Message);
            }
            if (eventSuccessData.Timestamp != null)
            {
                this.Print("Timestamp: " + eventSuccessData.Timestamp);
            }
            if (eventSuccessData.Adid != null)
            {
                this.Print("Adid: " + eventSuccessData.Adid);
            }
            if (eventSuccessData.EventToken != null)
            {
                this.Print("EventToken: " + eventSuccessData.EventToken);
            }
            if (eventSuccessData.CallbackId != null)
            {
                this.Print("CallbackId: " + eventSuccessData.CallbackId);
            }
            if (eventSuccessData.JsonResponse != null)
            {
                this.Print("JsonResponse: " + eventSuccessData.GetJsonResponse());
            }
        }
        public void EventFailureCallback(AdjustEventFailure eventFailureData)
        {
            this.Print("Event tracking failed!");

            if (eventFailureData.Message != null)
            {
                this.Print("Message: " + eventFailureData.Message);
            }
            if (eventFailureData.Timestamp != null)
            {
                this.Print("Timestamp: " + eventFailureData.Timestamp);
            }
            if (eventFailureData.Adid != null)
            {
                this.Print("Adid: " + eventFailureData.Adid);
            }
            if (eventFailureData.EventToken != null)
            {
                this.Print("EventToken: " + eventFailureData.EventToken);
            }
            if (eventFailureData.CallbackId != null)
            {
                this.Print("CallbackId: " + eventFailureData.CallbackId);
            }
            if (eventFailureData.JsonResponse != null)
            {
                this.Print("JsonResponse: " + eventFailureData.GetJsonResponse());
            }

            this.Print("WillRetry: " + eventFailureData.WillRetry.ToString());
        }

        private IEnumerator SendTokenForUninstallTrack()
        {
            yield return new WaitForSeconds(5f);
            string appToken = "";
#if UNITY_ANDROID
            appToken = "";

#elif UNITY_IOS
        byte[] token = UnityEngine.iOS.NotificationServices.deviceToken;
        if (token != null)
        {
            string hexToken = System.BitConverter.ToString(token).Replace("-", "");
            appToken = hexToken;
        }
#endif
            Adjust.setDeviceToken(appToken);
        }

        private void SetAdjustIdToAdmost(AdjustAttribution obj)
        {
#if PP_ADMOST
            AMR.AMRSDK.setAdjustUserId(obj.adid);
            AdjustData = obj;
#endif
        }

        public void Adjust_TrackEvent(string token)
        {
            if (token != null && !token.Equals(string.Empty))
            {
                AdjustEvent adjustEvent = new AdjustEvent(token);
                Adjust.trackEvent(adjustEvent);
            }
        }

        public void AdjustTrackEventWithCallback(string token, string callbackKey, string callbackValue)
        {
            AdjustEvent adjustEvent = new AdjustEvent(token);
            adjustEvent.addCallbackParameter(callbackKey, callbackValue);
            Adjust.trackEvent(adjustEvent);
        }

        public void AdjustRevenueEvent(string token, double ecpm)
        {
            AdjustEvent adjustEvent = new AdjustEvent(token);
            double revenueUsd = ecpm / (1000 * 100); //calculate dollar amount for one user, ecpm value is cent for 1000 users
            adjustEvent.setRevenue(revenueUsd, "USD");
            Adjust.trackEvent(adjustEvent);
        }

        private void DeferredDeeplinkCallback(string deeplinkURL)
        {
            this.Print("Deeplink URL: " + deeplinkURL);

            if (string.IsNullOrEmpty(deeplinkURL))
                return;

            //Send Event if Needed
            //OnDeeplink(deeplinkURL); 

            //var param = deeplinkURL.Split('=')[1];
        }

        public void OnDeeplink(string deeplink)
        {
            AdjustEvent adjustEvent = new AdjustEvent("");//put event token here
            adjustEvent.addCallbackParameter("deeplink", deeplink); // optional, for callback support
            Adjust.trackEvent(adjustEvent);
        }

        //Adjust IAP Events
        public void AdjustIAPEvents(string productIdentifierKey, float price, string transaction, string isoCurrencyCode)
        {
            this.Print("Sending total purchase analitycs data to Adjust: " + productIdentifierKey + " " + price);
            //Adjust Purchase track
            AdjustEvent adjustEvent = new AdjustEvent(productIdentifierKey);
            adjustEvent.setRevenue((float)(0.7f * price), isoCurrencyCode);
            adjustEvent.setTransactionId(transaction);
            Adjust.trackEvent(adjustEvent);
        }
#endif
        #endregion

        #region Firebase Analytic Methods
#if PP_FIREBASE
        public void Firebase_Initialize()
        {
            this.Print("Enabling data collection.");
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

            this.Print("Set user properties.");
            // Set the user's sign up method.
            FirebaseAnalytics.SetUserProperty(
                FirebaseAnalytics.UserPropertySignUpMethod,
                "Google");
        }

        public void Firebase_TrackEvent(string eventName, Parameter[] parameters)
        {
            FirebaseAnalytics.LogEvent(eventName, parameters);
        }

        public void Firebase_TrackEvent(string text)
        {
            FirebaseAnalytics.LogEvent(text);
            this.Print("fire log event: " + text);
        }
        public void Firebase_TrackEvent(string token, string key, int value)
        {
            FirebaseAnalytics.LogEvent(token, key, value);
            this.Print("fire log event: " + token + " " + key + " " + value);
        }
        public void Firebase_TrackEvent(string token, string key, string value)
        {
            FirebaseAnalytics.LogEvent(token, key, value);
            this.Print("fire log event: " + token + " " + key + " " + value);
        }
        public void Firebase_TrackEvent(string token, string key, float value)
        {
            FirebaseAnalytics.LogEvent(token, key, value);
            this.Print("fire log event: " + token + " " + key + " " + value);
        }
#endif
        #endregion

        #region Facebook Analytic Methods
#if PP_FACEBOOK
        public void FB_Initialize()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        }
        public void FB_TrackEvent(string eventName)
        {
            FB.LogAppEvent(eventName);
        }

        public void FB_TrackEventWithParams(string eventToken,float? valueToSum = null, Dictionary<string, object> eventParams = null)
        {
            FB.LogAppEvent(eventToken, valueToSum, eventParams);
        }

        public void FB_TrackEventWithParams(string eventToken, Dictionary<string, object> eventParams)
        {
            FB.LogAppEvent(eventToken, parameters: eventParams);
        }

        // Unity will call OnApplicationPause(false) when an app is resumed
        // from the background
        void OnApplicationPause(bool pauseStatus)
        {
            // Check the pauseStatus to see if we are in the foreground
            // or background
            if (!pauseStatus)
            {
                //app resume
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
                }
                else
                {
                    //Handle FB.Init
                    FB.Init(() => {
                        FB.ActivateApp();
                    });
                }
            }
        }

        public void FB_CollectionOfADvertiserIDS(bool isActive)
        {
            FB.Mobile.SetAdvertiserIDCollectionEnabled(isActive);
        }

        public void FB_AutoLoggedEventsState(bool isActive)
        {
            FB.Mobile.SetAutoLogAppEventsEnabled(false);
        }
#endif
        #endregion
    }
}
