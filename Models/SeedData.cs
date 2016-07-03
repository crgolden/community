using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Community.Data;
using System;
using System.Linq;

namespace Community.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.Event.Any())
                {
                    return;   // DB has been seeded
                }

                context.Event.AddRange(
                     new Event
                     {
                         Title = "When Harry Met Sally",
                         Date = DateTime.Parse("1989-1-11"),
                         Location = "Rome",
                         Price = 7.99M,
                         Creator = "Jim"
                     },

                     new Event
                     {
                         Title = "Transformers",
                         Date = DateTime.Parse("2000-1-11"),
                         Location = "Texas",
                         Price = 7.99M,
                         Creator = "Jennifer"
                     },

                     new Event
                     {
                         Title = "Ghostbusters ",
                         Date = DateTime.Parse("1984-3-13"),
                         Location = "New York",
                         Price = 8.99M,
                         Creator = "Sam"
                     },

                     new Event
                     {
                         Title = "Ghostbusters 2",
                         Date = DateTime.Parse("1986-2-23"),
                         Location = "Los Angeles",
                         Price = 9.99M,
                         Creator = "Tim"
                     },

                   new Event
                   {
                       Title = "Rio Bravo",
                       Date = DateTime.Parse("1959-4-15"),
                       Location = "Denver",
                       Price = 3.99M,
                       Creator = "Luke"
                   }
                );
                context.SaveChanges();
            }
        }
    }
}
