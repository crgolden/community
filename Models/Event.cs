using System;
using System.Collections.Generic;

namespace Community.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<EventAttender> Attenders { get; set; }
        public ICollection<EventFollower> Followers { get; set; }
    }
}