using GiftsManager.Models;
using System.Web.Mvc;

namespace GiftsManager.ViewModels.Shared
{
    public class EventDropDownViewModel
    {
        public Group Group { get; set; }
        public SelectList Events { get; set; }
        public bool IsAdmin { get; set; }
    }
}