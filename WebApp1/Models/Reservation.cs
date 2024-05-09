public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }  // Foreign key
    public Room Room { get; set; }    // Navigation property
    public DateTime DateTime { get; set; }
    public string ReservedBy { get; set; }
}