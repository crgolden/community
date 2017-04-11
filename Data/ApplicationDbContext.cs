using Community.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Community.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<ApplicationUserFollowing> UserFollowings { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAttending> EventAttendings { get; set; }
        public DbSet<EventFollowing> EventFollowings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SetupAddress(builder);
            SetupApplicationUser(builder);
            SetupApplicationUserFollowing(builder);
            SetupEvent(builder);
            SetupEventAttending(builder);
            SetupEventFollowing(builder);
        }

        private static void SetupAddress(ModelBuilder builder)
        {
            var address = builder.Entity<Address>();
            address
                .Property(p => p.IdInt)
                .ValueGeneratedOnAdd();
            address
                .HasKey(p => p.IdInt);
            address
                .HasKey(p => p.Id)
                .ForSqlServerIsClustered(false);
            address
                .HasOne(p => p.Creator)
                .WithMany(p => p.Addresses)
                .HasForeignKey(p => p.CreatorId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            address
                .HasMany(p => p.Events)
                .WithOne(p => p.Address)
                .HasForeignKey(p => p.AddressId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetupApplicationUser(ModelBuilder builder)
        {
            var applicationUser = builder.Entity<ApplicationUser>();
            applicationUser
                .Property(p => p.IdInt)
                .ValueGeneratedOnAdd();
            applicationUser
                .HasKey(p => p.IdInt);
            applicationUser
                .HasKey(p => p.Id)
                .ForSqlServerIsClustered(false);
            applicationUser
                .HasMany(p => p.Addresses)
                .WithOne(p => p.Creator)
                .HasForeignKey(p => p.CreatorId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            applicationUser
                .HasMany(p => p.Events)
                .WithOne(p => p.Creator)
                .HasForeignKey(p => p.CreatorId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            applicationUser
                .HasMany(p => p.AttendedEvents)
                .WithOne(p => p.Attender)
                .HasForeignKey(p => p.AttendedEventId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            applicationUser
                .HasMany(p => p.FollowedEvents)
                .WithOne(p => p.Follower)
                .HasForeignKey(p => p.FollowedEventId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            applicationUser
                .HasMany(p => p.FollowedUsers)
                .WithOne(p => p.Follower)
                .HasForeignKey(p => p.FollowedUserId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            applicationUser
                .HasMany(p => p.Followers)
                .WithOne(p => p.FollowedUser)
                .HasForeignKey(p => p.FollowerId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetupApplicationUserFollowing(ModelBuilder builder)
        {
            var applicationUserFollowing = builder.Entity<ApplicationUserFollowing>();
            applicationUserFollowing
                .HasKey(p => new {p.FollowedUserId, p.FollowerId});
        }

        private static void SetupEvent(ModelBuilder builder)
        {
            var @event = builder.Entity<Event>();
            @event
                .Property(p => p.IdInt)
                .ValueGeneratedOnAdd();
            @event
                .HasKey(p => p.IdInt);
            @event
                .HasKey(p => p.Id)
                .ForSqlServerIsClustered(false);
            @event
                .HasOne(p => p.Creator)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.CreatorId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            @event
                .HasOne(p => p.Address)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.AddressId)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            @event
                .HasMany(p => p.Attenders);
            @event
                .HasMany(p => p.Followers);
        }

        private static void SetupEventAttending(ModelBuilder builder)
        {
            var eventAttending = builder.Entity<EventAttending>();
            eventAttending
                .HasKey(p => new {p.AttendedEventId, p.AttenderId});
        }

        private static void SetupEventFollowing(ModelBuilder builder)
        {
            var eventFollowing = builder.Entity<EventFollowing>();
            eventFollowing
                .HasKey(p => new {p.FollowedEventId, p.FollowerId});
        }
    }
}