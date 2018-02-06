using System;
using System.Collections.Generic;

namespace community.Models
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool Home { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Event> Events { get; set; }
    }
}