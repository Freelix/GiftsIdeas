using System;

namespace GiftsManager.Models.Dal.IDal
{
    public interface IDalGift : IDisposable
    {
        bool AddGiftToWishlist(string newGift, string userEmail, string eventName);
        bool IsGiftExists(string name, string eventName, string groupName, User user);
        bool ReserveGift(int id, int userId, string buyerEmail);
        bool BuyGift(int id, int userId, double price, string buyerEmail);
        void RemoveGiftFromWishlist(string userEmail, string eventName, string giftName);
        void RemoveGiftFromReservation(string userEmail, string eventName, string groupName, string giftName);
    }
}
