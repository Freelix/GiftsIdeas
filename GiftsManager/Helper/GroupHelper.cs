using GiftsManager.Models.Dal;
using GiftsManager.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GiftsManager.Models.Dal.IDal;

namespace GiftsManager.Helper
{
    public class GroupHelper
    {
        public static GroupDropDownViewModel GetGroups(IDalUser dalUser, string email, bool showAddGroup, bool readOnly = false)
        {
            GroupDropDownViewModel vm = new GroupDropDownViewModel
            {
                User = dalUser.GetUserByEmail(email),
                IsAdmin = showAddGroup,
                ReadOnly = readOnly
            };

            if (vm.User?.Groups != null)
                vm.Groups = new SelectList(vm.User.Groups, "Id", "Name");

            return vm;
        }
    }
}