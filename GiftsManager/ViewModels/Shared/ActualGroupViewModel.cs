using GiftsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftsManager.ViewModels.Shared
{
    public class ActualGroupViewModel
    {
        public User User { get; set; }
        public Group ActualGroup { get; set; }
        public bool IsAdmin { get; set; }
    }
}