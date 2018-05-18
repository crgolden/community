using System;
using System.ComponentModel.DataAnnotations;
using community.Core.Models;

namespace community.Api.v1.ViewModels
{
    // TypeScript: event.ts
    public class EventViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public Guid AddressId { get; set; }
        [Required]
        public string Street { get; set; }
        public string Street2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }

        public EventViewModel()
        {
        }

        public EventViewModel(Event @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Details = @event.Details;
            Date = @event.Date;
            UserId = @event.UserId;
            AddressId = @event.AddressId;
            Street = @event.Address.Street;
            Street2 = @event.Address.Street2;
            City = @event.Address.City;
            State = @event.Address.City;
            ZipCode = @event.Address.ZipCode;
        }
    }
}