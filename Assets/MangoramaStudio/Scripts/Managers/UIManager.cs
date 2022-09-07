using MangoramaStudio.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class UIManager : CustomBehaviour
{
    public MainMenuPanel mainMenuPanel;
    public HudPanel hudPanel;
    public DiamondPanel diamondPanel;
    public InGamePanel _inGamePanel;
    public FinishPanel finishPanel;
    public SettingsPanel settingsPanel;

    private List<UIPanel> UIPanels;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        UIPanels = new List<UIPanel> {mainMenuPanel, hudPanel, diamondPanel, finishPanel, settingsPanel, _inGamePanel };

        UIPanels.ForEach(x =>
        {
            x.Initialize(this);
            x.gameObject.SetActive(false);
        });
        //mainMenuPanel.ShowPanel();
        _inGamePanel.ShowPanel();
    }
}