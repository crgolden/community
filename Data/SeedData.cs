using Community.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Community.Data
{
    public class SeedData
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfigurationRoot _configuration;
        public SeedData(ApplicationDbContext context, IConfigurationRoot configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task SeedRoles()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                }
            };
            foreach (var role in roles)
            {
                if (!_context.Roles.Any(r => r.Name.Equals(role.Name)))
                {
                    await roleStore.CreateAsync(role);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task SeedUsers(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = _configuration["AppKeys:AdminEmail"];
            var adminPassword = _configuration["AppKeys:AdminPassword"];
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Email = adminEmail,
                    UserName = adminEmail
                },
                new ApplicationUser()
                {
                    Email = "abdullahnaseer999@gmail.com",
                    UserName = "abdullahnaseer999@gmail.com"
                },
                new ApplicationUser()
                {
                    Email = "jim@gmail.com",
                    UserName = "jim@gmail.com"
                },
                new ApplicationUser()
                {
                    Email = "marlene@gmail.com",
                    UserName = "marlene@gmail.com"
                },
                new ApplicationUser()
                {
                    Email = "tim@gmail.com",
                    UserName = "tim@gmail.com"
                }
            };

            // Build users
            foreach (var user in users)
            {
                if (!_context.Users.Any(u => u.Email.Equals(user.Email)))
                {
                    var passwordHasher = new PasswordHasher<ApplicationUser>();
                    if (!user.Email.Equals(adminEmail))
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, "secret");
                        await userManager.CreateAsync(user);
                        await userManager.AddToRoleAsync(user, "User");
                    }
                    else
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, adminPassword);
                        await userManager.CreateAsync(user);
                        await userManager.AddClaimAsync(user, new Claim("CanEdit", "true"));
                        await userManager.AddToRolesAsync(user, new string[] { "Administrator", "User" });
                    }
                }
            }
            await _context.SaveChangesAsync();
        }
        public void SeedEvents()
        {
            var events = new List<Event>()
            {
                new Event
                {
                    Title = "When Harry Met Sally",
                    Date = DateTime.Parse("1989-1-11"),
                    Location = "Rome",
                    Price = 7.99M
                },
                new Event
                {
                    Title = "Transformers",
                    Date = DateTime.Parse("2000-1-11"),
                    Location = "Texas",
                    Price = 7.99M
                },
                new Event
                {
                    Title = "Ghostbusters ",
                    Date = DateTime.Parse("1984-3-13"),
                    Location = "New York",
                    Price = 8.99M
                },
                new Event
                {
                    Title = "Ghostbusters 2",
                    Date = DateTime.Parse("1986-2-23"),
                    Location = "Los Angeles",
                    Price = 9.99M
                },
                new Event
                {
                    Title = "Rio Bravo",
                    Date = DateTime.Parse("1959-4-15"),
                    Location = "Denver",
                    Price = 3.99M
                }
            };
            foreach (var _event in events)
            {
                _context.Events.Add(_event);
            }
            _context.SaveChanges();
        }
    }
}
