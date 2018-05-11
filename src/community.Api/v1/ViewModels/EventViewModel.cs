using System;
using community.Core.Models;

namespace community.Api.v1.ViewModels
{
    // TypeScript: event.ts
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public Guid AddressId { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
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
        }
    }
}