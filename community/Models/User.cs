using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace community.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<EventAttender> AttendedEvents { get; set; }
        public ICollection<EventFollower> FollowedEvents { get; set; }
        public ICollection<UserFollower> FollowedUsers { get; set; }
        public ICollection<UserFollower> Followers { get; set; }
    }
}
