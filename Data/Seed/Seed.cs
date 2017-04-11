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
        private int _adminIntId, _jimIntId, _marleneIntId, _timIntId,
            _yardBarIntId, _bullCreekDistrictParkIntId, _alamoDrafthouseCinemaIntId,
            _drinksIntId, _softballIntId,  _moviesIntId;

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
                user.PasswordHash = _passwordHasher.HashPassword(user, "password");

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                await SeedUserRoles(user);
            }

            _jimIntId = _context.Users.Single(u => u.Email == "jim@gmail.com").IdInt;
            _marleneIntId = _context.Users.Single(u => u.Email == "marlene@gmail.com").IdInt;
            _timIntId = _context.Users.Single(u => u.Email == "tim@gmail.com").IdInt;

            await SeedUserFollowings();
        }

        private async Task SeedAdmin()
        {
            var email = _configuration["AdminEmail"];
            var password = _configuration["AdminPassword"];
            var admin = new ApplicationUser
            {
                Email = email,
                NormalizedEmail = email.ToUpper(),
                EmailConfirmed = true,
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                PhoneNumber = "+12345678901",
                PhoneNumberConfirmed = true
            };
            admin.PasswordHash = _passwordHasher.HashPassword(admin, password);
            _adminIntId = admin.IdInt;

            await _context.Users.AddAsync(admin);
            await _context.SaveChangesAsync();
            await SeedUserRoles(admin);
        }

        private async Task SeedUserRoles(ApplicationUser user)
        {
            var userRoles = new List<IdentityUserRole<string>>();

            foreach (var role in _context.Roles)
                if (role.Name == "Admin")
                {
                    if (user.IdInt == _adminIntId)
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
                    if (user.IdInt == _adminIntId)
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
                    FollowedUserId = _jimIntId,
                    FollowerId = _timIntId
                },
                new ApplicationUserFollowing
                {
                    FollowedUserId = _jimIntId,
                    FollowerId = _marleneIntId
                },
                new ApplicationUserFollowing
                {
                    FollowedUserId = _timIntId,
                    FollowerId = _marleneIntId
                },
                new ApplicationUserFollowing
                {
                    FollowedUserId = _marleneIntId,
                    FollowerId = _jimIntId
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
                    CreatorId = _jimIntId
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
                CreatorId = _marleneIntId
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
                CreatorId = _timIntId
            }));

            await _context.Addresses.AddRangeAsync(addresses);
            await _context.SaveChangesAsync();

            _yardBarIntId = _context.Addresses
                .Single(a => a.Latitude == "30.343087" && a.Longitude == "-97.739128")
                .IdInt;
            _bullCreekDistrictParkIntId = _context.Addresses
                .Single(a => a.Latitude == "30.368693" && a.Longitude == "-97.784469")
                .IdInt;
            _alamoDrafthouseCinemaIntId = _context.Addresses
                .Single(a => a.Latitude == "30.360028" && a.Longitude == "-97.734848")
                .IdInt;
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
                CreatorId = _jimIntId,
                AddressId = _yardBarIntId
            }));
            events.AddRange(Events.MarleneEvents.Select(@event => new Event
            {
                Name = @event["Name"],
                Details = @event["Details"],
                Date = date,
                Time = time,
                CreatorId = _marleneIntId,
                AddressId = _bullCreekDistrictParkIntId
            }));
            events.AddRange(Events.TimEvents.Select(@event => new Event
            {
                Name = @event["Name"],
                Details = @event["Details"],
                Date = date,
                Time = time,
                CreatorId = _timIntId,
                AddressId = _alamoDrafthouseCinemaIntId
            }));

            await _context.Events.AddRangeAsync(events);
            await _context.SaveChangesAsync();

            _drinksIntId = _context.Events.Single(e => e.Name == "Drinks").IdInt;
            _softballIntId = _context.Events.Single(e => e.Name == "Softball").IdInt;
            _moviesIntId = _context.Events.Single(e => e.Name == "Movies").IdInt;

            await SeedEventAttendings();
            await SeedEventFollowings();
        }

        private async Task SeedEventAttendings()
        {
            var eventAttendings = new List<EventAttending>
            {
                new EventAttending
                {
                    AttenderId = _jimIntId,
                    AttendedEventId = _drinksIntId
                },
                new EventAttending
                {
                    AttenderId = _marleneIntId,
                    AttendedEventId = _softballIntId
                },
                new EventAttending
                {
                    AttenderId = _timIntId,
                    AttendedEventId = _softballIntId
                },
                new EventAttending
                {
                    AttenderId = _timIntId,
                    AttendedEventId = _moviesIntId
                },
                new EventAttending
                {
                    AttenderId = _jimIntId,
                    AttendedEventId = _moviesIntId
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
                    FollowedEventId = _drinksIntId,
                    FollowerId = _timIntId
                },
                new EventFollowing
                {
                    FollowedEventId = _drinksIntId,
                    FollowerId = _marleneIntId
                },
                new EventFollowing
                {
                    FollowedEventId = _moviesIntId,
                    FollowerId = _marleneIntId
                },
                new EventFollowing
                {
                    FollowedEventId = _softballIntId,
                    FollowerId = _jimIntId
                }
            };

            await _context.EventFollowings.AddRangeAsync(eventFollowings);
            await _context.SaveChangesAsync();
        }
    }
}