using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Community.Data;

namespace Community.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170411000058_AddAddressesEvents")]
    partial class AddAddressesEvents
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Community.Models.Address", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<int>("CreatorId");

                    b.Property<bool>("Home");

                    b.Property<int>("IdInt")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Latitude");

                    b.Property<string>("Longitude");

                    b.Property<string>("State");

                    b.Property<string>("Street");

                    b.Property<string>("Street2");

                    b.Property<string>("ZipCode");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("CreatorId");

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

                    b.Property<int>("IdInt")
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

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Community.Models.ApplicationUserFollowing", b =>
                {
                    b.Property<int>("FollowedUserId");

                    b.Property<int>("FollowerId");

                    b.HasKey("FollowedUserId", "FollowerId");

                    b.HasIndex("FollowerId");

                    b.ToTable("UserFollowings");
                });

            modelBuilder.Entity("Community.Models.Event", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressId");

                    b.Property<int>("CreatorId");

                    b.Property<string>("Date");

                    b.Property<string>("Details");

                    b.Property<int>("IdInt")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("Time");

                    b.HasKey("Id")
                        .HasAnnotation("SqlServer:Clustered", false);

                    b.HasAlternateKey("IdInt");

                    b.HasIndex("AddressId");

                    b.HasIndex("CreatorId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Community.Models.EventAttending", b =>
                {
                    b.Property<int>("AttendedEventId");

                    b.Property<int>("AttenderId");

                    b.Property<string>("AttendedEventId1");

                    b.HasKey("AttendedEventId", "AttenderId");

                    b.HasIndex("AttendedEventId1");

                    b.ToTable("EventAttendings");
                });

            modelBuilder.Entity("Community.Models.EventFollowing", b =>
                {
                    b.Property<int>("FollowedEventId");

                    b.Property<int>("FollowerId");

                    b.Property<string>("FollowedEventId1");

                    b.HasKey("FollowedEventId", "FollowerId");

                    b.HasIndex("FollowedEventId1");

                    b.ToTable("EventFollowings");
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
                        .HasForeignKey("CreatorId")
                        .HasPrincipalKey("IdInt");
                });

            modelBuilder.Entity("Community.Models.ApplicationUserFollowing", b =>
                {
                    b.HasOne("Community.Models.ApplicationUser", "Follower")
                        .WithMany("FollowedUsers")
                        .HasForeignKey("FollowedUserId")
                        .HasPrincipalKey("IdInt");

                    b.HasOne("Community.Models.ApplicationUser", "FollowedUser")
                        .WithMany("Followers")
                        .HasForeignKey("FollowerId")
                        .HasPrincipalKey("IdInt");
                });

            modelBuilder.Entity("Community.Models.Event", b =>
                {
                    b.HasOne("Community.Models.Address", "Address")
                        .WithMany("Events")
                        .HasForeignKey("AddressId")
                        .HasPrincipalKey("IdInt");

                    b.HasOne("Community.Models.ApplicationUser", "Creator")
                        .WithMany("Events")
                        .HasForeignKey("CreatorId")
                        .HasPrincipalKey("IdInt");
                });

            modelBuilder.Entity("Community.Models.EventAttending", b =>
                {
                    b.HasOne("Community.Models.ApplicationUser", "Attender")
                        .WithMany("AttendedEvents")
                        .HasForeignKey("AttendedEventId")
                        .HasPrincipalKey("IdInt");

                    b.HasOne("Community.Models.Event", "AttendedEvent")
                        .WithMany("Attenders")
                        .HasForeignKey("AttendedEventId1");
                });

            modelBuilder.Entity("Community.Models.EventFollowing", b =>
                {
                    b.HasOne("Community.Models.ApplicationUser", "Follower")
                        .WithMany("FollowedEvents")
                        .HasForeignKey("FollowedEventId")
                        .HasPrincipalKey("IdInt");

                    b.HasOne("Community.Models.Event", "FollowedEvent")
                        .WithMany("Followers")
                        .HasForeignKey("FollowedEventId1");
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
