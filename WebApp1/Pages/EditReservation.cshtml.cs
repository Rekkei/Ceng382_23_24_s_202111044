using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApp1.Data;
using WebApp1.Models;

namespace WebApp1.Pages
{
    public class EditReservationModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public EditReservationModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public SelectList Rooms { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Reservation = await _dbContext.Reservations.Include(r => r.Room).FirstOrDefaultAsync(r => r.Id == id);
            if (Reservation == null)
            {
                return NotFound();
            }

            Rooms = new SelectList(_dbContext.Rooms, "Id", "RoomName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Rooms = new SelectList(_dbContext.Rooms, "Id", "RoomName");
                return Page();
            }

            var originalReservation = await _dbContext.Reservations.FindAsync(Reservation.Id);
            if (originalReservation == null)
            {
                return NotFound();
            }

            originalReservation.RoomId = Reservation.RoomId;
            originalReservation.DateTime = Reservation.DateTime;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservationExists(Reservation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("ReservationList");
        }

        private bool ReservationExists(int id)
        {
            return _dbContext.Reservations.Any(e => e.Id == id);
        }
    }
}
