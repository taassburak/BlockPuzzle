using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.UI;
using static UnityEngine.Debug;
using System.Globalization;
using Iso;

public class DiamondPanel : UIPanel
{
    public AmountDisplay amountDisplay;
    public Image Icon;
    private int _diamondIconTmp = -1, _diamondAddTmp = -1;
    public Text negDisplay;
    private Vector3 negDisplayIniPos;

    public override void Initialize(UIManager uiManager)
    {
        base.Initialize(uiManager);
        amountDisplay.Initialize(GameManager);
        negDisplayIniPos = negDisplay.transform.localPosition;
        negDisplay.SetActive(false);
    }

    /****************************************************************************************/

    public void PlayAnimation(int _amount, bool withTextAnim, bool withIconAnim)
    {
        var _diamondAmount0 = GameManager.DiamondManager.DiamondAmount;
        var _diamondAmount1 = GameManager.DiamondManager.DiamondAmount + _amount;
        if (withTextAnim) DOTween.To(() => _diamondAmount0, x => _diamondAmount0 = x, _diamondAmount1, 1.25f).OnUpdate(() => GameManager.DiamondManager.SetTotalAmount(_diamondAmount0)).SetDelay(0f);
        else
        {
            GameManager.DiamondManager.AddDiamond(_amount);
        }

        if (withIconAnim) PlayIconAnimation();

        if (_amount < 0) PlayNegAnimation(_amount);
    }

    private void PlayNegAnimation(int _amount)
    {
        DOTween.Kill(negDisplay);
        negDisplay.transform.localPosition = negDisplayIniPos;
        var duration = 2;
        negDisplay.color = Color.white;
        negDisplay.text = _amount.ToString();
        negDisplay.SetActive(true);
        negDisplay.transform.localPosition = negDisplayIniPos;
        negDisplay.transform.DOLocalMoveY(100, 2).OnComplete(() => negDisplay.SetActive(false));
        negDisplay.DOFade(0, 0);
        negDisplay.DOFade(1, 1).SetLoops(2, LoopType.Yoyo);
    }

    private void PlayIconAnimation()
    {
        ShowPanel();
        StartCoroutine(PlayIconAnimationIe());
    }

    private IEnumerator PlayIconAnimationIe()
    {
        var counter = 0;
        while (counter < 5)
        {
            counter++;
            var newIcon = Instantiate(Icon.gameObject);

            newIcon.SetActive(true);
            var startPos = Icon.transform.position - new Vector3(300, 600, 0);
            newIcon.transform.parent = transform.root;
            newIcon.transform.localScale = Vector3.one;
            newIcon.transform.position = startPos;
            newIcon.transform.DOScale(Vector3.one * 2, .5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
            newIcon.transform.DOMove(Icon.transform.position, 1).OnComplete(() => Destroy(newIcon)).SetEase(Ease.InOutSine);
            newIcon.transform.parent = Icon.transform.parent;
            yield return new WaitForSeconds(.1f);
        }
    }


    /*************************************DEFAULT********************************************/
}