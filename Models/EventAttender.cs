using System;

namespace Community.Models
{
    public class EventAttender
    {
        public Guid AttendedEventId { get; set; }
        public Event AttendedEvent { get; set; }
        public string AttenderId { get; set; }
        public ApplicationUser Attender { get; set; }
    }
}