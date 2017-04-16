using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Community.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Community.Data.Seed
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _adminEmail, _adminPassword;
        private readonly Data _data;

        public Seed(ApplicationDbContext context, string adminEmail, string adminPassword,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _adminPassword = adminPassword;
            _adminEmail = adminEmail;
            _data = new Data();
        }

        public async Task SeedData()
        {
            if (!_roleManager.Roles.Any()) await CreateRoles();
            if (!_userManager.Users.Any()) await CreateUsers();
            if (!_context.Addresses.Any()) await CreateAddresses();
            if (!_context.Events.Any()) await CreateEvents();
        }

        private async Task CreateRoles()
        {
            using (_roleManager)
            {
                foreach (var role in Data.Roles)
                {
                    await _roleManager.CreateAsync(role);
                    foreach (var claim in Data.Claims.Where(claim => claim.Type == role.Name))
                        await _roleManager.AddClaimAsync(role, claim);
                }
            }
        }

        private async Task CreateUsers()
        {
            var admin = new ApplicationUser {UserName = _adminEmail, Email = _adminEmail};
            var allRoles = Data.Roles.Select(role => role.Name);
            var userClaims = Data.Claims.Where(claim => claim.Type.Equals("User")).ToList();

            using (_userManager)
            {
                await _userManager.CreateAsync(admin, _adminPassword);
                await _userManager.AddToRolesAsync(admin, allRoles);
                await _userManager.AddClaimsAsync(admin, Data.Claims);

                foreach (var user in Data.Users)
                {
                    await _userManager.CreateAsync(user, "@Password1");
                    await _userManager.AddToRoleAsync(user, "User");
                    await _userManager.AddClaimsAsync(user, userClaims);
                }

                _data.Jim = _userManager.FindByEmailAsync("jim@gmail.com").Result;
                _data.Marlene = _userManager.FindByEmailAsync("marlene@gmail.com").Result;
                _data.Tim = _userManager.FindByEmailAsync("tim@gmail.com").Result;
            }
            await CreateUserFollowers();
        }

        private async Task CreateUserFollowers()
        {
            var userFollowers = new List<ApplicationUserFollower>
            {
                new ApplicationUserFollower
                {
                    FollowedUserId = _data.Jim.Id,
                    FollowerId = _data.Tim.Id
                },
                new ApplicationUserFollower
                {
                    FollowedUserId = _data.Jim.Id,
                    FollowerId = _data.Marlene.Id
                },
                new ApplicationUserFollower
                {
                    FollowedUserId = _data.Tim.Id,
                    FollowerId = _data.Marlene.Id
                },
                new ApplicationUserFollower
                {
                    FollowedUserId = _data.Marlene.Id,
                    FollowerId = _data.Jim.Id
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
                    UserId = _data.Jim.Id,
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
                    UserId = _data.Jim.Id,
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
                    UserId = _data.Marlene.Id,
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
                    UserId = _data.Marlene.Id,
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
                    UserId = _data.Tim.Id,
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
                    UserId = _data.Tim.Id,
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

            _data.YardBar = _context.Addresses.Single(address =>
                address.Latitude == "30.343087" &&
                address.Longitude == "-97.739128");
            _data.BullCreek = _context.Addresses.Single(address =>
                address.Latitude == "30.368693" &&
                address.Longitude == "-97.784469");
            _data.AlamoDrafthouse = _context.Addresses.Single(address =>
                address.Latitude == "30.360028" &&
                address.Longitude == "-97.734848");
        }

        private async Task CreateEvents()
        {
            var events = new List<Event>
            {
                new Event
                {
                    UserId = _data.Jim.Id,
                    Name = "Drinks",
                    Details = "Let's have some drinks at the Yard Bar.",
                    Date = DateTime.Now,
                    AddressId = _data.YardBar.Id
                },
                new Event
                {
                    UserId = _data.Marlene.Id,
                    Name = "Softball",
                    Details = "Join in for a game of softball at Bull Creek!",
                    Date = DateTime.Now,
                    AddressId = _data.BullCreek.Id
                },
                new Event
                {
                    UserId = _data.Tim.Id,
                    Name = "Movies",
                    Details = "We're going to watch some movies at the Alamo Drafthouse.",
                    Date = DateTime.Now,
                    AddressId = _data.AlamoDrafthouse.Id
                }
            };

            await _context.Events.AddRangeAsync(events);
            await _context.SaveChangesAsync();

            _data.Drinks = _context.Events.Single(@event =>
                @event.Name == "Drinks");
            _data.Softball = _context.Events.Single(@event =>
                @event.Name == "Softball");
            _data.Movies = _context.Events.Single(@event =>
                @event.Name == "Movies");

            await CreateEventAttenders();
            await CreateEventFollowers();
        }

        private async Task CreateEventAttenders()
        {
            var eventAttenders = new List<EventAttender>
            {
                new EventAttender
                {
                    AttenderId = _data.Jim.Id,
                    AttendedEventId = _data.Drinks.Id
                },
                new EventAttender
                {
                    AttenderId = _data.Marlene.Id,
                    AttendedEventId = _data.Softball.Id
                },
                new EventAttender
                {
                    AttenderId = _data.Tim.Id,
                    AttendedEventId = _data.Softball.Id
                },
                new EventAttender
                {
                    AttenderId = _data.Tim.Id,
                    AttendedEventId = _data.Movies.Id
                },
                new EventAttender
                {
                    AttenderId = _data.Jim.Id,
                    AttendedEventId = _data.Movies.Id
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
                    FollowedEventId = _data.Drinks.Id,
                    FollowerId = _data.Tim.Id
                },
                new EventFollower
                {
                    FollowedEventId = _data.Drinks.Id,
                    FollowerId = _data.Marlene.Id
                },
                new EventFollower
                {
                    FollowedEventId = _data.Movies.Id,
                    FollowerId = _data.Marlene.Id
                },
                new EventFollower
                {
                    FollowedEventId = _data.Softball.Id,
                    FollowerId = _data.Jim.Id
                }
            };

            await _context.EventFollowers.AddRangeAsync(eventFollowers);
            await _context.SaveChangesAsync();
        }
    }
}