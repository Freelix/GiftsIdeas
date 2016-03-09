using GiftsManager.ViewModels.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GiftsManager.Models;
using GiftsManager.Models.Dal.IDal;

namespace GiftsManager.Helper
{
    public class EventHelper
    {
        public static EventDropDownViewModel GetEvents(IDalGroup dalGroup, string groupName, bool showAddEvent)
        {
            EventDropDownViewModel vm = new EventDropDownViewModel
            {
                Group = dalGroup.GetGroupByName(groupName),
                IsAdmin = showAddEvent
            };

            if (vm.Group?.Events != null)
                vm.Events = new SelectList(vm.Group.Events, "Id", "Name");

            return vm;
        }

        public static List<Gift> GetWishListByEvent(List<Gift> wishList, string eventName)
        {
            return wishList.Where(x => x.Event.Name.Equals(eventName)).ToList();
        }
    }
}