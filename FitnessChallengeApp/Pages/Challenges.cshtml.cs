using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace FitnessChallengeApp
{
    public class ChallengesModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ChallengesModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Challenge> Challenges { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Difficulties { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedDifficulty { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Period { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Challenges.Include(c => c.CreatedBy).AsQueryable();

            Categories = await _context.Challenges.Select(c => c.Category).Distinct().ToListAsync();
            Difficulties = await _context.Challenges.Select(c => c.Difficulty).Distinct().ToListAsync();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.Name.Contains(SearchTerm) || c.Instructions.Contains(SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(SelectedCategory))
            {
                query = query.Where(c => c.Category == SelectedCategory);
            }

            if (!string.IsNullOrWhiteSpace(SelectedDifficulty))
            {
                query = query.Where(c => c.Difficulty == SelectedDifficulty);
            }

            if (!string.IsNullOrWhiteSpace(Period))
            {
                query = query.Where(c => c.Period.Contains(Period));
            }

            Challenges = await query.ToListAsync();
        }
    }
}
