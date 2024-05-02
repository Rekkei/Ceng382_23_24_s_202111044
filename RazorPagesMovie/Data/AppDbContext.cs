using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
public AppDbContext(DbContextOptions<AppDbContext> options) :
base(options)
{
}
public DbSet<Room> Rooms { get; set; }
public DbSet<Reservation> Reservations { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany()
            .HasForeignKey(r => r.RoomId);
    }
}