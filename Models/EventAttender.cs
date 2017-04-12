using System;

namespace Community.Models
{
    public class EventAttender
    {
        public Guid EventId { get; set; }
        public int EventIndex { get; set; }
        public Event Event { get; set; }
        public string AttenderId { get; set; }
        public int AttenderIndex { get; set; }
        public ApplicationUser Attender { get; set; }
    }
}