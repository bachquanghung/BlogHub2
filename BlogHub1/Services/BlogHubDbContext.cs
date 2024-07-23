using BlogHub1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace BlogHub1.Services
{
    public class BlogHubDbContext : IdentityDbContext<BlogUser>
    {
        public BlogHubDbContext(DbContextOptions options) : base(options) { 

        }
        public DbSet<BlogHub1.Models.BlogUser> BlogUsers { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var reader = new IdentityRole("reader");
            reader.NormalizedName = "reader";

            builder.Entity<IdentityRole>().HasData(reader, admin);
        }
    }
}
