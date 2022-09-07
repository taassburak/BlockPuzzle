//using Firebase.Crashlytics;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

namespace PassionPunch.Dealer
{
#if PP_REVENUECAT
public class RevenueCatController : Purchases.UpdatedPurchaserInfoListener, IPurchasers
#else
    public class RevenueCatController
#endif
    {

#if PP_REVENUECAT
    public Purchases purchases;


    private bool isInited = false;
    [HideInInspector] public List<Purchases.Package> allPackages;


    public void Initialize()
    {
#if PP_ADJUST
        StartCoroutine(SetRevenuecatToAdjust());
#endif
        SetCurrentItemPackages();
        GetPlayerPurchases();
        isInited = true;
    }
    public bool IsInitialized()
    {
        return Purchases.Instance.listener != null;
    }
#if PP_ADJUST && PP_SHERLOCK
    private IEnumerator SetRevenuecatToAdjust()
    {
        //Wait for adjust data to be filled
        yield return new WaitForSeconds(0.5f);
        //while (!Sherlock.Instance && Sherlock.Instance.adjustContoller.AdjustData == null)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        try
        {
            Purchases.Instance.AddAttributionData(JsonUtility.ToJson(Sherlock.Instance.adjustContoller.AdjustData), Purchases.AttributionNetwork.ADJUST);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            //Crashlytics.LogException(ex);
        }
    }
#endif

    //Initing inapp packages
    private void SetCurrentItemPackages()
    {
        purchases.GetOfferings((offerings, error) =>
        {
            if (error != null)
            {
                LogError(error);
            }
            else
            {
                Debug.Log("offerings received " + offerings.ToString());

                if (offerings.Current == null)
                {
                    Debug.Log("Offerings.Current is null");
                    return;
                }

                if (offerings.Current.AvailablePackages != null)
                {
                    allPackages = offerings.Current.AvailablePackages;
                }
                else
                {
                    Debug.Log("Offerings.Current.AvailablePackages is null");
                }
            }
        });
        if (allPackages == null)
        {
            allPackages = new List<Purchases.Package>();
        }
    }

    //Getting List of the player pruchases
    private void GetPlayerPurchases()
    {
        Purchases.Instance.GetPurchaserInfo((info, error) =>
        {
            print("purchaser info " + info.ActiveSubscriptions.ToArray());

            if (error != null)
            {
                LogError(error);
            }
            else
            {
                SetPreviousPurchases(info);
            }
        });
    }

    //Purchased subscriptions and nonconsumable will be processed here
    public void SetPreviousPurchases(Purchases.PurchaserInfo purchaserInfo)
    {
        foreach (string purchasedProduct in purchaserInfo.AllPurchasedProductIdentifiers)
        {
            Dealer.Instance.PreviouslyPurchasedProductCallback(Dealer.Instance.GetInappProductFromIdentifier(purchasedProduct));
        }
    }

    public void SetPreviousPurchases()
    {
        throw new NotImplementedException();
    }

    public string GetItemPrice(string currentProduct)
    {
        for (int i = 0; i < allPackages.Count; i++)
        {
            //Debug.Log("Getting Item Price: " + allPackages[i].Product.identifier +   "     "   +  Dealer.Instance.GetProductIdentifierFromInappProductType(currentProduct));
            if (allPackages[i].Product.identifier == Dealer.Instance.GetProductIdentifierFromInappProductType(currentProduct))
            {
                return allPackages[i].Product.priceString;
            }
        }
        return null;
    }


    public bool IsProductPurchased(string currentProduct)
    {
        bool isProductPurchased = false;
        //for (int i=0; i< Dealer.Instance.settings.inappProducts.Count; i++)
        //{
        //    if(currentProduct == Dealer.Instance.settings.inappProducts[i].inappProduct)
        //    {
        //        Purchases.Instance.GetPurchaserInfo((info, error) =>
        //        {
        //            if (info.Entitlements.Active.ContainsKey(Dealer.Instance.settings.inappProducts[i].GetProductID()))
        //            {
        //                isProductPurchased = true;
        //            }
        //        });
        //    }
        //}
        //Debug.Log("Is Product Purchased: " + currentProduct);
        Purchases.Instance.GetPurchaserInfo((info, error) =>
        {
            if (info.Entitlements.Active.ContainsKey(currentProduct))
            {
                isProductPurchased = true;
            }
        });
        return isProductPurchased;
    }

    //Purchase new product
    public void PurchaseProduct(string currentProduct)
    {
        if (currentProduct.Equals(string.Empty))
        {
            Debug.Log("Invalid or empty product id: " + currentProduct);
            return;
        }
        else
        {
            Debug.Log("Purchasing product : " + currentProduct + " has started.");
        }

        Purchases.Package selectedProduct = null;
        for (int i = 0; i < allPackages.Count; i++)
        {
            //Debug.Log(currentProduct + " " + allPackages[i].Product.identifier);
            if (currentProduct.Equals(allPackages[i].Product.identifier))
            {
                selectedProduct = allPackages[i];
                break;
            }
        }


        if (selectedProduct != null)
        {
            purchases.PurchasePackage(selectedProduct, (receipt, productIdentifier, purchaserInfo, userCancelled, error) =>
            {
                if (!userCancelled)
                {
                    if (error != null)
                    {
                        LogError(error);
                    }
                    else
                    {
                        PurchaseComplete(selectedProduct, productIdentifier, purchaserInfo, receipt);
                    }
                }
                else
                {
                    Debug.Log("Subtester: User cancelled, don't show an error");
                }
            });
        }
    }

    private void PurchaseComplete(Purchases.Package selectedProduct, string productIdentifier, Purchases.PurchaserInfo purchaserInfo, string receipt)
    {
        Debug.Log("Purchased Product: " + selectedProduct.Identifier + " " + productIdentifier);
        //For Analytic Tracking
        Dealer.Instance.ProductPurchasedCallback(Dealer.Instance.GetInappProductFromIdentifier(productIdentifier), selectedProduct.Product.price, selectedProduct.Product.currencyCode, receipt);
        //For Stabdart Purchase Complete
        //Dealer.Instance.ProductPurchasedCallback(Dealer.Instance.GetInappProductFromIdentifier(productIdentifier));
    }


    public void RestorePurchases()
    {
        Debug.Log("Restore Purchase begin");
        purchases.RestoreTransactions((purchaserInfo, error) =>
        {
            if (error != null)
            {
                LogError(error);
            }
            else
            {
                GetPlayerPurchases();
            }
        });
    }



    private void LogError(Purchases.Error error)
    {
        Debug.Log("Subtester: " + JsonUtility.ToJson(error));
    }


    private string GetTransactionIdFromReceipt(string identifier)
    {

        string transactionID = string.Empty;

        //        try
        //        {
        //#if !UNITY_IOS
        //            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        //            // Get a reference to IAppleConfiguration during IAP initialization.
        //            var appleConfig = builder.Configure<IAppleConfiguration>();
        //            var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
        //            AppleReceipt receipt = new AppleValidator(AppleTangle.Data()).Validate(receiptData);

        //            foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts)
        //            {
        //                if(productReceipt.productID == identifier)
        //                {
        //                    transactionID = productReceipt.originalTransactionIdentifier;
        //                }
        //            }
        //#endif

        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.Log(ex.Message);
        //            //Crashlytics.LogException(ex);
        //        }

        return transactionID;
    }

        #region Override MEthods
    public override void PurchaserInfoReceived(Purchases.PurchaserInfo purchaserInfo)
    {
        SetPreviousPurchases(purchaserInfo);
    }

    private void logError(Purchases.Error error)
    {
        Debug.Log("Subtester: " + JsonUtility.ToJson(error));
    }

        #endregion


#endif
    }

}