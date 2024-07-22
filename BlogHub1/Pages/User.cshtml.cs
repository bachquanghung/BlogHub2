using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BlogHub1.Models;
namespace BlogHub1.Pages
{
    [Authorize]
    public class UserModel : PageModel
    {
        private readonly UserManager<BlogUser> userManager;

        public BlogUser? blogUser;

        public UserModel(UserManager<BlogUser> userManager)
        {
            this.userManager = userManager;
        }
        public void OnGet()
        {
            var task = userManager.GetUserAsync(User);
            task.Wait();
            blogUser = task.Result;
        }
    }
}
