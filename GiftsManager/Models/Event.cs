using System.ComponentModel.DataAnnotations;

namespace GiftsManager.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Group Group { get; set; }
    }
}