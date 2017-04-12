using System;
using System.Collections.Generic;

namespace Community.Models
{
    public class Address
    {
        public string Id { get; set; }
        public int Index { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool Home { get; set; }
        public int CreatorIndex { get; set; }
        public ApplicationUser Creator { get; set; }
        public ICollection<Event> Events { get; set; }

        public Address()
        {
            Id = Guid.NewGuid().ToString("D");
        }
    }
}