using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data;

namespace FitnessChallengeApp.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ApplicationUser UserProfile { get; set; }
        public string ProfilePictureBase64 { get; set; }
        public IQueryable<Challenge> SavedChallenges { get; set; }

        public async Task OnGetAsync()
        {
            UserProfile = await _userManager.GetUserAsync(User);
            if (UserProfile != null)
            {
                if (UserProfile.ProfilePictureUrl != null)
                {
                    ProfilePictureBase64 = Convert.ToBase64String(UserProfile.ProfilePictureUrl);
                }
                SavedChallenges = _context.Challenges.Where(c => c.CreatedBy.Id == UserProfile.Id);
            }
        }
        public async Task<IActionResult> OnPostSaveChallengeAsync(int id)
{
    var challenge = await _context.Challenges.FindAsync(id);
    if (challenge == null)
    {
        return NotFound();
    }

    var user = await _userManager.GetUserAsync(User);
    if (user.SavedChallenges == null)
    {
        user.SavedChallenges = new List<Challenge>(); // Initialize the collection if null
    }
    user.SavedChallenges.Add(challenge);
    await _userManager.UpdateAsync(user);

    return RedirectToPage(new { id });
}

    }
}
