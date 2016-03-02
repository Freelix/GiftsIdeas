using GiftsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftsManager.ViewModels.Profile
{
    public class GiftsViewModel
    {
        public List<Gift> WishList { get; set; }
        public List<ReservedGiftsStruct> ReservedGifts { get; set; }
        public List<BoughtGiftsStruct> BoughtGifts { get; set; }
    }

    public struct ReservedGiftsStruct
    {
        public Gift Gift;
        public User User;
    }

    public struct BoughtGiftsStruct
    {
        public Gift Gift;
        public User User;
        public double Price;
    }
}