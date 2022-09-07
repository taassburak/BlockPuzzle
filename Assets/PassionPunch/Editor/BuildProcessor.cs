#if UNITY_IOS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEditor.iOS.Xcode;

[Serializable]
public class SkAdNetworkData
{
    [SerializeField] public Network[] Networks;
}

[Serializable]
public class Network
{
    [SerializeField] public string name;
    [SerializeField] public string[] skAdNetwork;
}

public class BuildProcessor
{
	[PostProcessBuild(int.MaxValue)]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
	{

        // Get plist
        string plistPath = pathToBuiltProject + "/Info.plist";
        PlistDocument plist = new PlistDocument();
        plist.ReadFromString(File.ReadAllText(plistPath));
 
        // Get root
        PlistElementDict rootDict = plist.root;
 
        // Set encryption usage boolean
        string ITSAppUsesNonExemptEncryption = "ITSAppUsesNonExemptEncryption";
        rootDict.SetBoolean(ITSAppUsesNonExemptEncryption, false);

        //firebase calismayinca ekle !
        string GADIsAdManagerApp = "GADIsAdManagerApp";
        //  rootDict.SetBoolean(GADIsAdManagerApp, true);

        string GADApplicationIdentifier = "GADApplicationIdentifier";
        rootDict.SetString(GADApplicationIdentifier, "AdmobAppIdIOS");

        string AppLovinSdkKey = "AppLovinSdkKey";
        rootDict.SetString(AppLovinSdkKey, "AppLovinSdkKey");
        
        // Set location usage info
        string NSLocationAlwaysAndWhenInUseUsageDescription = "NSLocationAlwaysAndWhenInUseUsageDescription";
        if (!rootDict.values.ContainsKey(NSLocationAlwaysAndWhenInUseUsageDescription))
        {
            rootDict.SetString(NSLocationAlwaysAndWhenInUseUsageDescription, "App needs Location access");
        }
        string NSLocationAlwaysUsageDescription = "NSLocationAlwaysUsageDescription";
        if (!rootDict.values.ContainsKey(NSLocationAlwaysUsageDescription))
        {
            rootDict.SetString(NSLocationAlwaysUsageDescription, "App needs Location access");
        }
        string NSLocationWhenInUseUsageDescription = "NSLocationWhenInUseUsageDescription";
        if (!rootDict.values.ContainsKey(NSLocationWhenInUseUsageDescription))
        {
            rootDict.SetString(NSLocationWhenInUseUsageDescription, "App needs Location access");
        }

        // remove exit on suspend if it exists.
        string UIApplicationExitsOnSuspend = "UIApplicationExitsOnSuspend";
        if(rootDict.values.ContainsKey(UIApplicationExitsOnSuspend))
        {
            rootDict.values.Remove(UIApplicationExitsOnSuspend);
        }

        string NSCalendarsUsageDescription = "NSCalendarsUsageDescription";
        if (!rootDict.values.ContainsKey(NSCalendarsUsageDescription))
        {
            rootDict.SetString(NSCalendarsUsageDescription, "Some ad content may access calendar");
        }

        string NSCameraUsageDescription = "NSCameraUsageDescription";
        if (!rootDict.values.ContainsKey(NSCameraUsageDescription))
        {
            rootDict.SetString(NSCameraUsageDescription, "App needs cam access");
        }

        string NSMicrophoneUsageDescription = "NSMicrophoneUsageDescription";
        if (!rootDict.values.ContainsKey(NSMicrophoneUsageDescription))
        {
            rootDict.SetString(NSMicrophoneUsageDescription, "App needs mic access");
        }

        const string trackingDescription = "This identifier will be used to deliver personalized ads to you. ";
        rootDict.SetString("NSUserTrackingUsageDescription", trackingDescription);

#if PP_ADMOST
        AddSkAdNetworksInfoIfNeeded(plist);

        UpdateAppTransportSecuritySettingsIfNeeded(plist);
#endif  
        
        // Write to file
        File.WriteAllText(plistPath, plist.WriteToString());

    }

#if PP_ADMOST
    private static void AddSkAdNetworksInfoIfNeeded(PlistDocument plist)
        {
            var skAdNetworkData = GetSkAdNetworkData();
            var activeNetworks = GetCurrentNetworksFromXML();
            HashSet<string> skIds = new HashSet<string>();
            foreach (var network in skAdNetworkData.Networks)
            {
                if (activeNetworks.Contains(network.name))
                {
                    foreach (var networkSkAdId in network.skAdNetwork)
                    {
                        skIds.Add(networkSkAdId);
                    }
                }
            }
            var skAdNetworkIds = skIds.ToArray();
            // Check if we have a valid list of SKAdNetworkIds that need to be added.
            if (skAdNetworkIds == null || skAdNetworkIds.Length < 1) return;

            //
            // Add the SKAdNetworkItems to the plist. It should look like following:
            //
            //    <key>SKAdNetworkItems</key>
            //    <array>
            //        <dict>
            //            <key>SKAdNetworkIdentifier</key>
            //            <string>ABC123XYZ.skadnetwork</string>
            //        </dict>
            //        <dict>
            //            <key>SKAdNetworkIdentifier</key>
            //            <string>123QWE456.skadnetwork</string>
            //        </dict>
            //        <dict>
            //            <key>SKAdNetworkIdentifier</key>
            //            <string>987XYZ123.skadnetwork</string>
            //        </dict>
            //    </array>
            //
            PlistElement skAdNetworkItems;
            plist.root.values.TryGetValue("SKAdNetworkItems", out skAdNetworkItems);
            var existingSkAdNetworkIds = new HashSet<string>();
            // Check if SKAdNetworkItems array is already in the Plist document and collect all the IDs that are already present.
            if (skAdNetworkItems != null && skAdNetworkItems.GetType() == typeof(PlistElementArray))
            {
                var plistElementDictionaries = skAdNetworkItems.AsArray().values.Where(plistElement => plistElement.GetType() == typeof(PlistElementDict));
                foreach (var plistElement in plistElementDictionaries)
                {
                    PlistElement existingId;
                    plistElement.AsDict().values.TryGetValue("SKAdNetworkIdentifier", out existingId);
                    if (existingId == null || existingId.GetType() != typeof(PlistElementString) || string.IsNullOrEmpty(existingId.AsString())) continue;

                    existingSkAdNetworkIds.Add(existingId.AsString());
                }
            }
            // Else, create an array of SKAdNetworkItems into which we will add our IDs.
            else
            {
                skAdNetworkItems = plist.root.CreateArray("SKAdNetworkItems");
            }

             
            foreach (var skAdNetworkId in skAdNetworkIds)
            {
                // Skip adding IDs that are already in the array.
                if (existingSkAdNetworkIds.Contains(skAdNetworkId)) continue;

                var skAdNetworkItemDict = skAdNetworkItems.AsArray().AddDict();
                skAdNetworkItemDict.SetString("SKAdNetworkIdentifier", skAdNetworkId);
            }
            
        }

