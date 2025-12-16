using System.ComponentModel.DataAnnotations;

namespace FlipkartClone.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty; // e.g., Mobiles, Fashion

        public int DisplayOrder { get; set; } // To sort them on the navbar
    }
}