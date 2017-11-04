using community.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace community.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<ApplicationUserFollower> UserFollowers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAttender> EventAttenders { get; set; }
        public DbSet<EventFollower> EventFollowers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasMany(p => p.Followers);
            builder.Entity<Event>()
                .HasMany(p => p.Attenders);
            builder.Entity<Event>()
                .HasMany(p => p.Followers);
            builder.Entity<ApplicationUserFollower>()
                .HasKey(p => new {p.FollowedUserId, p.FollowerId});
            builder.Entity<ApplicationUserFollower>()
                .HasOne(p => p.FollowedUser)
                .WithMany(p => p.Followers)
                .HasForeignKey(p => p.FollowedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ApplicationUserFollower>()
                .HasOne(p => p.Follower)
                .WithMany(p => p.FollowedUsers)
                .HasForeignKey(p => p.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<EventAttender>()
                .HasKey(p => new {p.AttendedEventId, p.AttenderId});
            builder.Entity<EventAttender>()
                .HasOne(p => p.AttendedEvent)
                .WithMany(p => p.Attenders)
                .HasForeignKey(p => p.AttendedEventId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<EventAttender>()
                .HasOne(p => p.Attender)
                .WithMany(p => p.AttendedEvents)
                .HasForeignKey(p => p.AttenderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<EventFollower>()
                .HasKey(p => new {p.FollowedEventId, p.FollowerId});
            builder.Entity<EventFollower>()
                .HasOne(p => p.FollowedEvent)
                .WithMany(p => p.Followers)
                .HasForeignKey(p => p.FollowedEventId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<EventFollower>()
                .HasOne(p => p.Follower)
                .WithMany(p => p.FollowedEvents)
                .HasForeignKey(p => p.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}