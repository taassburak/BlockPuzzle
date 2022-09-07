using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Iso;
using UnityEngine;
using UnityEngine.UI;

public class AmountDisplay : CustomBehaviour
{
    private Text textComp;

    #region Initialize

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        textComp = GetComponent<Text>();
        GameManager.EventManager.OnChangeDiamondAmount += ShowAmount;
    }

    private void OnDestroy()
    {
        GameManager.EventManager.OnChangeDiamondAmount -= ShowAmount;
    }

    #endregion

    /****************************************************************************************/

    private void ShowAmount(int amount)
    {
        textComp.text = amount.ToString("#,#", CultureInfo.InvariantCulture);
    }
}