using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp1.Data;
using WebApp1.Models;

namespace WebApp1.Pages
{
    public class ReservationListModel : PageModel
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ReservationListModel(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public Dictionary<string, List<Reservation>> WeeklyReservations { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RoomFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DateFilterStart { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? DateFilterEnd { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CapacityFilter { get; set; }

        public async Task OnGetAsync()
        {
            var query = _dbContext.Reservations.Include(r => r.Room).AsQueryable();

            if (!string.IsNullOrEmpty(RoomFilter))
            {
                query = query.Where(r => r.Room.RoomName.Contains(RoomFilter));
            }

            if (DateFilterStart.HasValue && DateFilterEnd.HasValue)
            {
                query = query.Where(r => r.DateTime >= DateFilterStart.Value && r.DateTime <= DateFilterEnd.Value);
            }

            if (CapacityFilter.HasValue)
            {
                query = query.Where(r => r.Room.Capacity >= CapacityFilter.Value);
            }

            var reservations = await query.ToListAsync();
            var groupedByDate = reservations.GroupBy(r => r.DateTime.Date).ToDictionary(g => g.Key, g => g.ToList());

            WeeklyReservations = new Dictionary<string, List<Reservation>>();

            DateTime startOfWeek = DateFilterStart?.StartOfWeek(DayOfWeek.Monday) ?? DateTime.Today.StartOfWeek(DayOfWeek.Monday);
            DateTime endOfWeek = startOfWeek.AddDays(6);

            for (DateTime date = startOfWeek; date <= endOfWeek; date = date.AddDays(1))
            {
                string key = date.ToString("dddd, MMM dd");

                if (groupedByDate.ContainsKey(date))
                {
                    WeeklyReservations[key] = groupedByDate[date];
                }
                else
                {
                    WeeklyReservations[key] = new List<Reservation>();
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var reservation = await _dbContext.Reservations.Include(r => r.Room).FirstOrDefaultAsync(r => r.Id == id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Retrieve user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Create a log entry before deleting the reservation
            var logMessage = reservation.Room != null
                ? $"Reservation for room '{reservation.Room.RoomName}' by user '{reservation.ReservedBy}' was deleted by user ID '{userId}'"
                : $"Reservation by user '{reservation.ReservedBy}' was deleted by user ID '{userId}'";
            
            var log = new Log
            {
                Message = logMessage,
                Timestamp = DateTime.UtcNow
            };
            _dbContext.Logs.Add(log);

            _dbContext.Reservations.Remove(reservation);
            await _dbContext.SaveChangesAsync();

            return RedirectToPage();
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
