using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp1.Data;
using WebApp1.Models;

namespace WebApp1.Pages
{
    public class CreateReservationModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;

        public CreateReservationModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public SelectList Rooms { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Rooms = new SelectList(_dbContext.Rooms, "Id", "RoomName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
        
           
            if (_dbContext.Reservations.Any(r =>
                r.DateTime <= Reservation.DateTime &&
                r.DateTime.AddMinutes(30) >= Reservation.DateTime))
            {
                ModelState.AddModelError("Reservation.DateTime", "Another reservation already exists for the selected room in the same time interval.");
            }

            if (!ModelState.IsValid)
            {
                Rooms = new SelectList(_dbContext.Rooms, "Id", "RoomName");
                 var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Reservation.ReservedBy = userId;
            _dbContext.Reservations.Add(Reservation);
            await _dbContext.SaveChangesAsync();
            var room = await _dbContext.Rooms.FindAsync(Reservation.RoomId);
            var log = new Log
            {
                Message = $"Room '{room.RoomName}' was reserved by user '{userId}'",
                Timestamp = DateTime.UtcNow
            };
            _dbContext.Logs.Add(log);
            await _dbContext.SaveChangesAsync();
                return RedirectToPage("ReservationList");
            }
            
           

            return RedirectToPage("ReservationList");
        }
    }
}
