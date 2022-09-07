using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HudPanel : UIPanel
{
    /****************************************************************************************/

    public override void Initialize(UIManager uiManager)
    {
        base.Initialize(uiManager);
    }

    private void OnDestroy()
    {
    }

    /****************************************************************************************/

    public override void ShowPanel()
    {
        base.ShowPanel();

    }
}