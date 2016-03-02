using GiftsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftsManager.ViewModels.Home
{
    public class HomeViewModel
    {
        public User User { get; set; }
        public Group ActualGroup { get; set; }
    }
}