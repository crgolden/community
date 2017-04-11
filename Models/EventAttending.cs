namespace Community.Models
{
    public class EventAttending
    {
        public string AttenderId { get; set; }
        public int AttenderIdInt { get; set; }
        public ApplicationUser Attender { get; set; }
        public string AttendedEventId { get; set; }
        public int AttendedEventIdInt { get; set; }
        public Event AttendedEvent { get; set; }
    }
}