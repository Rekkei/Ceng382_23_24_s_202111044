using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
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
            if (!ModelState.IsValid)
            {
                Rooms = new SelectList(_dbContext.Rooms, "Id", "RoomName");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                 Reservation.ReservedBy = userId;
                _dbContext.Reservations.Add(Reservation);
                await _dbContext.SaveChangesAsync();
                return RedirectToPage("ReservationList");
            }

            

            // Log the creation
            /* _dbContext.Logs.Add(new Log { Message = $"Reservation created by {userId} for room {Reservation.RoomId} at {Reservation.DateTime}" });
            await _dbContext.SaveChangesAsync(); */

            return RedirectToPage("ReservationList");
        }
    }
}
