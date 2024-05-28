
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp1.Pages;
using WebApp1.Data;
using Microsoft.AspNetCore.Authorization; 
using WebApp1.Models;


namespace WebApp1.Pages{

[Authorize]
public class AddRoomModel : PageModel
{
    private readonly  ApplicationDbContext _dbContext;

    public AddRoomModel(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [BindProperty]
    public Room Room { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _dbContext.Rooms.Add(Room);
        _dbContext.SaveChanges();

        Room = new Room();

        return RedirectToPage("ListRooms");
    }
}

}
