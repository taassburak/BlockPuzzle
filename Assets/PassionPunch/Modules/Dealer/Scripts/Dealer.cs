//using Firebase.Crashlytics;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

namespace PassionPunch.Dealer
{

    public class Dealer : MonoBehaviour
    {
        public static Dealer Instance;

        public DealerSettings settings;

        private PurchaseData purchaseData;


        #region EVENTS
        public delegate void ProductPurchasedCallbackDelegate(string purchasedProduct);
        public event ProductPurchasedCallbackDelegate onProductPurchasedCallBack;

        public delegate void ProductPurchasedCallbackWithAnalyticsDelegate(string currentInappProduct, float price, string currencyCode, string receipt);
        public event ProductPurchasedCallbackWithAnalyticsDelegate onProductPurchasedCallBackWithAnalytics;

        public delegate void PreviouslyPurchasedProductCallbackDelegate(string purchasedProduct);
        public event PreviouslyPurchasedProductCallbackDelegate onPreviouslyPurchasedProductCallback;
        #endregion
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Initialize()
        {
#if PP_REVENUECAT
        if (!Purchases.Instance)
        {
            gameObject.AddComponent<Purchases>();
        }
        SetRevenueCat();
        Purchases.Instance.Init();
#elif PP_UNITYIAP
            if (!UnityIAPController.Instance)
            {
                gameObject.AddComponent<UnityIAPController>();
            }
            UnityIAPController.Instance.Initialize();
#endif
        }

        private void SetRevenueCat()
        {
#if PP_REVENUECAT
        Purchases.Instance.productIdentifiers = new string[settings.inappProducts.Count];

        for (int i = 0; i < settings.inappProducts.Count; i++)
        {
            Purchases.Instance.productIdentifiers[i] = settings.inappProducts[i].GetProductID();
        }

        Purchases.Instance.revenueCatAPIKey = settings.revenueCatAPIKey;
#endif
        }

        public void PurchaseProduct(string currentInappProduct)
        {
            for (int i = 0; i < settings.inappProducts.Count; i++)
            {
                if (currentInappProduct == settings.inappProducts[i].inappProduct)
                {
#if PP_REVENUECAT
                Purchases.Instance.revenueCatController.PurchaseProduct(settings.inappProducts[i].GetProductID());
#elif PP_UNITYIAP
                    UnityIAPController.Instance.PurchaseProduct(settings.inappProducts[i].GetProductID());
#endif
                }
            }
        }

        //Prodcuts Pruchased Before
        public void PreviouslyPurchasedProductCallback(string currentInappProduct)
        {
            if (onPreviouslyPurchasedProductCallback != null)
            {
                onPreviouslyPurchasedProductCallback(currentInappProduct);
            }
        }

        public bool IsInitialized()
        {
#if PP_REVENUECAT
       return Purchases.Instance.revenueCatController.IsInitialized();
#elif PP_UNITYIAP
            return UnityIAPController.Instance.IsInitialized();
#endif
            return false;
        }

        //For standart package  processing
        public void ProductPurchasedCallback(string currentInappProduct)
        {
            if (onProductPurchasedCallBack != null)
            {
                onProductPurchasedCallBack(currentInappProduct);
            }
        }

        //For Purchased Product Processing With Analytics
        //Purchases.Package selectedProduct.Product.price   selectedProduct.Product.currencyCode
        public void ProductPurchasedCallback(string currentInappProduct, float price, string currencyCode, string receipt)
        {
            TransactionReceiptData transactionID = JsonUtility.FromJson<TransactionReceiptData>(receipt);
#if UNITY_IOS
            receipt = transactionID.TransactionID;
#elif UNITY_ANDROID
#endif

            if (onProductPurchasedCallBackWithAnalytics != null)
            {
                onProductPurchasedCallBackWithAnalytics(currentInappProduct, price, currencyCode, receipt);
            }
        }

        public void PrepareAndSendPurchaseDataForAdmost(string[] currTags, float localPrice, string isoCode, string receipt, string identifier)
        {
            purchaseData = new PurchaseData
            {
                LocalizedPrice = localPrice,
                IsoCurrencyCode = isoCode,
                Transaction = receipt,
                ProductIdentifierKey = identifier
            };
            if (currTags.Length > 0)
            {
                Debug.Log("Sending purchase analitycs data to Admost: Tag:" + currTags[0] + purchaseData.ProductIdentifierKey);
            }
            SendSubscriptionInfoToAdmost(purchaseData, currTags);
        }

        private void SendSubscriptionInfoToAdmost(PurchaseData admostData, string[] tags)
        {
#if PP_ADMOST
        try
        {
            if (AMR.AMRSDK.initialized())
            {
                if (admostData.Transaction != string.Empty)
                {
#if UNITY_IOS
                    AMR.AMRSDK.trackIAPForIOS(admostData.Transaction, (decimal)admostData.LocalizedPrice, admostData.IsoCurrencyCode, tags);
#elif UNITY_ANDROID
                    AMR.AMRSDK.trackIAPForAndroid(admostData.Transaction, (decimal)admostData.LocalizedPrice, admostData.IsoCurrencyCode, tags);
#endif
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            //Crashlytics.LogException(ex);
        }
#endif
        }
        public bool IsProductPurchased(string currentInappProduct)
        {
#if PP_REVENUECAT
        return Purchases.Instance.revenueCatController.IsProductPurchased(currentInappProduct);
#elif PP_UNITYIAP
            return UnityIAPController.Instance.IsProductPurchased(currentInappProduct);
#endif
            return false;
        }

        public void RestorePurchases()
        {
#if PP_REVENUECAT
        Purchases.Instance.revenueCatController.RestorePurchases();
#elif PP_UNITYIAP
            UnityIAPController.Instance.RestorePurchases();
#endif
        }

        public string GetItemPrice(string currentInappProduct)
        {
#if PP_REVENUECAT
        return Purchases.Instance.revenueCatController.GetItemPrice(currentInappProduct);
#elif PP_UNITYIAP
            return UnityIAPController.Instance.GetItemPrice(currentInappProduct);
#endif
            return string.Empty;
        }

        public string GetInappProductFromIdentifier(string productID)
        {
            for (int i = 0; i < settings.inappProducts.Count; i++)
            {
                if (productID.Equals(settings.inappProducts[i].GetProductID()))
                {
                    return settings.inappProducts[i].inappProduct;
                }
            }
            return string.Empty;
        }
        public string GetProductIdentifierFromInappProductType(string currInappProduct)
        {
            for (int i = 0; i < settings.inappProducts.Count; i++)
            {
                if (currInappProduct.Equals(settings.inappProducts[i].inappProduct))
                {
                    return settings.inappProducts[i].GetProductID();
                }
            }
            return null;
        }


        private struct PurchaseData
        {
            public string Transaction;
            public float LocalizedPrice;
            public string IsoCurrencyCode;
            public string ProductIdentifierKey;
        }
        private struct TransactionReceiptData
        {
            public string TransactionID;
        }

    }
}
