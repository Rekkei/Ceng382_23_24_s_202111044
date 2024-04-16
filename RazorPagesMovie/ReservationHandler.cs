public class ReservationHandler
{
    private readonly IReservationRepository _reservationRepository;
    private readonly RoomHandler _roomHandler;

    private Dictionary<string, List<DateTime>> _roomTimeSlots;


    public ReservationHandler(IReservationRepository reservationRepository, RoomHandler roomHandler)
    {
        _reservationRepository = reservationRepository;
        _roomHandler = roomHandler;

        // Generate time slots for each room
        var rooms = _roomHandler.GetRooms();
        _roomTimeSlots = GenerateTimeSlotsForRooms(rooms);
    }

private Dictionary<string, List<DateTime>> GenerateTimeSlotsForRooms(List<Room> rooms)
{
    // Define your logic to generate time slots for each room here
    Dictionary<string, List<DateTime>> roomTimeSlots = new Dictionary<string, List<DateTime>>();

    foreach (var room in rooms)
    {
        List<DateTime> timeSlots = new List<DateTime>();
        DateTime currentDate = DateTime.Today.Date; // Start from today and set the time to 9 am

        // Start generating time slots from 9 am to 6 pm
        for (int i = 9; i <= 18; i++)
        {
            // Add the current hour to the time slots list
            timeSlots.Add(currentDate.AddHours(i));
        }

        // Associate the generated time slots with the room ID
        roomTimeSlots.Add(room.RoomId, timeSlots);
    }

    return roomTimeSlots;
}

public bool AddReservation(Reservation reservation, string reserverName, DateTime chosenDateTime)
{
    // Check if the chosen date and time slot is available for the selected room
    if (IsTimeSlotAvailable(reservation.Room.RoomId, chosenDateTime))
    {
        reservation.DateTime = chosenDateTime; // Set the reservation date and time
        _reservationRepository.AddReservation(reservation); // Add the reservation

        // Log reservation added successfully
        var logRecord = new LogRecord
        {
            Timestamp = DateTime.Now, // Add timestamp for logging
            Action = "Reservation added",
            ReserverName = reserverName,
            RoomName = reservation.Room.RoomId,
            DateTime = reservation.DateTime // Log the chosen date and time
        };

        LogReservation(reservation.ReservedBy, reservation.Room.RoomId, "deleted");
        Console.WriteLine("\nReservation added successfully.\n");

        return true; // Return true to indicate successful addition
    }
    else
    {
        Console.WriteLine("Selected date and time slot is not available for the chosen room.");
        return false; // Return false to indicate failed addition
    }
}

private bool IsTimeSlotAvailable(string roomId, DateTime chosenDateTime)
{   
    if (_roomTimeSlots.ContainsKey(roomId))
    {
        // Check if the chosen date and time slot is within the predefined time slots for the room
        if (!_roomTimeSlots[roomId].Contains(chosenDateTime))
        {
            Console.WriteLine("Selected date and time slot is not within the available time slots for the chosen room.");
            return false;
        }

        // Check if the chosen time slot is already reserved
        var reservationsForRoom = _reservationRepository.GetAllReservations().Where(r => r.Room.RoomId == roomId);
        if (reservationsForRoom.Any(r => r.DateTime == chosenDateTime))
        {
            Console.WriteLine("Selected date and time slot is already reserved for the chosen room.");
            return false;
        }

        return true;
    }
    else
    {
        Console.WriteLine("Invalid room ID.");
        return false;
    }
}



    public void DeleteReservation(Reservation reservation)
    {
        try
        {
            _reservationRepository.DeleteReservation(reservation);
            LogReservation(reservation.ReservedBy, reservation.Room.RoomId, "deleted");
            Console.WriteLine("\nReservation deleted successfully.\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete reservation: {ex.Message}");
        }
    }


    public void DisplayScheduleForWeek()
    {

        var reservations = _reservationRepository.GetAllReservations();

        Console.WriteLine($"Total reservations: {reservations.Count}");
        Console.WriteLine("\n-------------------------------------------------------");
        Console.WriteLine("|   Date     |   Time    |     Room    |  Reserved By |");
        Console.WriteLine("-------------------------------------------------------");


        foreach (var reservation in reservations)
        {
            if (reservation != null) // Check for current week
            {
                Console.WriteLine($"| {reservation.DateTime.ToShortDateString(),-10} | {reservation.DateTime.ToShortTimeString(),-8} | {reservation.Room.RoomId,-12} | {reservation.ReservedBy,-12} |");
            }
        }

        Console.WriteLine("-------------------------------------------------------\n");
    }

    public void ShowAvailableRooms(List<Room> rooms)
    {
        ShowRoomCapacities(rooms);
    }

    private void ShowRoomCapacities(List<Room> rooms)
    {
        Console.WriteLine("\n-------------------------------------------------------");
        Console.WriteLine("| Room ID | Room Name     | Remaining Capacity |");
        Console.WriteLine("-------------------------------------------------------");

        foreach (var room in rooms)
        {
            int existingReservations = _reservationRepository.GetAllReservations().Count(r => r.Room.RoomId == room.RoomId);
            int remainingCapacity = room.Capacity - existingReservations;

            Console.WriteLine($"| {room.RoomId,-7} | {room.RoomName,-13} | {remainingCapacity,-18} |");
        }

        Console.WriteLine("-------------------------------------------------------\n");
    }

    private void LogReservation(string reserverName, string roomName, string action)
    {
        var logRecord = new LogRecord
        {
            Timestamp = DateTime.Now,
            ReserverName = reserverName,
            RoomName = roomName
        };

        // Implement logic to write the log record to a file or external system (e.g., call LogHandler)
        // You'll likely need to inject a logger dependency into this class

        Console.WriteLine($"Reservation {action} successfully. Log created."); // Placeholder for logging success message
    }
}
