using GiftsManager.Models.Context;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GiftsManager.Models.Dal.IDal;

namespace GiftsManager.Models.Dal
{
    public class DalUser : IDalUser
    {
        private readonly DataBaseContext _dbContext;

        public DalUser()
        {
            _dbContext = new DataBaseContext();
        }

        #region Public Functions

        public int AddUser(User user)
        {
            string password = EncodeMd5(user.Password);
            user.Password = password;

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user.Id;
        }

        public User Authenticate(string email, string password)
        {
            string pass = EncodeMd5(password);
            return _dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == pass);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public User GetUserById(int id)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByEmail(string email)
        {
            // Use Eager Loading
            return _dbContext.Users
                .Include("Groups")
                .Include("WishList.Event")
                .Include("ReservedGifts.Event")
                .Include("ReservedGifts.User")
                .Include("BoughtGifts.Event")
                .Include("BoughtGifts.User")
                .FirstOrDefault(u => u.Email.Equals(email));
        }

        public bool IsUserExist(string email)
        {
            if (_dbContext.Users.FirstOrDefault(u => u.Email.Equals(email)) != null)
                return true;
            return false;
        }

        #endregion

        #region Private Functions

        private static string EncodeMd5(string pass)
        {
            // TODO: Do a better encryption
            string password = "GiftsManager" + pass + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(password)));
        }

        #endregion
    }
}