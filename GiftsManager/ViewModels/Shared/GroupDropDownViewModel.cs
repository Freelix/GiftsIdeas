using GiftsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftsManager.ViewModels.Shared
{
    public class GroupDropDownViewModel
    {
        public User User { get; set; }
        public SelectList Groups { get; set; }
        public bool IsAdmin { get; set; }
        public bool ReadOnly { get; set; }
    }
}