using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace GiftsManager.Models
{
    public class Group
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

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}