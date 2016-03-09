using System.ComponentModel.DataAnnotations;

namespace GiftsManager.Models
{
    public class Gift
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public StatusOption Status { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        public Event Event { get; set; }

        public User Buyer { get; set; }

        public double Price { get; set; }

        public bool IsBought { get; set; }

        public enum StatusOption
        {
            Open,
            Bought,
            Reserved
        };
    }
}