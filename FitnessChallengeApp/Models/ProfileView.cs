using System.ComponentModel.DataAnnotations;
public class ProfileViewModel
{
    public string UserId { get; set; }

    [Display(Name = "Profile Picture")]
    public byte[] ProfilePicture { get; set; }

    public string Bio { get; set; }
}
