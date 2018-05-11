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
        public readonly ApplicationDbContext Context;
        public readonly UserManager<User> UserManager;
        public readonly RoleManager<Role> RoleManager;
        public User Jim { get; set; }
        public User Marlene { get; set; }
        public User Tim { get; set; }
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

        public static IEnumerable<User> Users => new List<User>
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

        public Seed(ApplicationDbContext context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            Context = context;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public async Task SeedData(string adminEmail, string adminPassword)
        {
            if (!await RoleManager.Roles.AnyAsync()) await this.CreateRolesAsync();
            if (!await UserManager.Users.AnyAsync()) await this.CreateUsersAsync(adminEmail, adminPassword);
            if (!await Context.UserFollowers.AnyAsync()) await this.CreateUserFollowers();
            if (!await Context.Addresses.AnyAsync()) await this.CreateAddresses();
            if (!await Context.Events.AnyAsync()) await this.CreateEvents();
            if (!await Context.EventAttenders.AnyAsync()) await this.CreateEventAttenders();
            if (!await Context.EventFollowers.AnyAsync()) await this.CreateEventFollowers();
        }

        public async Task CreateRolesAsync()
        {
            using (RoleManager)
            {
                foreach (var role in Seed.Roles)
                {
                    await RoleManager.CreateAsync(role);
                    foreach (var claim in Seed.Claims.Where(claim => claim.Type == role.Name))
                        await RoleManager.AddClaimAsync(role, claim);
                }
            }
        }

        public async Task CreateUsersAsync(string adminEmail, string adminPassword)
        {
            var admin = new User
            {
                UserName = adminEmail,
                FirstName = "Jack",
                LastName = "Smith",
                Email = adminEmail,
                SecurityStamp = $"{Guid.NewGuid()}"
            };
            using (UserManager)
            {
                await UserManager.CreateAsync(admin, adminPassword);
                await UserManager.AddToRolesAsync(admin, Seed.Roles.Select(role => role.Name));
                await UserManager.AddClaimsAsync(admin, Seed.Claims);

                foreach (var user in Users)
                {
                    await UserManager.CreateAsync(user, "@Password1");
                    await UserManager.AddToRoleAsync(user, "User");
                    await UserManager.AddClaimsAsync(user, Seed.Claims.Where(claim =>
                        claim.Type.Equals("User")));
                }
                Jim = await UserManager.FindByEmailAsync("jim@gmail.com");
                Marlene = await UserManager.FindByEmailAsync("marlene@gmail.com");
                Tim = await UserManager.FindByEmailAsync("tim@gmail.com");
            }
        }

        public async Task CreateUserFollowers()
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

            await Context.UserFollowers.AddRangeAsync(userFollowers);
            await Context.SaveChangesAsync();
        }

        public async Task CreateAddresses()
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

            await Context.Addresses.AddRangeAsync(addresses);
            await Context.SaveChangesAsync();

            YardBar = await Context.Addresses.SingleAsync(address =>
                address.Latitude == "30.343087" &&
                address.Longitude == "-97.739128");
            BullCreek = await Context.Addresses.SingleAsync(address =>
                address.Latitude == "30.368693" &&
                address.Longitude == "-97.784469");
            AlamoDrafthouse = await Context.Addresses.SingleAsync(address =>
                address.Latitude == "30.360028" &&
                address.Longitude == "-97.734848");
        }

        public async Task CreateEvents()
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

            await Context.Events.AddRangeAsync(events);
            await Context.SaveChangesAsync();

            Drinks = await Context.Events.SingleAsync(@event =>
                @event.Name == "Drinks");
            Softball = await Context.Events.SingleAsync(@event =>
                @event.Name == "Softball");
            Movies = await Context.Events.SingleAsync(@event =>
                @event.Name == "Movies");
        }

        public async Task CreateEventAttenders()
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

            await Context.EventAttenders.AddRangeAsync(eventAttenders);
            await Context.SaveChangesAsync();
        }

        public async Task CreateEventFollowers()
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

            await Context.EventFollowers.AddRangeAsync(eventFollowers);
            await Context.SaveChangesAsync();
        }
    }
}