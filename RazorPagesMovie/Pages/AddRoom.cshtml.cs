using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
//using AppDbContext.Models;

public class AddRoomModel : PageModel
{
    private readonly  AppDbContext _dbContext;

    public AddRoomModel(AppDbContext dbContext)
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

        return Page();
    }
}