using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using community.Core.Models;
using community.Data;
using community.Data.Managers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace community.UnitTests.Managers
{
    public class AddressManagerFacts
    {
        [Fact]
        public async Task Index()
        {
            Address address1 = new Address {Id = Guid.NewGuid()},
                address2 = new Address {Id = Guid.NewGuid()},
                address3 = new Address {Id = Guid.NewGuid()};
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Index")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Addresses.AddRangeAsync(new List<Address> {address1, address2, address3});
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var addressManager = new AddressManager(context);
                var addresss = await addressManager.Index();

                Assert.NotNull(addresss);
                Assert.IsType<List<Address>>(addresss);
                Assert.Equal(3, addresss.Count);
                Assert.IsType<Address>(addresss.Single(x => x.Id == address1.Id));
                Assert.IsType<Address>(addresss.Single(x => x.Id == address2.Id));
                Assert.IsType<Address>(addresss.Single(x => x.Id == address3.Id));
            }
        }

        [Fact]
        public async Task Details()
        {
            var id = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Details")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Addresses.Add(new Address {Id = id});
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var addressManager = new AddressManager(context);
                var address = await addressManager.Details(id);

                Assert.NotNull(address);
                Assert.IsType<Address>(address);
                Assert.Equal(id, address.Id);
            }
        }

        [Fact]
        public async Task Details_Null_Id()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Details_Null_Id")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var addressManager = new AddressManager(context);

                await Assert.ThrowsAsync<ArgumentNullException>(() => addressManager.Details(null));
            }
        }

        [Fact]
        public async Task Create()
        {
            Guid id;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Create")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var addressManager = new AddressManager(context);
                var address = await addressManager.Create(new Address());
                id = address.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var address = await context.Addresses.FindAsync(id);

                Assert.NotNull(address);
                Assert.IsType<Address>(address);
            }
        }

        [Fact]
        public async Task Create_Null_Address()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Create_Null_Address")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var addressManager = new AddressManager(context);

                await Assert.ThrowsAsync<ArgumentNullException>(() => addressManager.Create(null));
            }
        }

        [Fact]
        public async Task Edit()
        {
            Guid id;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Edit")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var address = new Address();
                context.Addresses.Add(address);
                await context.SaveChangesAsync();
                id = address.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var addressManager = new AddressManager(context);
                var address = await context.Addresses.FindAsync(id);
                await addressManager.Edit(address);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var address = await context.Addresses.FindAsync(id);

                Assert.NotNull(address);
                Assert.IsType<Address>(address);
            }
        }

        [Fact]
        public async Task Edit_Null_Address()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Edit_Null_Address")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var addressManager = new AddressManager(context);

                await Assert.ThrowsAsync<ArgumentNullException>(() => addressManager.Edit(null));
            }
        }

        [Fact]
        public async Task Edit_Bad_Id()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Edit_Bad_Id")
                .Options;
            var address = new Address {Id = Guid.NewGuid()};

            using (var context = new ApplicationDbContext(options))
            {
                var addressManager = new AddressManager(context);

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => addressManager.Edit(address));
            }
        }

        [Fact]
        public async Task Delete()
        {
            Guid id;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Delete")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var address = new Address();

                context.Addresses.Add(address);
                await context.SaveChangesAsync();

                id = address.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                Assert.Single(context.Addresses);

                var addressManager = new AddressManager(context);
                await addressManager.Delete(id);

                Assert.Empty(context.Addresses);
            }
        }

        [Fact]
        public async Task Delete_Null_Id()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AddressManager_Delete_Null_Id")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var addressManager = new AddressManager(context);

                await Assert.ThrowsAsync<ArgumentNullException>(() => addressManager.Delete(null));
            }
        }
    }
}
