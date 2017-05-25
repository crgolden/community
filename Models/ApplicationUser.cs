using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Community.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Name => $"{FirstName} {LastName}";
        public ICollection<Address> Addresses { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<EventAttender> AttendedEvents { get; set; }
        public ICollection<EventFollower> FollowedEvents { get; set; }
        public ICollection<ApplicationUserFollower> FollowedUsers { get; set; }
        public ICollection<ApplicationUserFollower> Followers { get; set; }
    }
}
