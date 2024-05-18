using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO; 
using Microsoft.AspNetCore.Identity; 
using System.ComponentModel.DataAnnotations;
using FitnessChallengeApp.Models;
namespace FitnessChallengeApp.Pages{
public class ProfileModel : PageModel
{
public async Task<IActionResult> OnPostAsync()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    var user = await _userManager.GetUserAsync(User);
    if (user == null)
    {
        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
    }

    if (Input.ProfilePicture != null && Input.ProfilePicture.Length > 0)
    {
        using (var memoryStream = new MemoryStream())
        {
            await Input.ProfilePicture.CopyToAsync(memoryStream);
            user.ProfilePicture = memoryStream.ToArray();
        }
    }

    user.Bio = Input.Bio;

    await _userManager.UpdateAsync(user);

    return RedirectToPage();
}

}
}

