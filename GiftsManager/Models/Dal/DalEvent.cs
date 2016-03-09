using GiftsManager.Models.Context;
using System.Collections.Generic;
using System.Linq;
using GiftsManager.Models.Dal.IDal;

namespace GiftsManager.Models.Dal
{
    public class DalEvent : IDalEvent
    {
        private readonly DataBaseContext _dbContext;

        public DalEvent()
        {
            _dbContext = new DataBaseContext();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public void AddEvent(string name, string groupName)
        {
            Group group = _dbContext.Groups.FirstOrDefault(x => x.Name.Equals(groupName));

            Event newEvent = new Event
            {
                Name = name,
                Group = group
            };

            if (group != null && group.Events == null)
            {
                group.Events = new List<Event>();
                group.Events.Add(newEvent);
            }

            _dbContext.Events.Add(newEvent);
            _dbContext.SaveChanges();
        }

        public Event GetEventByName(string name)
        {
            return _dbContext.Events.FirstOrDefault(x => x.Name.Equals(name));
        }

        public bool IsEventExist(string name, string groupName)
        {
            return _dbContext.Events.FirstOrDefault(x => x.Group.Name == groupName && x.Name == name) != null;
        }

        public void DeleteEvent(string eventName, string groupName, string userEmail)
        {
            Event currentEvent =
                _dbContext.Events
                    .FirstOrDefault(x => x.Group.Name == groupName && x.Name == eventName && x.Group.GroupAdmin == userEmail);

            if (currentEvent != null)
            {
                _dbContext.Events.Remove(currentEvent);
                _dbContext.SaveChanges();
            }
        }
    }
}