﻿#if UNITY_ANDROID
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEditor;
using UnityEditor.Build;
#if UNITY_2018_1_OR_NEWER
using UnityEditor.Build.Reporting;
#endif
using UnityEngine;


#if UNITY_2018_1_OR_NEWER
public class ManifestProcessor : IPreprocessBuildWithReport
#else
public class ManifestProcessor : IPreprocessBuild
#endif
{
    private const string META_APPLICATION_ID = "com.google.android.gms.ads.APPLICATION_ID";

    private XNamespace ns = "http://schemas.android.com/apk/res/android";

    public int callbackOrder { get { return 0; } }

#if UNITY_2018_1_OR_NEWER
    public void OnPreprocessBuild(BuildReport report)
#else
    public void OnPreprocessBuild(BuildTarget target, string path)
#endif
    {
            string appId = "ProjectConstants.AppConstants.AdmobAndroidAppId";
            var EnableAdmob = false;
            if (EnableAdmob)
            {
                string manifestPath = Path.Combine(
                        Application.dataPath, "IronSource/Plugins/Android/IronSource.plugin/AndroidManifest.xml");

                XDocument manifest = null;
                try
                {
                    manifest = XDocument.Load(manifestPath);
                }
#pragma warning disable 0168
                catch (IOException e)
#pragma warning restore 0168
                {
                    StopBuildWithMessage("AndroidManifest.xml is missing. Try re-importing the plugin.");
                }

                XElement elemManifest = manifest.Element("manifest");
                if (elemManifest == null)
                {
                    StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
                }

                XElement elemApplication = elemManifest.Element("application");
                if (elemApplication == null)
                {
                    StopBuildWithMessage("AndroidManifest.xml is not valid. Try re-importing the plugin.");
                }

                IEnumerable<XElement> metas = elemApplication.Descendants()
                        .Where(elem => elem.Name.LocalName.Equals("meta-data"));

                XElement elemAdMobEnabled = GetMetaElement(metas, META_APPLICATION_ID);

                if (appId.Length == 0)
                {
                    StopBuildWithMessage(
                        "Android AdMob app ID is empty. Please enter your app ID to run ads properly");
                }
                else if (!Regex.IsMatch(appId, "^[a-zA-Z0-9-~]*$"))
                {
                    StopBuildWithMessage(
                        "Android AdMob app ID is not valid. Please enter a valid app ID to run ads properly");
                }

                else if (elemAdMobEnabled == null)
                {
                    elemApplication.Add(CreateMetaElement(META_APPLICATION_ID, appId));
                }
                else
                {
                    elemAdMobEnabled.SetAttributeValue(ns + "value", appId);
                }

                elemManifest.Save(manifestPath);
            }
    }

    private XElement CreateMetaElement(string name, object value)
    {
        return new XElement("meta-data",
                new XAttribute(ns + "name", name), new XAttribute(ns + "value", value));
    }

    private XElement GetMetaElement(IEnumerable<XElement> metas, string metaName)
    {
        foreach (XElement elem in metas)
        {
            IEnumerable<XAttribute> attrs = elem.Attributes();
            foreach (XAttribute attr in attrs)
            {
                if (attr.Name.Namespace.Equals(ns)
                        && attr.Name.LocalName.Equals("name") && attr.Value.Equals(metaName))
                {
                    return elem;
                }
            }
        }
        return null;
    }

    private void StopBuildWithMessage(string message)
    {
        string prefix = "[IronSourceApplicationSettings] ";

        EditorUtility.DisplayDialog(
            "IronSource Developer Settings", "Error: " + message, "", "");
#if UNITY_2017_1_OR_NEWER
        throw new BuildPlayerWindow.BuildMethodException(prefix + message);
#else
        throw new OperationCanceledException(prefix + message);
#endif
    }
}

#endif
