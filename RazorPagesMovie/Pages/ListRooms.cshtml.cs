using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ListRoomsModel : PageModel
{
    private readonly AppDbContext  _dbContext;

    public ListRoomsModel(AppDbContext  dbContext)
    {
        _dbContext = dbContext;
    }

    public IList<Room> Rooms { get; set; }

    public async Task OnGetAsync()
    {
        Rooms = await _dbContext.Rooms.ToListAsync();
    }
}
