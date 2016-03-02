using GiftsManager.Models.Dal;
using GiftsManager.ViewModels.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftsManager.Helper
{
    public class GroupHelper
    {
        public static GroupDropDownViewModel GetGroups(IDalUser dalUser, string email, bool showAddGroup, bool readOnly = false)
        {
            GroupDropDownViewModel vm = new GroupDropDownViewModel();
            vm.User = dalUser.GetUserByEmail(email);
            vm.IsAdmin = showAddGroup;
            vm.ReadOnly = readOnly;

            if (vm.User != null && vm.User.Groups != null)
                vm.Groups = new SelectList(vm.User.Groups, "Id", "Name");

            return vm;
        }
    }
}