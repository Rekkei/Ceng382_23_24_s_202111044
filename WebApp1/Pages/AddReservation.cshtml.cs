using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApp1.Data;
using WebApp1.Models;

namespace WebApp1.Pages
{
    public class ReservationModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Reservation Reservation { get; set; }

        public SelectList Rooms { get; set; }

        public ReservationModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            Rooms = new SelectList(_context.Rooms, "Id", "RoomName");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Rooms = new SelectList(_context.Rooms, "Id", "RoomName");
                return Page();
            }

            Reservation.ReservedBy = User.Identity.Name;

            Reservation.Room = await _context.Rooms.FindAsync(Reservation.Room.Id);

            if (Reservation.Room == null)
            {
                ModelState.AddModelError("Reservation.Room.Id", "Invalid room selection.");
                Rooms = new SelectList(_context.Rooms, "Id", "RoomName");
                return Page();
            }

            _context.Reservations.Add(Reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
