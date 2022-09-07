using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPanel : UIPanel
{

    public GameObject WinPanel;
    public GameObject LosePanel;

    public CustomButton ButtonNext;
    public CustomButton ButtonRetry;


    public override void Initialize(UIManager uiManager)
    {
        base.Initialize(uiManager);

        ButtonNext.Initialize(uiManager, ClickNext);
        ButtonRetry.Initialize(uiManager, ClickRetry);

        GameManager.EventManager.OnLevelFinished += SetWinLosePanel;
        GameManager.EventManager.OnLevelStarted += RefreshPanels;
        GameManager.EventManager.OnLevelRestarted += RefreshPanels;
    }


    private void OnDestroy()
    {
        GameManager.EventManager.OnLevelFinished -= SetWinLosePanel;
        GameManager.EventManager.OnLevelStarted -= RefreshPanels;
        GameManager.EventManager.OnLevelRestarted -= RefreshPanels;
    }

    /****************************************************************************************/
    private void RefreshPanels()
    {
        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
    }

    private void SetWinLosePanel(bool isSuccess)
    {
        ShowPanel();

        StartCoroutine(SetWinLosePanelCo(isSuccess));
    }

    private IEnumerator SetWinLosePanelCo(bool isSuccess)
    {
        var delayTime = isSuccess == true ? FindObjectOfType<LevelBehaviour>().WinPanelDelayTime : FindObjectOfType<LevelBehaviour>().LosePanelDelayTime;

        yield return new WaitForSeconds(delayTime);

        WinPanel.SetActive(isSuccess);
        LosePanel.SetActive(!isSuccess);
    }


    private void ClickNext()
    {

    }

    private void ClickRetry()
    {

    }
}
