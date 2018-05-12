using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using community.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Role = Microsoft.AspNetCore.Identity.IdentityRole;

namespace community.Data
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private User Jim { get; set; }
        private User Marlene { get; set; }
        private User Tim { get; set; }
        private Address YardBar { get; set; }
        private Address BullCreek { get; set; }
        private Address AlamoDrafthouse { get; set; }
        private Event Drinks { get; set; }
        private Event Softball { get; set; }
        private Event Movies { get; set; }

        private static IEnumerable<Claim> Claims =>
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

        private static IEnumerable<Role> Roles => new List<Role>
        {
            new Role("User"),
            new Role("Admin")
        };

        private static IEnumerable<User> Users => new List<User>
        {
            new User
            {
                UserName = "jim@gmail.com",
                Email = "jim@gmail.com",
                FirstName = "Jim",
                LastName = "Brown"
            },
            new User
            {
                UserName = "marlene@gmail.com",
                Email = "marlene@gmail.com",
                FirstName = "Marlene",
                LastName = "Stephens"
            },
            new User
            {
                UserName = "tim@gmail.com",
                Email = "tim@gmail.com",
                FirstName = "Tim",
                LastName = "Jones"
            }
        };

        public Seed(ApplicationDbContext context, UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedData(string adminEmail, string adminPassword)
        {
            if (!await _roleManager.Roles.AnyAsync()) await CreateRolesAsync();
            if (!await _userManager.Users.AnyAsync()) await CreateUsersAsync(adminEmail, adminPassword);
            if (!await _context.UserFollowers.AnyAsync()) await CreateUserFollowers();
            if (!await _context.Addresses.AnyAsync()) await CreateAddresses();
            if (!await _context.Events.AnyAsync()) await CreateEvents();
            if (!await _context.EventAttenders.AnyAsync()) await CreateEventAttenders();
            if (!await _context.EventFollowers.AnyAsync()) await CreateEventFollowers();
        }

        private async Task CreateRolesAsync()
        {
            using (_roleManager)
            {
                foreach (var role in Roles)
                {
                    await _roleManager.CreateAsync(role);
                    foreach (var claim in Claims.Where(claim => claim.Type == role.Name))
                        await _roleManager.AddClaimAsync(role, claim);
                }
            }
        }

        private async Task CreateUsersAsync(string adminEmail, string adminPassword)
        {
            var admin = new User
            {
                UserName = adminEmail,
                FirstName = "Jack",
                LastName = "Smith",
                Email = adminEmail,
                SecurityStamp = $"{Guid.NewGuid()}"
            };
            using (_userManager)
            {
                await _userManager.CreateAsync(admin, adminPassword);
                await _userManager.AddToRolesAsync(admin, Roles.Select(role => role.Name));
                await _userManager.AddClaimsAsync(admin, Claims);

                foreach (var user in Users)
                {
                    await _userManager.CreateAsync(user, "@Password1");
                    await _userManager.AddToRoleAsync(user, "User");
                    await _userManager.AddClaimsAsync(user, Claims.Where(claim =>
                        claim.Type.Equals("User")));
                }

                Jim = await _userManager.FindByEmailAsync("jim@gmail.com");
                Marlene = await _userManager.FindByEmailAsync("marlene@gmail.com");
                Tim = await _userManager.FindByEmailAsync("tim@gmail.com");
            }
        }

        private async Task CreateUserFollowers()
        {
            var userFollowers = new List<UserFollower>
            {
                new UserFollower
                {
                    UserId = Jim.Id,
                    FollowerId = Tim.Id
                },
                new UserFollower
                {
                    UserId = Jim.Id,
                    FollowerId = Marlene.Id
                },
                new UserFollower
                {
                    UserId = Tim.Id,
                    FollowerId = Marlene.Id
                },
                new UserFollower
                {
                    UserId = Marlene.Id,
                    FollowerId = Jim.Id
                }
            };

            await _context.UserFollowers.AddRangeAsync(userFollowers);
            await _context.SaveChangesAsync();
        }

        private async Task CreateAddresses()
        {
            var addresses = new List<Address>
            {
                new Address
                {
                    UserId = Jim.Id,
                    Street = "4600 W Guadalupe St",
                    Street2 = "806",
                    City = "Austin",
                    State = "TX",
                    ZipCode = "78751",
                    Latitude = "30.314223",
                    Longitude = "-97.732929",
                    Home = true
                },
                new Address
                {
                    UserId = Jim.Id,
                    Street = "6700 Burnet Rd",
                    Street2 = string.Empty,
                    City = "Austin",
                    State = "TX",
                    ZipCode = "78757",
                    Latitude = "30.343087",
                    Longitude = "-97.739128",
                    Home = false
                },
                new Address
                {
                    UserId = Marlene.Id,
                    Street = "6804 N Capital of Texas Hwy",
                    Street2 = "1123",
                    City = "Austin",
                    State = "TX",
                    ZipCode = "78731",
                    Latitude = "30.369964",
                    Longitude = "-97.789853",
                    Home = true
                },
                new Address
                {
                    UserId = Marlene.Id,
                    Street = "6701 Lakewood Dr",
                    Street2 = string.Empty,
                    City = "Austin",
                    State = "TX",
                    ZipCode = "78731",
                    Latitude = "30.368693",
                    Longitude = "-97.784469",
                    Home = false
                },
                new Address
                {
                    UserId = Tim.Id,
                    Street = "2819 Foster Ln",
                    Street2 = "601",
                    City = "Austin",
                    State = "TX",
                    ZipCode = "78757",
                    Latitude = "30.356405",
                    Longitude = "-97.738210",
                    Home = true
                },
                new Address
                {
                    UserId = Tim.Id,
                    Street = "2700 W Anderson Ln",
                    Street2 = string.Empty,
                    City = "Austin",
                    State = "TX",
                    ZipCode = "78757",
                    Latitude = "30.360028",
                    Longitude = "-97.734848",
                    Home = false
                }
            };

            await _context.Addresses.AddRangeAsync(addresses);
            await _context.SaveChangesAsync();

            YardBar = await _context.Addresses.SingleAsync(address =>
                address.Latitude == "30.343087" &&
                address.Longitude == "-97.739128");
            BullCreek = await _context.Addresses.SingleAsync(address =>
                address.Latitude == "30.368693" &&
                address.Longitude == "-97.784469");
            AlamoDrafthouse = await _context.Addresses.SingleAsync(address =>
                address.Latitude == "30.360028" &&
                address.Longitude == "-97.734848");
        }

        private async Task CreateEvents()
        {
            var events = new List<Event>
            {
                new Event
                {
                    UserId = Jim.Id,
                    Name = "Drinks",
                    Details = "Let's have some drinks at the Yard Bar.",
                    Date = DateTime.Now,
                    AddressId = YardBar.Id
                },
                new Event
                {
                    UserId = Marlene.Id,
                    Name = "Softball",
                    Details = "Join in for a game of softball at Bull Creek!",
                    Date = DateTime.Now,
                    AddressId = BullCreek.Id
                },
                new Event
                {
                    UserId = Tim.Id,
                    Name = "Movies",
                    Details = "We're going to watch some movies at the Alamo Drafthouse.",
                    Date = DateTime.Now,
                    AddressId = AlamoDrafthouse.Id
                }
            };

            await _context.Events.AddRangeAsync(events);
            await _context.SaveChangesAsync();

            Drinks = await _context.Events.SingleAsync(@event =>
                @event.Name == "Drinks");
            Softball = await _context.Events.SingleAsync(@event =>
                @event.Name == "Softball");
            Movies = await _context.Events.SingleAsync(@event =>
                @event.Name == "Movies");
        }

        private async Task CreateEventAttenders()
        {
            var eventAttenders = new List<EventAttender>
            {
                new EventAttender
                {
                    AttenderId = Jim.Id,
                    AttendedEventId = Drinks.Id
                },
                new EventAttender
                {
                    AttenderId = Marlene.Id,
                    AttendedEventId = Softball.Id
                },
                new EventAttender
                {
                    AttenderId = Tim.Id,
                    AttendedEventId = Softball.Id
                },
                new EventAttender
                {
                    AttenderId = Tim.Id,
                    AttendedEventId = Movies.Id
                },
                new EventAttender
                {
                    AttenderId = Jim.Id,
                    AttendedEventId = Movies.Id
                }
            };

            await _context.EventAttenders.AddRangeAsync(eventAttenders);
            await _context.SaveChangesAsync();
        }

        private async Task CreateEventFollowers()
        {
            var eventFollowers = new List<EventFollower>
            {
                new EventFollower
                {
                    FollowedEventId = Drinks.Id,
                    FollowerId = Tim.Id
                },
                new EventFollower
                {
                    FollowedEventId = Drinks.Id,
                    FollowerId = Marlene.Id
                },
                new EventFollower
                {
                    FollowedEventId = Movies.Id,
                    FollowerId = Marlene.Id
                },
                new EventFollower
                {
                    FollowedEventId = Softball.Id,
                    FollowerId = Jim.Id
                }
            };

            await _context.EventFollowers.AddRangeAsync(eventFollowers);
            await _context.SaveChangesAsync();
        }
    }
}
