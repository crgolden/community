using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using community.Data;
using community.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace community.Extensions
{
    public static class SeedExtensions
    {
        public static async Task CreateRolesAsync(this Seed seed)
        {
            using (seed.RoleManager)
            {
                foreach (var role in Seed.Roles)
                {
                    await seed.RoleManager.CreateAsync(role);
                    foreach (var claim in Seed.Claims.Where(claim => claim.Type == role.Name))
                        await seed.RoleManager.AddClaimAsync(role, claim);
                }
            }
        }

        public static async Task CreateUsersAsync(this Seed seed, string adminEmail, string adminPassword)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                FirstName = "Jack",
                LastName = "Smith",
                Email = adminEmail,
                SecurityStamp = $"{Guid.NewGuid()}"
            };
            using (seed.UserManager)
            {
                await seed.UserManager.CreateAsync(admin, adminPassword);
                await seed.UserManager.AddToRolesAsync(admin, Seed.Roles.Select(role => role.Name));
                await seed.UserManager.AddClaimsAsync(admin, Seed.Claims);

                foreach (var user in Seed.Users)
                {
                    await seed.UserManager.CreateAsync(user, "@Password1");
                    await seed.UserManager.AddToRoleAsync(user, "User");
                    await seed.UserManager.AddClaimsAsync(user, Seed.Claims.Where(claim =>
                        claim.Type.Equals("User")));
                }
                seed.Jim = await seed.UserManager.FindByEmailAsync("jim@gmail.com");
                seed.Marlene = await seed.UserManager.FindByEmailAsync("marlene@gmail.com");
                seed.Tim = await seed.UserManager.FindByEmailAsync("tim@gmail.com");
            }
        }

        public static async Task CreateUserFollowers(this Seed seed)
        {
            var userFollowers = new List<ApplicationUserFollower>
            {
                new ApplicationUserFollower
                {
                    FollowedUserId = seed.Jim.Id,
                    FollowerId = seed.Tim.Id
                },
                new ApplicationUserFollower
                {
                    FollowedUserId = seed.Jim.Id,
                    FollowerId = seed.Marlene.Id
                },
                new ApplicationUserFollower
                {
                    FollowedUserId = seed.Tim.Id,
                    FollowerId = seed.Marlene.Id
                },
                new ApplicationUserFollower
                {
                    FollowedUserId = seed.Marlene.Id,
                    FollowerId = seed.Jim.Id
                }
            };

            await seed.Context.UserFollowers.AddRangeAsync(userFollowers);
            await seed.Context.SaveChangesAsync();
        }

        public static async Task CreateAddresses(this Seed seed)
        {
            var addresses = new List<Address>
            {
                new Address
                {
                    UserId = seed.Jim.Id,
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
                    UserId = seed.Jim.Id,
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
                    UserId = seed.Marlene.Id,
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
                    UserId = seed.Marlene.Id,
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
                    UserId = seed.Tim.Id,
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
                    UserId = seed.Tim.Id,
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

            await seed.Context.Addresses.AddRangeAsync(addresses);
            await seed.Context.SaveChangesAsync();

            seed.YardBar = await seed.Context.Addresses.SingleAsync(address =>
                address.Latitude == "30.343087" &&
                address.Longitude == "-97.739128");
            seed.BullCreek = await seed.Context.Addresses.SingleAsync(address =>
                address.Latitude == "30.368693" &&
                address.Longitude == "-97.784469");
            seed.AlamoDrafthouse = await seed.Context.Addresses.SingleAsync(address =>
                address.Latitude == "30.360028" &&
                address.Longitude == "-97.734848");
        }

        public static async Task CreateEvents(this Seed seed)
        {
            var events = new List<Event>
            {
                new Event
                {
                    UserId = seed.Jim.Id,
                    Name = "Drinks",
                    Details = "Let's have some drinks at the Yard Bar.",
                    Date = DateTime.Now,
                    AddressId = seed.YardBar.Id
                },
                new Event
                {
                    UserId = seed.Marlene.Id,
                    Name = "Softball",
                    Details = "Join in for a game of softball at Bull Creek!",
                    Date = DateTime.Now,
                    AddressId = seed.BullCreek.Id
                },
                new Event
                {
                    UserId = seed.Tim.Id,
                    Name = "Movies",
                    Details = "We're going to watch some movies at the Alamo Drafthouse.",
                    Date = DateTime.Now,
                    AddressId = seed.AlamoDrafthouse.Id
                }
            };

            await seed.Context.Events.AddRangeAsync(events);
            await seed.Context.SaveChangesAsync();

            seed.Drinks = await seed.Context.Events.SingleAsync(@event =>
                @event.Name == "Drinks");
            seed.Softball = await seed.Context.Events.SingleAsync(@event =>
                @event.Name == "Softball");
            seed.Movies = await seed.Context.Events.SingleAsync(@event =>
                @event.Name == "Movies");
        }

        public static async Task CreateEventAttenders(this Seed seed)
        {
            var eventAttenders = new List<EventAttender>
            {
                new EventAttender
                {
                    AttenderId = seed.Jim.Id,
                    AttendedEventId = seed.Drinks.Id
                },
                new EventAttender
                {
                    AttenderId = seed.Marlene.Id,
                    AttendedEventId = seed.Softball.Id
                },
                new EventAttender
                {
                    AttenderId = seed.Tim.Id,
                    AttendedEventId = seed.Softball.Id
                },
                new EventAttender
                {
                    AttenderId = seed.Tim.Id,
                    AttendedEventId = seed.Movies.Id
                },
                new EventAttender
                {
                    AttenderId = seed.Jim.Id,
                    AttendedEventId = seed.Movies.Id
                }
            };

            await seed.Context.EventAttenders.AddRangeAsync(eventAttenders);
            await seed.Context.SaveChangesAsync();
        }

        public static async Task CreateEventFollowers(this Seed seed)
        {
            var eventFollowers = new List<EventFollower>
            {
                new EventFollower
                {
                    FollowedEventId = seed.Drinks.Id,
                    FollowerId = seed.Tim.Id
                },
                new EventFollower
                {
                    FollowedEventId = seed.Drinks.Id,
                    FollowerId = seed.Marlene.Id
                },
                new EventFollower
                {
                    FollowedEventId = seed.Movies.Id,
                    FollowerId = seed.Marlene.Id
                },
                new EventFollower
                {
                    FollowedEventId = seed.Softball.Id,
                    FollowerId = seed.Jim.Id
                }
            };

            await seed.Context.EventFollowers.AddRangeAsync(eventFollowers);
            await seed.Context.SaveChangesAsync();
        }
    }
}
