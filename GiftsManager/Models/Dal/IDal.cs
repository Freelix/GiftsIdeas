using System;
using System.Collections.Generic;

namespace GiftsManager.Models.Dal
{
    public interface IDalUser : IDisposable
    {
        int AddUser(User user);
        User Authenticate(string email, string password);
        User GetUserById(int id);
        User GetUserByEmail(string email);
        bool IsUserExist(string email);
    }

    public interface IDalGroup : IDisposable
    {
        int AddGroup(string name, User user);
        bool IsGroupExist(string name);
        Group GetGroupByName(string name);
        void AddUserToGroup(string group, User user);
        List<User> GetAllUsers(string group);
        List<User> GetAllUsersButMe(Group group, User user);
        void RemoveUserFromGroup(string groupName, string userEmail);
        void DeleteGroup(string groupName, string userEmail);
        bool IsAdmin(string userEmail, string groupName);
    }

    public interface IDalGift : IDisposable
    {
        bool AddGiftToWishlist(string newGift, string userEmail, string eventName);
        bool IsGiftExists(string name, string eventName, string groupName, User user);
        bool ReserveGift(int id, int userId, string buyerEmail);
        bool BuyGift(int id, int userId, double price, string buyerEmail);
        void RemoveGiftFromWishlist(string userEmail, string eventName, string giftName);
        void RemoveGiftFromReservation(string userEmail, string eventName, string groupName, string giftName);
    }

    public interface IDalEvent : IDisposable
    {
        Event GetEventByName(string name);
        void AddEvent(string name, string groupName);
        bool IsEventExist(string name, string groupName);
        void DeleteEvent(string eventName, string groupName, string userEmail);
    }
}