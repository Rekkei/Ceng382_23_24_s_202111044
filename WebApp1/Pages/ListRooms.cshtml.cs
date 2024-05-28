using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApp1.Data;
using WebApp1.Models;
using Microsoft.AspNetCore.Authorization; 

namespace WebApp1.Pages
{
    [Authorize]
    public class ListRoomsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public ListRoomsModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IList<Room> Rooms { get; set; }

        public async Task OnGetAsync()
        {
            Rooms = await _dbContext.Rooms.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var room = await _dbContext.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            _dbContext.Rooms.Remove(room);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
