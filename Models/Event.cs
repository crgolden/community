using System;
using System.Collections.Generic;

namespace Community.Models
{
    public class Event
    {
        public Guid Id { get; set; }
        public int Index { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int CreatorIndex { get; set; }
        public ApplicationUser Creator { get; set; }
        public int AddressIndex { get; set; }
        public Address Address { get; set; }
        public ICollection<EventAttender> Attenders { get; set; }
        public ICollection<EventFollower> Followers { get; set; }
    }
}