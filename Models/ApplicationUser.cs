using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Community.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Index { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<EventAttender> AttendedEvents { get; set; }
        public ICollection<EventFollower> FollowedEvents { get; set; }
        public ICollection<ApplicationUserFollower> FollowedUsers { get; set; }
        public ICollection<ApplicationUserFollower> Followers { get; set; }
    }
}