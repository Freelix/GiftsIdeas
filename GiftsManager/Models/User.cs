using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiftsManager.Models
{
    public sealed class User
    {
        public User()
        {
            Groups = new HashSet<Group>();
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public List<Gift> WishList { get; set; }
        public List<Gift> ReservedGifts { get; set; }
        public List<Gift> BoughtGifts { get; set; }

        public string Phone { get; set; }

        public ICollection<Group> Groups { get; set; }
    }
}