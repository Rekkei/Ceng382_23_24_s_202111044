using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization; 
using System.Threading.Tasks;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data;

namespace FitnessChallengeApp.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
         private readonly UserManager<ApplicationUser> _userManager; 

        public ProfileModel(UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
        }

        public ApplicationUser UserProfile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserProfile = user;

            return Page();
        } 
    }
}
