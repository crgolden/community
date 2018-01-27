using System;

namespace community.Models
{
    public class EventAttender
    {
        public Guid AttendedEventId { get; set; }
        public Event AttendedEvent { get; set; }
        public string AttenderId { get; set; }
        public User Attender { get; set; }
    }
}