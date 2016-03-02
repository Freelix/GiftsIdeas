using GiftsManager.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GiftsManager.Models.Dal
{
    public class DalUser : IDalUser
    {
        private DataBaseContext dbContext;

        public DalUser()
        {
            dbContext = new DataBaseContext();
        }

        #region Public Functions

        public int AddUser(User user)
        {
            string password = EncodeMD5(user.Password);
            user.Password = password;

            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            return user.Id;
        }

        public User Authenticate(string email, string password)
        {
            string pass = EncodeMD5(password);
            return dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == pass);
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public User GetUserById(int id)
        {
            return dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetUserByEmail(string email)
        {
            // Use Eager Loading
            return dbContext.Users
                .Include("WishList.Event")
                .Include("ReservedGifts.Event")
                .Include("ReservedGifts.User")
                .Include("BoughtGifts.Event")
                .Include("BoughtGifts.User")
                .FirstOrDefault(u => u.Email.Equals(email));
        }

        public bool IsUserExist(string email)
        {
            if (dbContext.Users.Where(u => u.Email.Equals(email)).FirstOrDefault() != null)
                return true;
            return false;
        }

        #endregion

        #region Private Functions

        private string EncodeMD5(string pass)
        {
            // TODO: Do a better encryption
            string password = "GiftsManager" + pass + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(password)));
        }

        #endregion
    }
}