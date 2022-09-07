using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : UIPanel
{
    public CustomToggle HapticToggle;
    public CustomToggle MusicToggle;
    public CustomToggle SfxToggle;

    public CustomButton CloseButton;

    public override void Initialize(UIManager uiManager)
    {
        base.Initialize(uiManager);

        HapticToggle.Initialize(uiManager, ChangeHaptic);
        MusicToggle.Initialize(uiManager, ChangeMusic);
        SfxToggle.Initialize(uiManager, ChangeSfx);

        CloseButton.Initialize(uiManager, CloseSettings);
    }

    private void OnDestroy()
    {

    }

    private void CloseSettings()
    {
        HidePanel();
    }

    private void ChangeHaptic()
    {
        
    }

    private void ChangeSfx()
    {
        GameManager.AudioManager.IsSfxEnabled = SfxToggle.isOn;
    }

    private void ChangeMusic()
    {
        GameManager.AudioManager.IsMusicEnabled = MusicToggle.isOn;
    }

}
