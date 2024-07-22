using Microsoft.AspNetCore.Identity;
namespace BlogHub1.Models
{
    public class BlogUser : IdentityUser
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string Address { get; set; } = "";

        public DateTime CreatedAt { get; set; } 
    }
}
