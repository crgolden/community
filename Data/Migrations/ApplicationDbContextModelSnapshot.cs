using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Community.Data;

namespace Community.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Community.Models.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<int>("CreatorIndex");

                    b.Property<bool>("Home");

                    b.Property<int>("Index")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Latitude");

                    b.Property<string>("Longitude");

                    b.Property<string>("State");

                    b.Property<string>("Street");

                    b.Property<string>("Street2");

                    b.Property<string>("ZipCode");

                    b.HasKey("Id");

                    b.HasIndex("CreatorIndex");

                    b.HasIndex("Index")
                        .IsUnique();

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Community.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("Index")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Index")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Community.Models.ApplicationUserFollower", b =>
                {
                    b.Property<int>("UserIndex");

                    b.Property<int>("FollowerIndex");

                    b.HasKey("UserIndex", "FollowerIndex");

                    b.HasIndex("FollowerIndex");

                    b.ToTable("UserFollowers");
                });

            modelBuilder.Entity("Community.Models.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressIndex");

                    b.Property<int>("CreatorIndex");

                    b.Property<string>("Date");

                    b.Property<string>("Details");

                    b.Property<int>("Index")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Time");

                    b.HasKey("Id");

                    b.HasIndex("AddressIndex");

                    b.HasIndex("CreatorIndex");

                    b.HasIndex("Index")
                        .IsUnique();

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Community.Models.EventAttender", b =>
                {
                    b.Property<int>("EventIndex");

                    b.Property<int>("AttenderIndex");

                    b.Property<string>("AttenderId");

                    b.Property<Guid>("EventId");

                    b.HasKey("EventIndex", "AttenderIndex");

                    b.HasIndex("AttenderId");

                    b.HasIndex("EventId");

                    b.ToTable("EventAttenders");
                });

            modelBuilder.Entity("Community.Models.EventFollower", b =>
                {
                    b.Property<int>("EventIndex");

                    b.Property<int>("FollowerIndex");

                    b.Property<Guid>("EventId");

                    b.Property<string>("FollowerId");

                    b.HasKey("EventIndex", "FollowerIndex");

                    b.HasIndex("EventId");

                    b.HasIndex("FollowerId");

                    b.ToTable("EventFollowers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Community.Models.Address", b =>
                {
                    b.HasOne("Community.Models.ApplicationUser", "Creator")
                        .WithMany("Addresses")
                        .HasForeignKey("CreatorIndex")
                        .HasPrincipalKey("Index");
                });

            modelBuilder.Entity("Community.Models.ApplicationUserFollower", b =>
                {
                    b.HasOne("Community.Models.ApplicationUser", "User")
                        .WithMany("Followers")
                        .HasForeignKey("FollowerIndex")
                        .HasPrincipalKey("Index");

                    b.HasOne("Community.Models.ApplicationUser", "Follower")
                        .WithMany("FollowedUsers")
                        .HasForeignKey("UserIndex")
                        .HasPrincipalKey("Index");
                });

            modelBuilder.Entity("Community.Models.Event", b =>
                {
                    b.HasOne("Community.Models.Address", "Address")
                        .WithMany("Events")
                        .HasForeignKey("AddressIndex")
                        .HasPrincipalKey("Index");

                    b.HasOne("Community.Models.ApplicationUser", "Creator")
                        .WithMany("Events")
                        .HasForeignKey("CreatorIndex")
                        .HasPrincipalKey("Index");
                });

            modelBuilder.Entity("Community.Models.EventAttender", b =>
                {
                    b.HasOne("Community.Models.ApplicationUser", "Attender")
                        .WithMany("AttendedEvents")
                        .HasForeignKey("AttenderId");

                    b.HasOne("Community.Models.Event", "Event")
                        .WithMany("Attenders")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Community.Models.EventFollower", b =>
                {
                    b.HasOne("Community.Models.Event", "Event")
                        .WithMany("Followers")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Community.Models.ApplicationUser", "Follower")
                        .WithMany("FollowedEvents")
                        .HasForeignKey("FollowerId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Community.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Community.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Community.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
