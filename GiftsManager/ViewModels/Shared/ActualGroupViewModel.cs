using GiftsManager.Models;

namespace GiftsManager.ViewModels.Shared
{
    public class ActualGroupViewModel
    {
        public User User { get; set; }
        public Group ActualGroup { get; set; }
        public bool IsAdmin { get; set; }
    }
}