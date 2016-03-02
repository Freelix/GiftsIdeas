using GiftsManager.Models.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace GiftsManager.Models.Dal
{
    public class DalEvent : IDalEvent
    {
        private DataBaseContext dbContext;

        public DalEvent()
        {
            dbContext = new DataBaseContext();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public void AddEvent(string name, string groupName)
        {
            Group group = dbContext.Groups.Where(x => x.Name.Equals(groupName)).FirstOrDefault();

            Event newEvent = new Event
            {
                Name = name,
                Group = group
            };

            if (group.Events == null)
                group.Events = new List<Event>();

            group.Events.Add(newEvent);

            dbContext.Events.Add(newEvent);
            dbContext.SaveChanges();
        }

        public Event GetEventByName(string name)
        {
            return dbContext.Events.Where(x => x.Name.Equals(name)).FirstOrDefault();
        }

        public bool IsEventExist(string name, string groupName)
        {
            if (dbContext.Events.Where(x => x.Group.Name == groupName && x.Name == name).FirstOrDefault() != null)
                return true;
            return false;
        }

        public void DeleteEvent(string eventName, string groupName, string userEmail)
        {
            Event currentEvent =
                dbContext.Events.Where(
                    x => x.Group.Name == groupName && x.Name == eventName && x.Group.GroupAdmin == userEmail)
                    .FirstOrDefault();

            if (currentEvent != null)
            {
                dbContext.Events.Remove(currentEvent);
                dbContext.SaveChanges();
            }
        }
    }
}