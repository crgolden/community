using System;
using System.Collections.Generic;

namespace Community.Models
{
    public class Event
    {
        public string Id { get; set; }
        public int IdInt { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public ICollection<EventAttending> Attenders { get; set; }
        public ICollection<EventFollowing> Followers { get; set; }

        public Event()
        {
            Id = Guid.NewGuid().ToString("D");
        }
    }
}