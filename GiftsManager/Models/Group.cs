using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GiftsManager.Models
{
    public sealed class Group
    {
        public Group()
        {
            Users = new HashSet<User>();
            Events = new HashSet<Event>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string GroupAdmin { get; set; }

        public ICollection<User> Users { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}