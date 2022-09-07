namespace PassionPunch.Dealer
{
    public interface IPurchasers
    {
        void Initialize();
        bool IsInitialized();
        void SetPreviousPurchases();
        void PurchaseProduct(string productID);
        void RestorePurchases();
        bool IsProductPurchased(string product);
        string GetItemPrice(string product);
    }
}