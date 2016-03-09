using System;
using System.Collections.Generic;

namespace GiftsManager.Models.Dal.IDal
{
    public interface IDalGroup : IDisposable
    {
        void AddGroup(string name, User user);
        bool IsGroupExist(string name);
        Group GetGroupByName(string name);
        void AddUserToGroup(string group, User user);
        List<User> GetAllUsers(string group);
        List<User> GetAllUsersButMe(Group group, User user);
        void RemoveUserFromGroup(string groupName, string userEmail);
        void DeleteGroup(string groupName, string userEmail);
        bool IsAdmin(string userEmail, string groupName);
    }
}
