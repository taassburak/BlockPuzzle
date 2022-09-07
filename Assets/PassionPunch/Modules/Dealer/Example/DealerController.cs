using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PassionPunch.Dealer;
using PassionPunch.Sherlock;

public class DealerController : MonoBehaviour
{

    public void Start()
    {
        InitializeDealer();
        SetListeners();
    }

    public void InitializeDealer()
    {
        Dealer.Instance.Initialize();
    }

    private void SetListeners()
    {
        Dealer.Instance.onProductPurchasedCallBack += OnProductPurchasedCallBack;
        Dealer.Instance.onProductPurchasedCallBackWithAnalytics += OnProductPurchasedCallBackWithAnalytics;
        Dealer.Instance.onPreviouslyPurchasedProductCallback += OnPreviouslyPurchasedProductCallback;
    }


    private void OnPreviouslyPurchasedProductCallback(string purchasedProduct)
    {
        switch (purchasedProduct)
        {
            case "pack1":
                break;

            case "pack2":
                break;

            case "subscription":
                break;
        }
    }

    private void OnProductPurchasedCallBack(string purchasedProduct)
    {
        switch (purchasedProduct)
        {
            case "pack1":
                break;

            case "pack2":
                break;

            case "subscription":
                Debug.Log("Subscription is active");
                //Check if not subscribed before
                break;
        }
    }

    private void OnProductPurchasedCallBackWithAnalytics(string purchasedProduct, float price, string currencyCode, string receipt)
    {
        switch (purchasedProduct)
        {
            case "pack1":
                Dealer.Instance.PrepareAndSendPurchaseDataForAdmost(new string[] { "iap_pack_1" }, price, currencyCode, receipt, "Adjust Event Code");
#if PP_SHERLOCK
                Sherlock.Instance.AdjustIAPEvents(Sherlock.Instance.settings.customEvents.GetEventCode("pack1"), price, receipt, currencyCode);
#endif
                break;

            case "pack2":
                Dealer.Instance.PrepareAndSendPurchaseDataForAdmost(new string[] { "iap_pack_1" },price, currencyCode, receipt, "Adjust Event Code");
#if PP_SHERLOCK
                Sherlock.Instance.AdjustIAPEvents(Sherlock.Instance.settings.customEvents.GetEventCode("pack2"), price, receipt, currencyCode);
#endif
                break;

            case "subscription":
                Dealer.Instance.PrepareAndSendPurchaseDataForAdmost(new string[] { "subscription" }, price, currencyCode, receipt, "Adjust Event Code");
#if PP_SHERLOCK
                Sherlock.Instance.AdjustIAPEvents(Sherlock.Instance.settings.customEvents.GetEventCode("subscription"), price, receipt, currencyCode);
#endif
                break;
        }
#if PP_SHERLOCK
        Sherlock.Instance.AdjustIAPEvents(Sherlock.Instance.settings.customEvents.GetEventCode("purhcase_total"), price, receipt, currencyCode);
#endif
    }


    public void PurchaseProduct()
    {
        Dealer.Instance.PurchaseProduct("pack1");
    }
    public void GetItemPrice()
    {
        Dealer.Instance.GetItemPrice("pack2");
    }
    public void IsProductPurchased()
    {
        Dealer.Instance.IsProductPurchased("pack1");
    }
    public void RestorePurchases()
    {
        Dealer.Instance.RestorePurchases();
    }
}
