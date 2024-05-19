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
    public class ListReservationsModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ListReservationsModel> _logger;

        public ListReservationsModel(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, ILogger<ListReservationsModel> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public string RoomNameFilter { get; set; }

        [BindProperty]
        public DateTime StartDateFilter { get; set; } = DateTime.Now.StartOfWeek(DayOfWeek.Monday);

        [BindProperty]
        public DateTime EndDateFilter { get; set; } = DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(7);

        [BindProperty]
        public int? CapacityFilter { get; set; }

        public List<SelectListItem> Rooms { get; set; }
        public List<Reservation> Reservations { get; set; }

        [BindProperty]
        public Reservation EditReservation { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await PopulateRoomsDropdown();
            await LoadReservations();
            return Page();
        }

        public async Task<IActionResult> OnPostFilterAsync()
        {
            await PopulateRoomsDropdown();
            await LoadReservations();
            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var reservation = await _dbContext.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            reservation.DateTime = EditReservation.DateTime;
            reservation.RoomId = EditReservation.RoomId;

            try
            {
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Reservation updated by {UserName} for Room {RoomId} at {DateTime}",
                    reservation.ReservedBy, reservation.RoomId, reservation.DateTime);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Reservations.Any(e => e.Id == reservation.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var reservation = await _dbContext.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            _dbContext.Reservations.Remove(reservation);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Reservation deleted by {UserName} for Room {RoomId} at {DateTime}",
                reservation.ReservedBy, reservation.RoomId, reservation.DateTime);

            return RedirectToPage();
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

        private async Task LoadReservations()
        {
            var query = _dbContext.Reservations.Include(r => r.Room).AsQueryable();

            if (!string.IsNullOrEmpty(RoomNameFilter))
            {
                query = query.Where(r => r.Room.RoomName.Contains(RoomNameFilter));
            }

            if (StartDateFilter != default && EndDateFilter != default)
            {
                query = query.Where(r => r.DateTime >= StartDateFilter && r.DateTime <= EndDateFilter);
            }

            if (CapacityFilter.HasValue)
            {
                query = query.Where(r => r.Room.Capacity == CapacityFilter);
            }

            Reservations = await query.ToListAsync();
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
