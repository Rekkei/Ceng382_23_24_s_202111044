using Microsoft.AspNetCore.Identity;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace FitnessChallengeApp.Models{
public class ApplicationUser : IdentityUser
{
    public byte[]? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
    public ICollection<Challenge> SavedChallenges { get; set; } = new List<Challenge>();
}
}
