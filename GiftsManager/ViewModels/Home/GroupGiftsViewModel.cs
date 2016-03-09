using GiftsManager.Models;
using System.Collections.Generic;

namespace GiftsManager.ViewModels.Home
{
    public class GroupGiftsViewModel
    {
        public List<User> Users { get; set; }
        public Group ActualGroup { get; set; }
        public string EventName { get; set; }
    }
}