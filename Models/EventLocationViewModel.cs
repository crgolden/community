using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Community.Models
{
    public class EventLocationViewModel
    {
        public List<Event> events;
        public SelectList locations;
        public string eventLocation { get; set; }
    }
}