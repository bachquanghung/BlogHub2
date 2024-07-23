using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BlogHub1.Services;
using BlogHub1.Models;

namespace BlogHub1.Pages
{
    [Authorize(Roles = "admin")]
    public class AdminModel : PageModel
    {
        private readonly BlogHubDbContext _dbContext;
        private readonly UserManager<BlogUser> _userManager;

        public AdminModel(BlogHubDbContext dbContext, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IList<BlogUser> BlogUsers { get; set; } = new List<BlogUser>();
        public Dictionary<string, IList<string>> UserRoles { get; set; } = new Dictionary<string, IList<string>>();

        public async Task OnGetAsync()
        {
            BlogUsers = await _dbContext.BlogUsers.ToListAsync();
            UserRoles = new Dictionary<string, IList<string>>();

            foreach (var user in BlogUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                UserRoles[user.Id] = roles;
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _dbContext.BlogUsers.FindAsync(id);

            if (user != null)
            {
                _dbContext.BlogUsers.Remove(user);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostChangeRoleAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "User ID is required.");
                return Page();
            }

            // Find the user by ID
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return Page();
            }

            // Get the current roles of the user
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Determine the new role
            var newRole = currentRoles.Contains("admin") ? "reader" : "admin";

            // Remove all current roles
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove current roles.");
                return Page();
            }

            // Add the user to the new role
            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add new role.");
                return Page();
            }

            // Redirect to refresh the page
            return RedirectToPage();
        }

    }
}
