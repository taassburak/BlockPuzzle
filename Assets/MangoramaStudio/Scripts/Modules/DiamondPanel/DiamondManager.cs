using System;
using System.Collections;
using System.Collections.Generic;
using Iso;
using UnityEngine;
using static UnityEngine.Debug;

public class DiamondManager : CustomBehaviour
{
    private DiamondPanel diamondPanel;

    #region Initialize

    private int[] currentLevelGift;

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        GameManager.EventManager.OnAddDiamond += PlayAnimations;
        diamondPanel = GameManager.UIManager.diamondPanel as DiamondPanel;

        // SetTotalAmount(90);
    }

    private void OnDestroy()
    {
        GameManager.EventManager.OnAddDiamond -= PlayAnimations;
    }

    #endregion

    /****************************************************************************************/
    public void PlayAnimations(int value, bool withTextAnim, bool withIconAnim)
    {
        diamondPanel.PlayAnimation(value, withTextAnim, withIconAnim);
    }

    public void AddDiamond(int amount)
    {
        DiamondAmount += amount;
        GameManager.EventManager.ChangeDiamondAmount(DiamondAmount);
    }

    public void SetTotalAmount(int amount)
    {
        DiamondAmount = amount;
        GameManager.EventManager.ChangeDiamondAmount(DiamondAmount);
    }

    public int DiamondAmount
    {
        get => PlayerPrefs.GetInt("DiamondAmount", 0);
        private set => PlayerPrefs.SetInt("DiamondAmount", value);
    }

    private void ShowDiamondPanel()
    {
        diamondPanel.ShowPanel();
    }

    /*************************************EXTRA METHODS********************************************/
}