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
                .HasKey(p => p.Id)
                .ForSqlServerIsClustered(false);
            address
                .HasAlternateKey(p => p.IdInt);
            address
                .HasIndex(p => p.IdInt)
                .IsUnique();
            address
                .HasOne(p => p.Creator)
                .WithMany(p => p.Addresses)
                .HasForeignKey(p => p.CreatorIdInt)
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
                .HasKey(p => p.Id)
                .ForSqlServerIsClustered(false);
            applicationUser
                .HasAlternateKey(p => p.IdInt);
            applicationUser
                .HasIndex(p => p.IdInt)
                .IsUnique();
            applicationUser
                .HasMany(p => p.AttendedEvents);
            applicationUser
                .HasMany(p => p.FollowedEvents);
        }

        private static void SetupApplicationUserFollowing(ModelBuilder builder)
        {
            var applicationUserFollowing = builder.Entity<ApplicationUserFollowing>();
            applicationUserFollowing
                .HasKey(p => new {p.FollowedUserIdInt, p.FollowerIdInt});
            applicationUserFollowing
                .HasOne(p => p.FollowedUser)
                .WithMany(p => p.Followers)
                .HasForeignKey(p => p.FollowerIdInt)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            applicationUserFollowing
                .HasOne(p => p.Follower)
                .WithMany(p => p.FollowedUsers)
                .HasForeignKey(p => p.FollowedUserIdInt)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetupEvent(ModelBuilder builder)
        {
            var @event = builder.Entity<Event>();
            @event
                .Property(p => p.IdInt)
                .ValueGeneratedOnAdd();
            @event
                .HasKey(p => p.Id)
                .ForSqlServerIsClustered(false);
            @event
                .HasAlternateKey(p => p.IdInt);
            @event
                .HasIndex(p => p.IdInt)
                .IsUnique();
            @event
                .HasOne(p => p.Creator)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.CreatorIdInt)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
            @event
                .HasOne(p => p.Address)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.AddressIdInt)
                .HasPrincipalKey(p => p.IdInt)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetupEventAttending(ModelBuilder builder)
        {
            var eventAttending = builder.Entity<EventAttending>();
            eventAttending
                .HasKey(p => new {p.AttendedEventIdInt, p.AttenderIdInt});
        }

        private static void SetupEventFollowing(ModelBuilder builder)
        {
            var eventFollowing = builder.Entity<EventFollowing>();
            eventFollowing
                .HasKey(p => new {p.FollowedEventIdInt, p.FollowerIdInt});
        }
    }
}