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
        public DbSet<ApplicationUserFollower> UserFollowers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAttender> EventAttenders { get; set; }
        public DbSet<EventFollower> EventFollowers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SetupAddress(builder);
            SetupApplicationUser(builder);
            SetupApplicationUserFollower(builder);
            SetupEvent(builder);
            SetupEventAttender(builder);
            SetupEventFollower(builder);
        }

        private static void SetupAddress(ModelBuilder builder)
        {
            var address = builder.Entity<Address>();
            address
                .Property(p => p.Index)
                .ValueGeneratedOnAdd();
            address
                .HasKey(p => p.Id)
                .ForSqlServerIsClustered(false);
            address
                .HasAlternateKey(p => p.Index);
            address
                .HasIndex(p => p.Index)
                .IsUnique();
            address
                .HasOne(p => p.Creator)
                .WithMany(p => p.Addresses)
                .HasForeignKey(p => p.CreatorIndex)
                .HasPrincipalKey(p => p.Index)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetupApplicationUser(ModelBuilder builder)
        {
            var applicationUser = builder.Entity<ApplicationUser>();
            applicationUser
                .Property(p => p.Index)
                .ValueGeneratedOnAdd();
            applicationUser
                .HasKey(p => p.Id)
                .ForSqlServerIsClustered(false);
            applicationUser
                .HasAlternateKey(p => p.Index);
            applicationUser
                .HasIndex(p => p.Index)
                .IsUnique();
            applicationUser
                .HasMany(p => p.AttendedEvents);
            applicationUser
                .HasMany(p => p.FollowedEvents);
        }

        private static void SetupApplicationUserFollower(ModelBuilder builder)
        {
            var applicationUserFollower = builder.Entity<ApplicationUserFollower>();
            applicationUserFollower
                .HasKey(p => new {p.UserIndex, p.FollowerIndex});
            applicationUserFollower
                .HasOne(p => p.User)
                .WithMany(p => p.Followers)
                .HasForeignKey(p => p.FollowerIndex)
                .HasPrincipalKey(p => p.Index)
                .OnDelete(DeleteBehavior.Restrict);
            applicationUserFollower
                .HasOne(p => p.Follower)
                .WithMany(p => p.FollowedUsers)
                .HasForeignKey(p => p.UserIndex)
                .HasPrincipalKey(p => p.Index)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetupEvent(ModelBuilder builder)
        {
            var @event = builder.Entity<Event>();
            @event
                .Property(p => p.Index)
                .ValueGeneratedOnAdd();
            @event
                .HasKey(p => p.Id)
                .ForSqlServerIsClustered(false);
            @event
                .HasAlternateKey(p => p.Index);
            @event
                .HasIndex(p => p.Index)
                .IsUnique();
            @event
                .HasOne(p => p.Creator)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.CreatorIndex)
                .HasPrincipalKey(p => p.Index)
                .OnDelete(DeleteBehavior.Restrict);
            @event
                .HasOne(p => p.Address)
                .WithMany(p => p.Events)
                .HasForeignKey(p => p.AddressIndex)
                .HasPrincipalKey(p => p.Index)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetupEventAttender(ModelBuilder builder)
        {
            var eventAttender = builder.Entity<EventAttender>();
            eventAttender
                .HasKey(p => new {p.EventIndex, p.AttenderIndex});
        }

        private static void SetupEventFollower(ModelBuilder builder)
        {
            var eventFollower = builder.Entity<EventFollower>();
            eventFollower
                .HasKey(p => new {p.EventIndex, p.FollowerIndex});
        }
    }
}