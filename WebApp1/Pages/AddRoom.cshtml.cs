using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp1.Data;
using WebApp1.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebApp1.Pages
{
    [Authorize]
    public class AddRoomModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

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

            // Check if a room with the same name already exists
            if (_dbContext.Rooms.Any(r => r.RoomName == Room.RoomName))
            {
                ModelState.AddModelError("Room.RoomName", "A room with the same name already exists.");
                return Page();
            }

            _dbContext.Rooms.Add(Room);
            _dbContext.SaveChanges();

            Room = new Room();

            return RedirectToPage("ListRooms");
        }
    }
}
