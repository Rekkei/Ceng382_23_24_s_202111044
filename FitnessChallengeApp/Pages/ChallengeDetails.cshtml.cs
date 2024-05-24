
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data;
using System;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace FitnessChallengeApp{
public class ChallengeDetailsModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ChallengeDetailsModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public Challenge Challenge { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Challenge = await _context.Challenges.Include(c => c.CreatedBy).FirstOrDefaultAsync(m => m.Id == id);
        if (Challenge == null)
        {
            return NotFound();
        }
        return Page();
    }
}


}
