namespace Community.Models
{
    public class EventAttending
    {
        public int AttenderId { get; set; }
        public ApplicationUser Attender { get; set; }
        public int AttendedEventId { get; set; }
        public Event AttendedEvent { get; set; }
    }
}