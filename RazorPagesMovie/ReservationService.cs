public class ReservationService : IReservationService
{
    private readonly ReservationHandler _reservationHandler;
    private readonly LogHandler _logHandler;
    public RoomHandler RoomHandler { get; }


    public ReservationService(ReservationHandler reservationHandler, LogHandler logHandler, RoomHandler roomHandler)
    {
        _reservationHandler = reservationHandler;
        _logHandler = logHandler;
        RoomHandler = roomHandler;

    }

        public bool AddReservation(Reservation reservation, string reserverName, DateTime chosenDateTime)
    {
        if (_reservationHandler.AddReservation(reservation, reserverName, chosenDateTime))
        {
            var logRecord = new LogRecord
            {
                Timestamp = DateTime.Now,
                Action = "Reservation added",
                ReserverName = reserverName,
                RoomName = reservation.Room?.RoomId,
                DateTime = reservation.DateTime
            };

            _logHandler.AddLog(logRecord);
            Console.WriteLine("\nReservation added successfully.\n");
            return true; // Return true to indicate successful addition
        }
        else
        {
            return false; // Return false if reservation addition fails
        }
    }


    public void DeleteReservation(Reservation reservation)
    {
        try
        {
                        // Log reservation deleted successfully (assuming success)
            var logRecord = new LogRecord
            {
                Timestamp = DateTime.Now,
                Action = "Reservation deleted",
                ReserverName = reservation.ReservedBy,
                RoomName = reservation.Room.RoomId
            };

            _logHandler.AddLog(logRecord);
            _reservationHandler.DeleteReservation(reservation); // Call the method even if it doesn't return bool


            Console.WriteLine("\nReservation deleted successfully.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting reservation: {ex.Message}");
            // Optionally log the exception details using _logHandler
        }
    }


    public void DisplayWeeklySchedule()
    {
        _reservationHandler.DisplayScheduleForWeek();
    }
}