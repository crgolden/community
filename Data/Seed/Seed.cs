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

            await SeedUserFollowers();
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
                    if (user.Index == _admin.Index)
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
                    if (user.Index == _admin.Index)
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

        private async Task SeedUserFollowers()
        {
            var userFollowers = new List<ApplicationUserFollower>
            {
                new ApplicationUserFollower
                {
                    UserIndex = _jim.Index,
                    FollowerIndex = _tim.Index
                },
                new ApplicationUserFollower
                {
                    UserIndex = _jim.Index,
                    FollowerIndex = _marlene.Index
                },
                new ApplicationUserFollower
                {
                    UserIndex = _tim.Index,
                    FollowerIndex = _marlene.Index
                },
                new ApplicationUserFollower
                {
                    UserIndex = _marlene.Index,
                    FollowerIndex = _jim.Index
                }
            };

            await _context.UserFollowers.AddRangeAsync(userFollowers);
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
                    CreatorIndex = _jim.Index
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
                CreatorIndex = _marlene.Index
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
                CreatorIndex = _tim.Index
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
                CreatorIndex = _jim.Index,
                AddressIndex = _yardBar.Index
            }));
            events.AddRange(Events.MarleneEvents.Select(@event => new Event
            {
                Name = @event["Name"],
                Details = @event["Details"],
                Date = date,
                Time = time,
                CreatorIndex = _marlene.Index,
                AddressIndex = _bullCreekDistrictPark.Index
            }));
            events.AddRange(Events.TimEvents.Select(@event => new Event
            {
                Name = @event["Name"],
                Details = @event["Details"],
                Date = date,
                Time = time,
                CreatorIndex = _tim.Index,
                AddressIndex = _alamoDrafthouseCinema.Index
            }));

            await _context.Events.AddRangeAsync(events);
            await _context.SaveChangesAsync();

            _drinks = _context.Events.Single(e => e.Name == "Drinks");
            _softball = _context.Events.Single(e => e.Name == "Softball");
            _movies = _context.Events.Single(e => e.Name == "Movies");

            await SeedEventAttenders();
            await SeedEventFollowers();
        }

        private async Task SeedEventAttenders()
        {
            var eventAttenders = new List<EventAttender>
            {
                new EventAttender
                {
                    AttenderId = _jim.Id,
                    AttenderIndex = _jim.Index,
                    EventId = _drinks.Id,
                    EventIndex = _drinks.Index
                },
                new EventAttender
                {
                    AttenderId = _marlene.Id,
                    AttenderIndex = _marlene.Index,
                    EventId = _softball.Id,
                    EventIndex = _softball.Index
                },
                new EventAttender
                {
                    AttenderId = _tim.Id,
                    AttenderIndex = _tim.Index,
                    EventId = _softball.Id,
                    EventIndex = _softball.Index
                },
                new EventAttender
                {
                    AttenderId = _tim.Id,
                    AttenderIndex = _tim.Index,
                    EventId = _movies.Id,
                    EventIndex = _movies.Index
                },
                new EventAttender
                {
                    AttenderId = _jim.Id,
                    AttenderIndex = _jim.Index,
                    EventId = _movies.Id,
                    EventIndex = _movies.Index
                }
            };

            await _context.EventAttenders.AddRangeAsync(eventAttenders);
            await _context.SaveChangesAsync();
        }

        private async Task SeedEventFollowers()
        {
            var eventFollowers = new List<EventFollower>
            {
                new EventFollower
                {
                    EventId = _drinks.Id,
                    EventIndex = _drinks.Index,
                    FollowerId = _tim.Id,
                    FollowerIndex = _tim.Index
                },
                new EventFollower
                {
                    EventId = _drinks.Id,
                    EventIndex = _drinks.Index,
                    FollowerId = _marlene.Id,
                    FollowerIndex = _marlene.Index
                },
                new EventFollower
                {
                    EventId = _movies.Id,
                    EventIndex = _movies.Index,
                    FollowerId = _marlene.Id,
                    FollowerIndex = _marlene.Index
                },
                new EventFollower
                {
                    EventId = _softball.Id,
                    EventIndex = _softball.Index,
                    FollowerId = _jim.Id,
                    FollowerIndex = _jim.Index
                }
            };

            await _context.EventFollowers.AddRangeAsync(eventFollowers);
            await _context.SaveChangesAsync();
        }
    }
}