        //[MenuItem("PassionPunch/Download")]
        private static SkAdNetworkData GetSkAdNetworkData()
        {
            var uriBuilder = new UriBuilder("https://raw.githubusercontent.com/admost/amrios/master/docs/ios_data.js");

            var unityWebRequest = UnityWebRequest.Get(uriBuilder.ToString());

#if UNITY_2017_2_OR_NEWER
            var operation = unityWebRequest.SendWebRequest();
#else
            var operation = unityWebRequest.Send();
#endif
            // Wait for the download to complete or the request to timeout.
            while (!operation.isDone) { }


#if UNITY_2020_1_OR_NEWER
            if (unityWebRequest.result != UnityWebRequest.Result.Success)
#elif UNITY_2017_2_OR_NEWER
            if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
#else
            if (unityWebRequest.isError)
#endif
            {
                Debug.Log("Failed to retrieve SKAdNetwork IDs with error: " + unityWebRequest.error);
                return new SkAdNetworkData();
            }

            try
            {
                string result = unityWebRequest.downloadHandler.text;
                result = result.Split('`')[3];
                return JsonUtility.FromJson<SkAdNetworkData>("{ \"Networks\":"+result+"}");
            }
            catch (Exception exception)
            {
                Debug.Log("Failed to parse data '" + unityWebRequest.downloadHandler.text + "' with exception: " + exception);
                return new SkAdNetworkData();
            }
        }

        //[MenuItem("PassionPunch/Networks")]
        private static List<string> GetCurrentNetworksFromXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            List<string> networkList = new List<string>();
            try
            {
                xmlDoc.LoadXml(File.ReadAllText("Assets/AMR/Editor/AMRDependencies.xml"));
            }
            catch (Exception)
            {
                return networkList;
            }

            try
            {
                XmlNode networks = xmlDoc.SelectSingleNode("dependencies/iosPods");
                for (int i = 1; i < networks.ChildNodes.Count; i++)
                {
                    XmlNode item = networks.ChildNodes[i];
                    string name = item.SelectSingleNode("@name").Value.Substring("AMRAdapter".Length);
                    networkList.Add(name);
                }
                return networkList;
            }
            catch (Exception e)
            {
                return networkList;
            }
        }
        private static void UpdateAppTransportSecuritySettingsIfNeeded(PlistDocument plist)
        {
            var root = plist.root.values;
            PlistElement atsRoot;
            root.TryGetValue("NSAppTransportSecurity", out atsRoot);

            if (atsRoot == null || atsRoot.GetType() != typeof(PlistElementDict))
            {
                // Add the missing App Transport Security settings for publishers if needed. 
                Debug.Log("Adding App Transport Security settings...");
                atsRoot = plist.root.CreateDict("NSAppTransportSecurity");
                atsRoot.AsDict().SetBoolean("NSAllowsArbitraryLoads", true);
            }

            var atsRootDict = atsRoot.AsDict().values;
            // Check if both NSAllowsArbitraryLoads and NSAllowsArbitraryLoadsInWebContent are present and remove NSAllowsArbitraryLoadsInWebContent if both are present.
            if (atsRootDict.ContainsKey("NSAllowsArbitraryLoads") && atsRootDict.ContainsKey("NSAllowsArbitraryLoadsInWebContent"))
            {
                Debug.Log("Removing NSAllowsArbitraryLoadsInWebContent");
                atsRootDict.Remove("NSAllowsArbitraryLoadsInWebContent");
            }
        }
#endif 
}
#endif 