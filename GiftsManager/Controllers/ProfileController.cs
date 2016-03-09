using System;
using GiftsManager.Models.Dal;
using GiftsManager.ViewModels.Shared;
using GiftsManager.ViewModels.Profile;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using GiftsManager.Helper;
using Microsoft.SqlServer.Server;

namespace GiftsManager.Controllers
{
    [Authorize]
    public class ProfileController : BaseController
    {
        private IDalUser dalUser;
        private IDalGroup dalGroup;
        private IDalGift dalGift;
        private IDalEvent dalEvent;

        public ProfileController() : this(new DalUser(), new DalGroup(), new DalGift(), new DalEvent())
        {

        }

        private ProfileController(IDalUser dalUser, IDalGroup dalGroup, IDalGift dalGift, IDalEvent dalEvent)
        {
            this.dalUser = dalUser;
            this.dalGroup = dalGroup;
            this.dalGift = dalGift;
            this.dalEvent = dalEvent;
        }
        
        public ActionResult UserProfile()
        {
            return View(GroupHelper.GetGroups(dalUser, HttpContext.User.Identity.Name, false));
        }

        #region AjaxPosts

        [HttpPost]
        public ActionResult LoadEvents(string groupName)
        {
            var isAdmin = dalGroup.IsAdmin(HttpContext.User.Identity.Name, groupName);
            return PartialView("_DropDownEvent", EventHelper.GetEvents(dalGroup, groupName, isAdmin));
        }

        [HttpPost]
        public ActionResult CreateEvent(string eventName, string groupName)
        {
            if (eventName != null && !dalEvent.IsEventExist(eventName, groupName) && dalGroup.IsGroupExist(groupName))
            {
                dalEvent.AddEvent(eventName, groupName);
                var isAdmin = dalGroup.IsAdmin(HttpContext.User.Identity.Name, groupName);
                return PartialView("_DropDownEvent", EventHelper.GetEvents(dalGroup, groupName, isAdmin));
            }

            return Content("");
        }

        [HttpPost]
        public ActionResult IsAdmin(string groupName)
        {
            var isAdmin = dalGroup.IsAdmin(HttpContext.User.Identity.Name, groupName);
            return Content(isAdmin.ToString());
        }

        [HttpPost]
        public bool IsGroupExist(string groupName)
        {
            return dalGroup.IsGroupExist(groupName);
        }

        [HttpPost]
        public bool IsEventExist(string eventName, string groupName)
        {
            return dalEvent.IsEventExist(eventName, groupName);
        }

        [HttpPost]
        public bool IsGiftExist(string name, string eventName, string groupName)
        {
            return dalGift.IsGiftExists(name, eventName, groupName, dalUser.GetUserByEmail(HttpContext.User.Identity.Name));
        }

        [HttpPost]
        public bool IsUserExist(string userEmail)
        {
            return dalUser.IsUserExist(userEmail);
        }

        [HttpPost]
        public ActionResult CreateGroup(string groupName)
        {
            if (groupName != null && !dalGroup.IsGroupExist(groupName))
            {
                dalGroup.AddGroup(groupName, dalUser.GetUserByEmail(HttpContext.User.Identity.Name));
                var isAdmin = dalGroup.IsAdmin(HttpContext.User.Identity.Name, groupName);
                return PartialView("_DropDownGroup", GroupHelper.GetGroups(dalUser, HttpContext.User.Identity.Name, isAdmin));
            }

            return Content("");
        }
        
        [HttpPost]
        public ActionResult AddUserToGroup(string groupName, string email)
        {
            if (groupName != null && email != null && dalUser.IsUserExist(email))
            {
                var user = dalUser.GetUserByEmail(email);
                dalGroup.AddUserToGroup(groupName, user);

                ActualGroupViewModel vm = new ActualGroupViewModel()
                {
                    ActualGroup = dalGroup.GetGroupByName(groupName),
                    User = dalUser.GetUserByEmail(HttpContext.User.Identity.Name),
                    IsAdmin = true
                };

                return PartialView("_UsersInGroup", vm);
            }

            return Content("");
        }
        
        [HttpPost]
        public ActionResult ChangeGroup(string groupName)
        {
            ActualGroupViewModel vm = new ActualGroupViewModel()
            {
                ActualGroup = dalGroup.GetGroupByName(groupName),
                User = dalUser.GetUserByEmail(HttpContext.User.Identity.Name),
                IsAdmin = dalGroup.IsAdmin(HttpContext.User.Identity.Name, groupName)
            };

            return PartialView("_UsersInGroup", vm);
        }
        
