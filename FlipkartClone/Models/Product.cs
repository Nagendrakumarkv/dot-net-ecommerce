using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlipkartClone.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string ISBN { get; set; } = string.Empty; // SKU or Product Code
        
        [Required]
        public string Author { get; set; } = string.Empty; // Or Brand Name
        
        [Range(1, 1000000)]
        public double Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty; // Main image

        // Relationships
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}