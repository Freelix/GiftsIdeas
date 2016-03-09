using GiftsManager.Models;

namespace GiftsManager.ViewModels.Authentication
{
    public class SignInViewModel
    {
        public User User { get; set; }
        public bool Authenticated { get; set; }
    }
}