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
            var adminEmail = _configuration["AdminEmail"];
            var adminPassword = _configuration["AdminPassword"];
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    NormalizedEmail = adminEmail.ToUpper(),
                    NormalizedUserName = adminEmail.ToUpper(),
                    PhoneNumber = "+923366633352",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
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
                        await userManager.AddToRolesAsync(user, new[] { "Administrator", "User" });
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
                    Title = "When Harry Met Sally"
                },
                new Event
                {
                    Title = "Transformers"
                },
                new Event
                {
                    Title = "Ghostbusters "
                },
                new Event
                {
                    Title = "Ghostbusters 2"
                },
                new Event
                {
                    Title = "Rio Bravo"
                }
            };
            foreach (var _event in events)
            {
                if (!_context.Events.Any(e => e.Title.Equals(_event.Title)))
                {
                    _context.Events.Add(_event);
                }
            }
            _context.SaveChanges();
        }
    }
}
