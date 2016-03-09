using GiftsManager.Models.Context;
using System;
using System.Linq;
using GiftsManager.Models.Dal.IDal;

namespace GiftsManager.Models.Dal
{
    public class DalGift : IDalGift
    {
        private readonly DataBaseContext _dbContext;

        public DalGift()
        {
            _dbContext = new DataBaseContext();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public bool AddGiftToWishlist(string newGift, string userEmail, string eventName)
        {
            User user = _dbContext.Users.Include("WishList").FirstOrDefault(x => x.Email == userEmail);
            Event currentEvent = _dbContext.Events.FirstOrDefault(x => x.Name == eventName);

            if (user != null)
            {
                Gift gift = new Gift
                {
                    Name = newGift,
                    Status = Gift.StatusOption.Open,
                    User = user,
                    Event = currentEvent
                };
                
                _dbContext.Gifts.Add(gift);
                user.WishList.Add(gift);
                _dbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public bool IsGiftExists(string name, string eventName, string groupName, User user)
        {
            if (user != null && _dbContext.Gifts.Include("User").Include("Event.Group").FirstOrDefault(x => x.Event.Name == eventName
                && x.Event.Group.Name == groupName
                && x.User.Id == user.Id
                && x.Name.Equals(name)) != null)
            {
                return true;
            }
                
            return false;
        }

        public bool ReserveGift(int id, int userId, string buyerEmail)
        {
            try
            {
                User buyer = _dbContext.Users.Include("ReservedGifts").FirstOrDefault(x => x.Email.Equals(buyerEmail));
                User user = _dbContext.Users.Include("WishList.Event").FirstOrDefault(x => x.Id == userId);
                Gift gift = user?.WishList.FirstOrDefault(x => x.Id == id);

                if (gift != null)
                {
                    gift.Status = Gift.StatusOption.Reserved;
                    gift.Buyer = buyer;
                    buyer.ReservedGifts.Add(gift);

                    _dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool BuyGift(int id, int userId, double price, string buyerEmail)
        {
            try
            {
                User buyer = _dbContext.Users.Include("BoughtGifts").FirstOrDefault(x => x.Email.Equals(buyerEmail));
                User user = _dbContext.Users.Include("WishList.Event").FirstOrDefault(x => x.Id == userId);
                Gift gift = user?.WishList.FirstOrDefault(x => x.Id == id);

                if (gift != null)
                {
                    gift.Status = Gift.StatusOption.Bought;
                    gift.Buyer = buyer;
                    gift.Price = price;
                    gift.IsBought = true;
                    buyer?.BoughtGifts.Add(gift);

                    _dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void RemoveGiftFromWishlist(string userEmail, string eventName, string giftName)
        {
            Gift gift =
                _dbContext.Gifts
                    .FirstOrDefault(x => x.User.Email == userEmail && x.Name == giftName && x.Event.Name == eventName);

            User user = _dbContext.Users.Include("WishList").FirstOrDefault(x => x.Email == userEmail);

            if (gift != null && user?.WishList != null)
            {
                user.WishList.Remove(gift);

                if (!gift.IsBought)
                    _dbContext.Gifts.Remove(gift);

                _dbContext.SaveChanges();
            }
        }

        public void RemoveGiftFromReservation(string userEmail, string eventName, string groupName, string giftName)
        {
            Gift gift =
                _dbContext.Gifts.Include("User").Include("Event").Include("Buyer")
                    .FirstOrDefault(x => x.Name == giftName && x.Event.Name == eventName);

            User user = _dbContext.Users.Include("ReservedGifts").FirstOrDefault(x => x.Email == userEmail);

            if (gift != null && user?.ReservedGifts != null)
            {
                user.ReservedGifts.Remove(gift);
                gift.Status = Gift.StatusOption.Open;
                gift.Buyer = null;

                _dbContext.SaveChanges();
            }
        }
    }
}