using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data;
using System;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace FitnessChallengeApp
{
    public class ChallengeDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChallengeDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Challenge Challenge { get; set; }
        public IList<Comment> Comments { get; set; }
        public double AverageRating { get; set; }

        public string SavedByUserName { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Challenge = await _context.Challenges
                .Include(c => c.CreatedBy)
                .Include(c => c.Comments)
                    .ThenInclude(comment => comment.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Challenge == null)
            {
                return NotFound();
            }

            Comments = Challenge.Comments.ToList();
            var ratings = await _context.Ratings
                .Where(r => r.ChallengeId == id)
                .Select(r => r.Value)
                .ToListAsync();

    AverageRating = ratings.Any() ? ratings.Average() : 0;
    

            return Page();
        }

        public async Task<IActionResult> OnPostAddCommentAsync(int id, string text)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var comment = new Comment
            {
                ChallengeId = id,
                Text = text,
                User = user,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostRateChallengeAsync(int id, int value)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var rating = new Rating
            {
                ChallengeId = id,
                Value = value,
                User = user
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostSaveChallengeAsync(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
             
            user.SavedChallenges.Add(challenge);
            SavedByUserName = user.UserName;
            await _userManager.UpdateAsync(user);

            return RedirectToPage(new { id });
        }
    }
}
