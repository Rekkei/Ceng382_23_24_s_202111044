public interface IReservationService
{
    bool AddReservation(Reservation reservation, string reserverName, DateTime chosenDateTime);
    void DeleteReservation(Reservation reservation);
    void DisplayWeeklySchedule();
}