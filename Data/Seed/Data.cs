using System.Collections.Generic;
using System.Security.Claims;
using Community.Models;
using Role = Microsoft.AspNetCore.Identity.IdentityRole;

namespace Community.Data.Seed
{
    public class Data
    {
        public ApplicationUser Jim { get; set; }
        public ApplicationUser Marlene { get; set; }
        public ApplicationUser Tim { get; set; }
        public Address YardBar { get; set; }
        public Address BullCreek { get; set; }
        public Address AlamoDrafthouse { get; set; }
        public Event Drinks { get; set; }
        public Event Softball { get; set; }
        public Event Movies { get; set; }

        public static IEnumerable<Claim> Claims =>
            new List<Claim>
            {
                new Claim("Admin", "Create User"),
                new Claim("Admin", "Edit User"),
                new Claim("Admin", "Delete User"),
                new Claim("Admin", "Edit Address"),
                new Claim("Admin", "Delete Address"),
                new Claim("User", "Create Address"),
                new Claim("User", "Create Event"),
                new Claim("User", "Edit Event"),
                new Claim("User", "Delete Event"),
                new Claim("User", "Attend Event"),
                new Claim("User", "Leave Event"),
                new Claim("User", "Follow User"),
                new Claim("User", "Unfollow User"),
                new Claim("User", "Follow Event"),
                new Claim("User", "Unfollow Event")
            };

        public static IEnumerable<Role> Roles => new List<Role>
        {
            new Role("User"),
            new Role("Admin")
        };

        public static IEnumerable<ApplicationUser> Users => new List<ApplicationUser>
        {
            new ApplicationUser
            {
                UserName = "jim@gmail.com",
                Email = "jim@gmail.com",
                FirstName = "Jim",
                LastName = "Brown"
            },
            new ApplicationUser
            {
                UserName = "marlene@gmail.com",
                Email = "marlene@gmail.com",
                FirstName = "Marlene",
                LastName = "Stephens"
            },
            new ApplicationUser
            {
                UserName = "tim@gmail.com",
                Email = "tim@gmail.com",
                FirstName = "Tim",
                LastName = "Jones"
            }
        };
    }
}