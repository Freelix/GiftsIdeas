using System;

namespace GiftsManager.Models.Dal.IDal
{
    public interface IDalUser : IDisposable
    {
        int AddUser(User user);
        User Authenticate(string email, string password);
        User GetUserById(int id);
        User GetUserByEmail(string email);
        bool IsUserExist(string email);
    }
}
