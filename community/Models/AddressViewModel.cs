using System;

namespace community.Models
{
    // TypeScript: address.ts
    public class AddressViewModel
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
        public UserViewModel User { get; set; }
        public EventViewModel[] Events { get; set; }

        public string FullAddress =>
            $"{Street}{(string.IsNullOrEmpty(Street2) ? string.Empty : $" {Street2}")}, {City}, {State} {ZipCode}";

        public AddressViewModel(Address address)
        {
            Id = address.Id;
            Street = address.Street;
            Street2 = address.Street2;
            City = address.City;
            State = address.State;
            ZipCode = address.ZipCode;
            Latitude = address.Latitude;
            Longitude = address.Longitude;
            Home = address.Home;
            UserId = address.UserId;
        }
    }
}