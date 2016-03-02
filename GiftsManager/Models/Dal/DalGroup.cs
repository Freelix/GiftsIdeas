using GiftsManager.Models.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GiftsManager.Models.Dal
{
    public class DalGroup : IDalGroup
    {
        private DataBaseContext dbContext;

        public DalGroup()
        {
            dbContext = new DataBaseContext();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public int AddGroup(string name, User user)
        {
            User userToModify = dbContext.Users.Where(x => x.Id == user.Id).FirstOrDefault();

            Group group = new Group
            {
                GroupAdmin = userToModify.Email,
                Name = name,
                Users = new List<User>()
            };

            if (userToModify.Groups == null)
                userToModify.Groups = new List<Group>();

            userToModify.Groups.Add(group);
            group.Users.Add(userToModify);

            dbContext.Groups.Add(group);
            dbContext.SaveChanges();

            return group.Id;
        }

        public void AddUserToGroup(string group, User user)
        {
            Group g = GetGroupByName(group);
            User userToModify = dbContext.Users.Where(x => x.Id == user.Id).FirstOrDefault();

            if (g != null && userToModify != null)
            {
                if (userToModify.Groups == null)
                    userToModify.Groups = new List<Group>();

                userToModify.Groups.Add(g);
                g.Users.Add(userToModify);
                dbContext.SaveChanges();
            }
        }

        public List<User> GetAllUsers(string group)
        {
            Group g = GetGroupByName(group);

            if (g != null)
                return g.Users.ToList();
            return null;
        }

        public List<User> GetAllUsersButMe(Group group, User user)
        {
            if (group != null)
                return group.Users.Where(x => x.Id != user.Id).ToList();
            return null;
        }

        public void RemoveUserFromGroup(string groupName, string userEmail)
        {
            var group = dbContext.Groups.Where(x => x.Name.Equals(groupName)).FirstOrDefault();
            var user = dbContext.Users.Where(x => x.Email.Equals(userEmail)).FirstOrDefault();

            if (group != null && user != null)
            {
                group.Users.Remove(user);
                dbContext.SaveChanges();
            }
        }

        public bool IsGroupExist(string name)
        {
            if (dbContext.Groups.Where(x => x.Name == name).FirstOrDefault() != null)
                return true;
            return false;
        }

        public Group GetGroupByName(string name)
        {
            // Eager loading to load automatically the GroupAdmin object
            return dbContext.Groups.Include("Users.WishList.Event").Where(x => x.Name.Equals(name)).FirstOrDefault();
        }

        public void DeleteGroup(string groupName, string userEmail)
        {
            Group group = dbContext.Groups.Where(x => x.Name == groupName && x.GroupAdmin == userEmail).FirstOrDefault();

            if (group != null)
            {
                dbContext.Groups.Remove(group);
                dbContext.SaveChanges();
            }
            
        }

        public bool IsAdmin(string userEmail, string groupName)
        {
            var exist = dbContext.Groups.Where(x => x.Name == groupName && x.GroupAdmin == userEmail).FirstOrDefault();

            return exist != null;
        }
    }
}