        [HttpPost]
        public ActionResult LoadGiftsSection(string eventName)
        {
            return PartialView("_Gifts", GetGiftsDetails(eventName));
        }
        
        [HttpPost]
        public ActionResult AddGiftInWishlist(string newGift, string eventName)
        {
            if (newGift.Trim() != string.Empty)
            {
                dalGift.AddGiftToWishlist(newGift, HttpContext.User.Identity.Name, eventName);
                return PartialView("_Gifts", GetGiftsDetails(eventName));
            }

            return Content("");
        }

        [HttpPost]
        public ActionResult RemoveUserFromGroup(string groupName, string email)
        {
            email = email.Split('(', ')')[1];

            if (!email.Equals(HttpContext.User.Identity.Name)) { }
                dalGroup.RemoveUserFromGroup(groupName, email);

            ActualGroupViewModel vm = new ActualGroupViewModel()
            {
                ActualGroup = dalGroup.GetGroupByName(groupName),
                User = dalUser.GetUserByEmail(HttpContext.User.Identity.Name),
                IsAdmin = true
            };

            return PartialView("_UsersInGroup", vm);
        }

        [HttpPost]
        public ActionResult RemoveGiftFromWishlist(string giftName, string eventName)
        {
            if (!string.IsNullOrEmpty(giftName) && !string.IsNullOrEmpty(eventName))
            {
                dalGift.RemoveGiftFromWishlist(HttpContext.User.Identity.Name, eventName, giftName);
                return PartialView("_Gifts", GetGiftsDetails(eventName));
            }

            return Content("");
        }

        [HttpPost]
        public ActionResult RemoveGiftFromReservation(string giftName, string eventName, string groupName)
        {
            if (!string.IsNullOrEmpty(giftName) && !string.IsNullOrEmpty(eventName) && !string.IsNullOrEmpty(groupName))
            {
                dalGift.RemoveGiftFromReservation(HttpContext.User.Identity.Name, eventName, groupName, giftName);
                return PartialView("_Gifts", GetGiftsDetails(eventName));
            }

            return Content("");
        }

        [HttpPost]
        public ActionResult BuyGiftFromReservation(string giftName, string eventName, string groupName, string userId, string giftId, string price)
        {
            if (!string.IsNullOrEmpty(giftName) && !string.IsNullOrEmpty(eventName) && !string.IsNullOrEmpty(groupName)
                && !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(giftId) && !string.IsNullOrEmpty(price))
            {
                dalGift.BuyGift(Convert.ToInt32(giftId), Convert.ToInt32(userId), double.Parse(price, CultureInfo.InvariantCulture), HttpContext.User.Identity.Name);
                dalGift.RemoveGiftFromReservation(HttpContext.User.Identity.Name, eventName, groupName, giftName);
                return PartialView("_Gifts", GetGiftsDetails(eventName));
            }

            return Content("");
        }

        [HttpPost]
        public ActionResult DeleteGroup(string groupName)
        {
            if (!string.IsNullOrEmpty(groupName))
                dalGroup.DeleteGroup(groupName, HttpContext.User.Identity.Name);

            return Content("");
        }

        [HttpPost]
        public ActionResult DeleteEvent(string eventName, string groupName)
        {
            if (!string.IsNullOrEmpty(groupName) && !string.IsNullOrEmpty(eventName))
                dalEvent.DeleteEvent(eventName, groupName, HttpContext.User.Identity.Name);

            return Content("");
        }

        #endregion

        #region Private Functions

        private GiftsViewModel GetGiftsDetails(string eventName)
        {
            var user = dalUser.GetUserByEmail(HttpContext.User.Identity.Name);

            GiftsViewModel vm = new GiftsViewModel()
            {
                WishList = user.WishList.Where(x => x.Event.Name.Equals(eventName)).ToList(),
                ReservedGifts = new List<ReservedGiftsStruct>(),
                BoughtGifts = new List<BoughtGiftsStruct>()
            };

            foreach (var i in user.ReservedGifts.Where(x => x.Event.Name.Equals(eventName)).ToList())
            {
                vm.ReservedGifts.Add(new ReservedGiftsStruct
                {
                    Gift = i,
                    User = i.User
                });
            }

            foreach (var i in user.BoughtGifts.Where(x => x.Event.Name.Equals(eventName)).ToList())
            {
                vm.BoughtGifts.Add(new BoughtGiftsStruct
                {
                    Gift = i,
                    User = i.User,
                    Price = i.Price
                });
            }

            return vm;
        }

        #endregion
    }
}