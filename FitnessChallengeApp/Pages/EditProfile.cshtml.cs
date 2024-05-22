using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization; 
using System.Threading.Tasks;
using FitnessChallengeApp.Models;
using FitnessChallengeApp.Data;
namespace FitnessChallengeApp.Pages
{
    [Authorize]
    public class EditProfileModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
