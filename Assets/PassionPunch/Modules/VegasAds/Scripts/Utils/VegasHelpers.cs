using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegasHelpers
{
    private static bool isGDPRApplicable;
    public static bool IsGDPRApplicable
    {
        get
        {
            isGDPRApplicable = PlayerPrefs.HasKey("VegasIsGDPRApplicable") ? (PlayerPrefs.GetInt("VegasIsGDPRApplicable") == 0 ? false : true) : false;
            return isGDPRApplicable;
        }
        set
        {
            PlayerPrefs.SetInt("VegasIsGDPRApplicable", value == true ? 1 : 0);
            isGDPRApplicable = value;
        }
    }
    private static bool m_adConsent;
    public static bool AdConsent
    {
        get
        {
            m_adConsent = PlayerPrefs.HasKey("VegasAdConsent") ? (PlayerPrefs.GetInt("VegasAdConsent") == 0 ? false : true) : true;
            return m_adConsent;
        }
        set
        {
            PlayerPrefs.SetInt("VegasAdConsent", value == true ? 1 : 0);
            m_adConsent = value;
        }
    }
}
