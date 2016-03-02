using GiftsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftsManager.ViewModels.Home
{
    public class GroupGiftsViewModel
    {
        public List<User> Users { get; set; }
        public Group ActualGroup { get; set; }
        public string EventName { get; set; }
    }
}