using GiftsManager.Models.Dal;
using GiftsManager.ViewModels.Authentication;
using GiftsManager.ViewModels.Shared;
using GiftsManager.ViewModels.Home;
using System.Web.Mvc;
using GiftsManager.Helper;
using System;
using System.Globalization;
using GiftsManager.Models.Dal.IDal;

namespace GiftsManager.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IDalUser _dalUser;
        private readonly IDalGroup _dalGroup;
        private readonly IDalGift _dalGift;

        public HomeController() : this(new DalUser(), new DalGroup(), new DalGift())
        {

        }

        private HomeController(IDalUser dalUser, IDalGroup dalGroup, IDalGift dalGift)
        {
            _dalUser = dalUser;
            _dalGroup = dalGroup;
            _dalGift = dalGift;
        }

        public ActionResult _Navigation()
        {
            SignInViewModel viewModel = new SignInViewModel
            {
                Authenticated = HttpContext.User.Identity.IsAuthenticated
            };
            
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                viewModel.User = _dalUser.GetUserByEmail(HttpContext.User.Identity.Name);
            }
            
            return PartialView(viewModel);
        }

        public ActionResult Index()
        {
            return View(GroupHelper.GetGroups(_dalUser, HttpContext.User.Identity.Name, false, true));
        }

        #region AjaxPosts

        [HttpPost]
        public ActionResult LoadEvents(string groupName)
        {
            return PartialView("_DropDownEvent", EventHelper.GetEvents(_dalGroup, groupName, false));
        }

        [HttpPost]
        public ActionResult ChangeGroup(string groupName)
        {
            ActualGroupViewModel vm = new ActualGroupViewModel()
            {
                ActualGroup = _dalGroup.GetGroupByName(groupName),
                User = _dalUser.GetUserByEmail(HttpContext.User.Identity.Name),
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
                _dalGift.BuyGift(Convert.ToInt32(id), Convert.ToInt32(userId), double.Parse(price, CultureInfo.InvariantCulture), HttpContext.User.Identity.Name);

            return Content("");
        }

        [HttpPost]
        public ActionResult ReserveGift(string id, string userId, string groupName)
        {
            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(groupName))
                _dalGift.ReserveGift(Convert.ToInt32(id), Convert.ToInt32(userId), HttpContext.User.Identity.Name);

            return Content("");
        }

        #endregion

        #region Private Functions

        private GroupGiftsViewModel GetGroupGiftsDetails(string eventName, string groupName)
        {
            var group = _dalGroup.GetGroupByName(groupName);
            var user = _dalUser.GetUserByEmail(HttpContext.User.Identity.Name);
            var allUsers = _dalGroup.GetAllUsersButMe(group, user);

            return new GroupGiftsViewModel()
            {
                ActualGroup = _dalGroup.GetGroupByName(groupName),
                Users = allUsers,
                EventName = eventName
            };
        }

        #endregion
    }
}