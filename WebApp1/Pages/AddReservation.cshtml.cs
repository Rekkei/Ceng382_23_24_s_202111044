using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp1.Data;

namespace WebApp1.Pages
{
    [Authorize]
    public class AddReservationModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AddReservationModel> _logger;

        public AddReservationModel(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, ILogger<AddReservationModel> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public List<SelectListItem> Rooms { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateRoomsDropdown();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await PopulateRoomsDropdown();
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            Reservation.ReservedBy = user.UserName;

            _logger.LogInformation("User {UserName} is creating a reservation for RoomId {RoomId} at {DateTime}",
                Reservation.ReservedBy, Reservation.RoomId, Reservation.DateTime);

            try
            {
                _dbContext.Reservations.Add(Reservation);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Reservation successfully created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reservation.");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the reservation.");
                await PopulateRoomsDropdown();
                return Page();
            }

            return RedirectToPage("/Index");
        }

        private async Task PopulateRoomsDropdown()
        {
            var rooms = await _dbContext.Rooms.ToListAsync();
            Rooms = rooms.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.RoomName
            }).ToList();
        }
    }
}
