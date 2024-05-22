using Microsoft.AspNetCore.Identity;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data;
namespace FitnessChallengeApp.Models{
public class ApplicationUser : IdentityUser
{
    public byte[]? ProfilePictureUrl { get; set; }
    public string? Bio { get; set; }
}
}
