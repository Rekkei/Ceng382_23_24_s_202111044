using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using FitnessChallengeApp.Models;
using System;

namespace FitnessChallengeApp.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public ApplicationUser UserProfile { get; set; }
        public string ProfilePictureBase64 { get; set; }

        public async Task OnGetAsync()
        {
            UserProfile = await _userManager.GetUserAsync(User);
            if (UserProfile?.ProfilePictureUrl != null)
            {
                ProfilePictureBase64 = Convert.ToBase64String(UserProfile.ProfilePictureUrl);
            }
        }
    }
}
