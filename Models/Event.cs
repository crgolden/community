using System;
using System.ComponentModel.DataAnnotations;

namespace Community.Models
{
    public class Event
    {
        public int Id { get; set; }
        [StringLength(75, MinimumLength = 3)]
        public string Title { get; set; }
    }
}
