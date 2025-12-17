using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FlipkartClone.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string StreetAddress { get; set; } = string.Empty;

        [Required]
        public string City { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;

        [Required]
        public string ZipCode { get; set; } = string.Empty;

        // Link to the logged-in user
        public string UserId { get; set; } = string.Empty;
        public IdentityUser? User { get; set; }
    }
}