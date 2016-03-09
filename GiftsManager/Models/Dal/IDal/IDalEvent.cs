using System;

namespace GiftsManager.Models.Dal.IDal
{
    public interface IDalEvent : IDisposable
    {
        Event GetEventByName(string name);
        void AddEvent(string name, string groupName);
        bool IsEventExist(string name, string groupName);
        void DeleteEvent(string eventName, string groupName, string userEmail);
    }
}