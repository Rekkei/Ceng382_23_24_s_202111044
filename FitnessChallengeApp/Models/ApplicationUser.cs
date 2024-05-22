using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string ProfilePictureUrl { get; set; }
    public string Bio { get; set; }
}