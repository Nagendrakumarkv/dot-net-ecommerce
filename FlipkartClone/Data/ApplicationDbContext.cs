using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Add this
using Microsoft.EntityFrameworkCore;
using FlipkartClone.Models;

namespace FlipkartClone.Data
{
    // Change DbContext to IdentityDbContext
    public class ApplicationDbContext : IdentityDbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}