using GiftsManager.Models.Dal;
using GiftsManager.ViewModels.Authentication;
using GiftsManager.ViewModels.Shared;
using GiftsManager.ViewModels.Home;
using System.Web.Mvc;
using GiftsManager.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GiftsManager.Models;

namespace GiftsManager.Controllers
{
    public class HomeController : BaseController
    {
        private IDalUser dalUser;
        private IDalGroup dalGroup;
        private IDalGift dalGift;
        private IDalEvent dalEvent;

        public HomeController() : this(new DalUser(), new DalGroup(), new DalGift(), new DalEvent())
        {

        }

        private HomeController(IDalUser dalUser, IDalGroup dalGroup, IDalGift dalGift, IDalEvent dalEvent)
        {
            this.dalUser = dalUser;
            this.dalGroup = dalGroup;
            this.dalGift = dalGift;
            this.dalEvent = dalEvent;
        }

        public ActionResult _Navigation()
        {
            SignInViewModel viewModel = new SignInViewModel
            {
                Authenticated = HttpContext.User.Identity.IsAuthenticated
            };
            
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                viewModel.User = dalUser.GetUserByEmail(HttpContext.User.Identity.Name);
            }
            
            return PartialView(viewModel);
        }

        public ActionResult Index()
        {
            return View(GroupHelper.GetGroups(dalUser, HttpContext.User.Identity.Name, false, true));
        }

        #region AjaxPosts

        [HttpPost]
        public ActionResult LoadEvents(string groupName)
        {
            return PartialView("_DropDownEvent", EventHelper.GetEvents(dalGroup, groupName, false));
        }

        [HttpPost]
        public ActionResult ChangeGroup(string groupName)
        {
            ActualGroupViewModel vm = new ActualGroupViewModel()
            {
                ActualGroup = dalGroup.GetGroupByName(groupName),
                User = dalUser.GetUserByEmail(HttpContext.User.Identity.Name),
                IsAdmin = false
            };

            return PartialView("_UsersInGroup", vm);
        }

        [HttpPost]
        public ActionResult LoadGiftsSection(string eventName, string groupName)
        {
            return PartialView("_GroupGifts", GetGroupGiftsDetails(eventName, groupName));
        }

        [HttpPost]
        public ActionResult BuyGift(string id, string userId, string groupName, string price)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(groupName) && !string.IsNullOrWhiteSpace(price))
                dalGift.BuyGift(Convert.ToInt32(id), Convert.ToInt32(userId), double.Parse(price, CultureInfo.InvariantCulture), HttpContext.User.Identity.Name);

            return Content("");
        }

        [HttpPost]
        public ActionResult ReserveGift(string id, string userId, string groupName)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(groupName))
                dalGift.ReserveGift(Convert.ToInt32(id), Convert.ToInt32(userId), HttpContext.User.Identity.Name);

            return Content("");
        }

        #endregion

        #region Private Functions

        private GroupGiftsViewModel GetGroupGiftsDetails(string eventName, string groupName)
        {
            var group = dalGroup.GetGroupByName(groupName);
            var user = dalUser.GetUserByEmail(HttpContext.User.Identity.Name);
            var allUsers = dalGroup.GetAllUsersButMe(group, user);

            return new GroupGiftsViewModel()
            {
                ActualGroup = dalGroup.GetGroupByName(groupName),
                Users = allUsers,
                EventName = eventName
            };
        }

        #endregion
    }
}