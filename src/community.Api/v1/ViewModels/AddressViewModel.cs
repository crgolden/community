using System;
using System.ComponentModel.DataAnnotations;
using community.Core.Models;

namespace community.Api.v1.ViewModels
{
    // TypeScript: address.ts
    public class AddressViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Street { get; set; }
        public string Street2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool Home { get; set; }
        [Required]
        public string UserId { get; set; }

        public string FullAddress =>
            $"{Street}{(string.IsNullOrEmpty(Street2) ? string.Empty : $" {Street2}")}, {City}, {State} {ZipCode}";

        public AddressViewModel()
        {
        }

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