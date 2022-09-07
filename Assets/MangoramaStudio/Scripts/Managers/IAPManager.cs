using PassionPunch.Dealer;
using System.Collections;
using UnityEngine;

namespace MangoramaStudio.Scripts.Managers
{
    public class IAPManager : CustomBehaviour
    {
        public override void Initialize(GameManager gameManager)
        {
            base.Initialize(gameManager);

            InitializeDealer();
            SetListeners();
        }

        private void InitializeDealer()
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
            //switch (purchasedProduct)
            //{
            //    case "pack1":
            //        break;

            //    case "pack2":
            //        break;

            //    case "subscription":
            //        Debug.Log("Subscription is active");
            //        //Check if not subscribed before
            //        break;
            //}
        }

        private void OnProductPurchasedCallBackWithAnalytics(string purchasedProduct, float price, string currencyCode, string receipt)
        {
            switch (purchasedProduct)
            {
                case "pack1":
                    Dealer.Instance.PrepareAndSendPurchaseDataForAdmost(new string[] { "iap_pack_1" }, price, currencyCode, receipt, "Adjust Event Code");
#if PP_SHERLOCK
                    GameManager.AnalyticsManager.TrackIAPEvent("pack1", price, receipt, currencyCode);
#endif
                    break;
            }
#if PP_SHERLOCK
        GameManager.AnalyticsManager.TrackIAPEvent("iap_total", price, receipt, currencyCode);
#endif
        }


        public void PurchaseProduct(string productName) // SingleAppItem name
        {
            Dealer.Instance.PurchaseProduct(productName);
        }
        public string GetItemPrice(string productName) // SingleAppItem name
        {
            return Dealer.Instance.GetItemPrice("pack2");
        }
        public bool IsProductPurchased(string productName) // SingleAppItem name
        {
            return Dealer.Instance.IsProductPurchased("pack1");
        }
        public void RestorePurchases()
        {
            Dealer.Instance.RestorePurchases();
        }
    }
}