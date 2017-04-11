using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Community.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int IdInt { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<EventAttending> AttendedEvents { get; set; }
        public ICollection<EventFollowing> FollowedEvents { get; set; }
        public ICollection<ApplicationUserFollowing> FollowedUsers { get; set; }
        public ICollection<ApplicationUserFollowing> Followers { get; set; }
    }
}