namespace WebApp1.Models
{
    public class Reservation
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public DateTime DateTime { get; set; }
    public string ReservedBy { get; set; }
}

}
