using GiftsManager.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftsManager.Models.Dal
{
    public class DalGift : IDalGift
    {
        private DataBaseContext dbContext;

        public DalGift()
        {
            dbContext = new DataBaseContext();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public bool AddGiftToWishlist(string newGift, string userEmail, string eventName)
        {
            User user = dbContext.Users.Include("WishList").Where(x => x.Email == userEmail).FirstOrDefault();
            Event currentEvent = dbContext.Events.Where(x => x.Name == eventName).FirstOrDefault();

            if (user != null)
            {
                Gift gift = new Gift
                {
                    Name = newGift,
                    Status = Gift.StatusOption.Open,
                    User = user,
                    Event = currentEvent
                };
                
                dbContext.Gifts.Add(gift);
                user.WishList.Add(gift);
                dbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public bool IsGiftExists(string name, string eventName, string groupName, User user)
        {
            if (user != null && dbContext.Gifts.Include("User").Include("Event.Group").Where(x => x.Event.Name == eventName
                && x.Event.Group.Name == groupName
                && x.User.Id == user.Id
                && x.Name.Equals(name)).FirstOrDefault() != null)
            {
                return true;
            }
                
            return false;
        }

        public bool ReserveGift(int id, int userId, string buyerEmail)
        {
            try
            {
                User buyer = dbContext.Users.Include("ReservedGifts").Where(x => x.Email.Equals(buyerEmail)).FirstOrDefault();
                User user = dbContext.Users.Include("WishList.Event").Where(x => x.Id == userId).FirstOrDefault();
                Gift gift = user != null ? user.WishList.Where(x => x.Id == id).FirstOrDefault(): null;

                if (gift != null)
                {
                    gift.Status = Gift.StatusOption.Reserved;
                    gift.Buyer = buyer;
                    buyer.ReservedGifts.Add(gift);

                    dbContext.SaveChanges();
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
                User buyer = dbContext.Users.Include("BoughtGifts").Where(x => x.Email.Equals(buyerEmail)).FirstOrDefault();
                User user = dbContext.Users.Include("WishList.Event").Where(x => x.Id == userId).FirstOrDefault();
                Gift gift = user != null ? user.WishList.Where(x => x.Id == id).FirstOrDefault() : null;

                if (gift != null)
                {
                    gift.Status = Gift.StatusOption.Bought;
                    gift.Buyer = buyer;
                    gift.Price = price;
                    gift.IsBought = true;
                    buyer.BoughtGifts.Add(gift);

                    dbContext.SaveChanges();
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
                dbContext.Gifts.Where(x => x.User.Email == userEmail && x.Name == giftName && x.Event.Name == eventName)
                    .FirstOrDefault();

            User user = dbContext.Users.Include("WishList").Where(x => x.Email == userEmail).FirstOrDefault();

            if (gift != null && user?.WishList != null)
            {
                user.WishList.Remove(gift);

                if (!gift.IsBought)
                    dbContext.Gifts.Remove(gift);

                dbContext.SaveChanges();
            }
        }

        public void RemoveGiftFromReservation(string userEmail, string eventName, string groupName, string giftName)
        {
            Gift gift =
                dbContext.Gifts.Include("User").Include("Event").Include("Buyer").Where(x => x.Name == giftName && x.Event.Name == eventName)
                    .FirstOrDefault();

            User user = dbContext.Users.Include("ReservedGifts").Where(x => x.Email == userEmail).FirstOrDefault();

            if (gift != null && user?.ReservedGifts != null)
            {
                user.ReservedGifts.Remove(gift);
                gift.Status = Gift.StatusOption.Open;
                gift.Buyer = null;
                dbContext.SaveChanges();
            }
        }
    }
}