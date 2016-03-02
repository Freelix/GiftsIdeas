using GiftsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftsManager.ViewModels.Authentication
{
    public class SignInViewModel
    {
        public User User { get; set; }
        public bool Authenticated { get; set; }
    }
}