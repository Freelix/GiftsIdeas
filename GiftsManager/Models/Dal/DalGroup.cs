using GiftsManager.Models.Context;
using System.Collections.Generic;
using System.Linq;
using GiftsManager.Models.Dal.IDal;

namespace GiftsManager.Models.Dal
{
    public class DalGroup : IDalGroup
    {
        private readonly DataBaseContext _dbContext;

        public DalGroup()
        {
            _dbContext = new DataBaseContext();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void AddGroup(string name, User user)
        {
            User userToModify = _dbContext.Users.FirstOrDefault(x => x.Id == user.Id);

            if (userToModify != null)
            {
                Group group = new Group
                {
                    GroupAdmin = userToModify.Email,
                    Name = name,
                    Users = new List<User>()
                };

                if (userToModify.Groups == null)
                    userToModify.Groups = new List<Group>();

                userToModify.Groups.Add(@group);
                @group.Users.Add(userToModify);

                _dbContext.Groups.Add(@group);
                _dbContext.SaveChanges();
            }
        }

        public void AddUserToGroup(string group, User user)
        {
            Group g = GetGroupByName(group);
            User userToModify = _dbContext.Users.FirstOrDefault(x => x.Id == user.Id);

            if (g != null && userToModify != null)
            {
                if (userToModify.Groups == null)
                    userToModify.Groups = new List<Group>();

                userToModify.Groups.Add(g);
                g.Users.Add(userToModify);
                _dbContext.SaveChanges();
            }
        }

        public List<User> GetAllUsers(string group)
        {
            Group g = GetGroupByName(group);

            return g?.Users.ToList();
        }

        public List<User> GetAllUsersButMe(Group group, User user)
        {
            return @group?.Users.Where(x => x.Id != user.Id).ToList();
        }

        public void RemoveUserFromGroup(string groupName, string userEmail)
        {
            var group = _dbContext.Groups.Include("Users").FirstOrDefault(x => x.Name.Equals(groupName));
            var user = _dbContext.Users.FirstOrDefault(x => x.Email.Equals(userEmail));

            if (group != null && user != null)
            {
                group.Users.Remove(user);
                _dbContext.SaveChanges();
            }
        }

        public bool IsGroupExist(string name)
        {
            if (_dbContext.Groups.FirstOrDefault(x => x.Name == name) != null)
                return true;
            return false;
        }

        public Group GetGroupByName(string name)
        {
            // Eager loading to load automatically the GroupAdmin object
            return _dbContext.Groups.Include("Events").Include("Users.WishList.Event").FirstOrDefault(x => x.Name.Equals(name));
        }

        public void DeleteGroup(string groupName, string userEmail)
        {
            Group group = _dbContext.Groups.FirstOrDefault(x => x.Name == groupName && x.GroupAdmin == userEmail);

            if (group != null)
            {
                _dbContext.Groups.Remove(group);
                _dbContext.SaveChanges();
            }
            
        }

        public bool IsAdmin(string userEmail, string groupName)
        {
            var exist = _dbContext.Groups.FirstOrDefault(x => x.Name == groupName && x.GroupAdmin == userEmail);

            return exist != null;
        }
    }
}