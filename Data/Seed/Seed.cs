using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Community.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Community.Data.Seed
{
    public class Seed
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationRoot _configuration;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher;
        private ApplicationUser _admin, _jim, _marlene, _tim;
        private Address _yardBar, _bullCreekDistrictPark, _alamoDrafthouseCinema;
        private Event _drinks, _softball, _movies;

        public Seed(ApplicationDbContext context, IConfigurationRoot configuration)
        {
            _context = context;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<ApplicationUser>();
        }

        public async Task SeedData()
        {
            _context.Database.EnsureCreated();

            if (!_context.Roles.Any()) await SeedRoles();
            if (!_context.Users.Any()) await SeedUsers();
            if (!_context.Addresses.Any()) await SeedAddresses();
            if (!_context.Events.Any()) await SeedEvents();
        }

        private async Task SeedRoles()
        {
            var roles = Roles.Names.Select(name => new IdentityRole
                {
                    Name = name,
                    NormalizedName = name.ToUpper()
                })
                .ToList();

            await _context.Roles.AddRangeAsync(roles);
            await _context.SaveChangesAsync();
            await SeedRoleClaims();
        }

        private async Task SeedRoleClaims()
        {
            var roleClaims = new List<IdentityRoleClaim<string>>();

            roleClaims.AddRange(RoleClaims.ValuesAdmin.Select(value =>
                new IdentityRoleClaim<string>
                {
                    ClaimType = "Admin",
                    ClaimValue = value,
                    RoleId = _context.Roles.Single(r => r.Name == "Admin").Id
                }));
            roleClaims.AddRange(RoleClaims.ValuesUser.Select(value =>
                new IdentityRoleClaim<string>
                {
                    ClaimType = "User",
                    ClaimValue = value,
                    RoleId = _context.Roles.Single(r => r.Name == "User").Id
                }));

            await _context.RoleClaims.AddRangeAsync(roleClaims);
            await _context.SaveChangesAsync();
        }

        private async Task SeedUsers()
        {
            await SeedAdmin();

            foreach (var name in Users.Names)
            {
                var user = new ApplicationUser
                {
                    Email = name,
                    NormalizedEmail = name.ToUpper(),
                    UserName = name,
                    EmailConfirmed = false,
                    NormalizedUserName = name.ToUpper(),
                    PhoneNumber = string.Empty,
                    PhoneNumberConfirmed = false
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                user.PasswordHash = _passwordHasher.HashPassword(user, "password");

                await SeedUserRoles(user);
            }

            _jim = _context.Users.Single(u => u.Email == "jim@gmail.com");
            _marlene = _context.Users.Single(u => u.Email == "marlene@gmail.com");
            _tim = _context.Users.Single(u => u.Email == "tim@gmail.com");

            await SeedUserFollowings();
        }

        private async Task SeedAdmin()
        {
            var email = _configuration["AdminEmail"];
            var password = _configuration["AdminPassword"];
            _admin = new ApplicationUser
            {
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                PhoneNumber = "+12345678901",
                PhoneNumberConfirmed = true
            };

            await _context.Users.AddAsync(_admin);
            await _context.SaveChangesAsync();

            _admin.PasswordHash = _passwordHasher.HashPassword(_admin, password);

            await SeedUserRoles(_admin);
        }

        private async Task SeedUserRoles(ApplicationUser user)
        {
            var userRoles = new List<IdentityUserRole<string>>();

            foreach (var role in _context.Roles)
                if (role.Name == "Admin")
                {
                    if (user.IdInt == _admin.IdInt)
                        userRoles.Add(new IdentityUserRole<string>
                        {
                            RoleId = role.Id,
                            UserId = user.Id
                        });
                }
                else
                {
                    userRoles.Add(new IdentityUserRole<string>
                    {
                        RoleId = role.Id,
                        UserId = user.Id
                    });
                }

            await _context.UserRoles.AddRangeAsync(userRoles);
            await _context.SaveChangesAsync();
            await SeedUserClaims(user);
        }

        private async Task SeedUserClaims(ApplicationUser user)
        {
            var userClaims = new List<IdentityUserClaim<string>>();

            foreach (var claim in _context.RoleClaims)
                if (claim.ClaimType == "Admin")
                {
                    if (user.IdInt == _admin.IdInt)
                        userClaims.Add(new IdentityUserClaim<string>
                        {
                            ClaimType = claim.ClaimType,
                            ClaimValue = claim.ClaimValue,
                            UserId = user.Id
                        });
                }
                else
                {
                    userClaims.Add(new IdentityUserClaim<string>
                    {
                        ClaimType = claim.ClaimType,
                        ClaimValue = claim.ClaimValue,
                        UserId = user.Id
                    });
                }

            await _context.UserClaims.AddRangeAsync(userClaims);
            await _context.SaveChangesAsync();
        }

        private async Task SeedUserFollowings()
        {
            var userFollowings = new List<ApplicationUserFollowing>
            {
                new ApplicationUserFollowing
                {
                    FollowedUserIdInt = _jim.IdInt,
                    FollowerIdInt = _tim.IdInt
                },
                new ApplicationUserFollowing
                {
                    FollowedUserIdInt = _jim.IdInt,
                    FollowerIdInt = _marlene.IdInt
                },
                new ApplicationUserFollowing
                {
                    FollowedUserIdInt = _tim.IdInt,
                    FollowerIdInt = _marlene.IdInt
                },
                new ApplicationUserFollowing
                {
                    FollowedUserIdInt = _marlene.IdInt,
                    FollowerIdInt = _jim.IdInt
                }
            };

            await _context.UserFollowings.AddRangeAsync(userFollowings);
            await _context.SaveChangesAsync();
        }

        private async Task SeedAddresses()
        {
            var addresses = new List<Address>();

            addresses.AddRange(Addresses.JimAddresses.Select(address => new Address
                {
                    Street = address["Street"],
                    Street2 = address["Street2"],
                    City = address["City"],
                    State = address["State"],
                    ZipCode = address["ZipCode"],
                    Latitude = address["Latitude"],
                    Longitude = address["Longitude"],
                    Home = bool.Parse(address["Home"]),
                    CreatorIdInt = _jim.IdInt
                }));
            addresses.AddRange(Addresses.MarleneAddresses.Select(address => new Address
            {
                Street = address["Street"],
                Street2 = address["Street2"],
                City = address["City"],
                State = address["State"],
                ZipCode = address["ZipCode"],
                Latitude = address["Latitude"],
                Longitude = address["Longitude"],
                Home = bool.Parse(address["Home"]),
                CreatorIdInt = _marlene.IdInt
            }));
            addresses.AddRange(Addresses.TimAddresses.Select(address => new Address
            {
                Street = address["Street"],
                Street2 = address["Street2"],
                City = address["City"],
                State = address["State"],
                ZipCode = address["ZipCode"],
                Latitude = address["Latitude"],
                Longitude = address["Longitude"],
                Home = bool.Parse(address["Home"]),
                CreatorIdInt = _tim.IdInt
            }));

            await _context.Addresses.AddRangeAsync(addresses);
            await _context.SaveChangesAsync();

            _yardBar = _context.Addresses
                .Single(a => a.Latitude == "30.343087" && a.Longitude == "-97.739128");
            _bullCreekDistrictPark = _context.Addresses
                .Single(a => a.Latitude == "30.368693" && a.Longitude == "-97.784469");
            _alamoDrafthouseCinema = _context.Addresses
                .Single(a => a.Latitude == "30.360028" && a.Longitude == "-97.734848");
        }

        private async Task SeedEvents()
        {
            var dateTime = DateTime.Now;
            var date = dateTime.Date.ToString(CultureInfo.InvariantCulture);
            var time = dateTime.TimeOfDay.ToString();
            var events = new List<Event>();

            events.AddRange(Events.JimEvents.Select(@event => new Event
            {
                Name = @event["Name"],
                Details = @event["Details"],
                Date = date,
                Time = time,
                CreatorIdInt = _jim.IdInt,
                AddressIdInt = _yardBar.IdInt
            }));
            events.AddRange(Events.MarleneEvents.Select(@event => new Event
            {
                Name = @event["Name"],
                Details = @event["Details"],
                Date = date,
                Time = time,
                CreatorIdInt = _marlene.IdInt,
                AddressIdInt = _bullCreekDistrictPark.IdInt
            }));
            events.AddRange(Events.TimEvents.Select(@event => new Event
            {
                Name = @event["Name"],
                Details = @event["Details"],
                Date = date,
                Time = time,
                CreatorIdInt = _tim.IdInt,
                AddressIdInt = _alamoDrafthouseCinema.IdInt
            }));

            await _context.Events.AddRangeAsync(events);
            await _context.SaveChangesAsync();

            _drinks = _context.Events.Single(e => e.Name == "Drinks");
            _softball = _context.Events.Single(e => e.Name == "Softball");
            _movies = _context.Events.Single(e => e.Name == "Movies");

            await SeedEventAttendings();
            await SeedEventFollowings();
        }

        private async Task SeedEventAttendings()
        {
            var eventAttendings = new List<EventAttending>
            {
                new EventAttending
                {
                    AttenderId = _jim.Id,
                    AttenderIdInt = _jim.IdInt,
                    AttendedEventId = _drinks.Id,
                    AttendedEventIdInt = _drinks.IdInt
                },
                new EventAttending
                {
                    AttenderId = _marlene.Id,
                    AttenderIdInt = _marlene.IdInt,
                    AttendedEventId = _softball.Id,
                    AttendedEventIdInt = _softball.IdInt
                },
                new EventAttending
                {
                    AttenderId = _tim.Id,
                    AttenderIdInt = _tim.IdInt,
                    AttendedEventId = _softball.Id,
                    AttendedEventIdInt = _softball.IdInt
                },
                new EventAttending
                {
                    AttenderId = _tim.Id,
                    AttenderIdInt = _tim.IdInt,
                    AttendedEventId = _movies.Id,
                    AttendedEventIdInt = _movies.IdInt
                },
                new EventAttending
                {
                    AttenderId = _jim.Id,
                    AttenderIdInt = _jim.IdInt,
                    AttendedEventId = _movies.Id,
                    AttendedEventIdInt = _movies.IdInt
                }
            };

            await _context.EventAttendings.AddRangeAsync(eventAttendings);
            await _context.SaveChangesAsync();
        }

        private async Task SeedEventFollowings()
        {
            var eventFollowings = new List<EventFollowing>
            {
                new EventFollowing
                {
                    FollowedEventId = _drinks.Id,
                    FollowedEventIdInt = _drinks.IdInt,
                    FollowerId = _tim.Id,
                    FollowerIdInt = _tim.IdInt
                },
                new EventFollowing
                {
                    FollowedEventId = _drinks.Id,
                    FollowedEventIdInt = _drinks.IdInt,
                    FollowerId = _marlene.Id,
                    FollowerIdInt = _marlene.IdInt
                },
                new EventFollowing
                {
                    FollowedEventId = _movies.Id,
                    FollowedEventIdInt = _movies.IdInt,
                    FollowerId = _marlene.Id,
                    FollowerIdInt = _marlene.IdInt
                },
                new EventFollowing
                {
                    FollowedEventId = _softball.Id,
                    FollowedEventIdInt = _softball.IdInt,
                    FollowerId = _jim.Id,
                    FollowerIdInt = _jim.IdInt
                }
            };

            await _context.EventFollowings.AddRangeAsync(eventFollowings);
            await _context.SaveChangesAsync();
        }
    }
}