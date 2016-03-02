using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GiftsManager.Models
{
    public class User
    {
        public User()
        {
            Groups = new HashSet<Group>();
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public List<Gift> WishList { get; set; }
        public List<Gift> ReservedGifts { get; set; }
        public List<Gift> BoughtGifts { get; set; }

        public string Phone { get; set; }

        public virtual ICollection<Group> Groups { get; set; }
    }
}