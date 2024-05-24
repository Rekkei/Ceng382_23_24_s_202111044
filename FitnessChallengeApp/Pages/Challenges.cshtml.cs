// Pages/Challenges.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data;
using System;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace FitnessChallengeApp{

public class ChallengesModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public ChallengesModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public IList<Challenge> Challenges { get; set; }

    public async Task OnGetAsync()
    {
        Challenges = await _context.Challenges.Include(c => c.CreatedBy).ToListAsync();
    }
}
}

