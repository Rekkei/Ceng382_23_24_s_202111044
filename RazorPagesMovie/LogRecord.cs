public class LogRecord
{
    public DateTime Timestamp { get; set; }
    public string? ReserverName { get; set; }
    public string? RoomName { get; set; }
    public string? Action { get; set; } // Add this property

    public DateTime? DateTime { get; set; } 


}