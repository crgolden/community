using System;

namespace community.Models
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