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
    public class EventManagerFacts
    {
        [Fact]
        public async Task Index()
        {
            var address = new Address();
            Event event1 = new Event {Id = Guid.NewGuid(), Address = address},
                event2 = new Event {Id = Guid.NewGuid(), Address = address},
                event3 = new Event {Id = Guid.NewGuid(), Address = address};
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Index")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Events.AddRangeAsync(new List<Event> {event1, event2, event3});
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var eventManager = new EventManager(context);
                var events = await eventManager.Index();

                Assert.NotNull(events);
                Assert.IsType<List<Event>>(events);
                Assert.Equal(3, events.Count);
                Assert.IsType<Event>(events.Single(x => x.Id == event1.Id));
                Assert.IsType<Event>(events.Single(x => x.Id == event2.Id));
                Assert.IsType<Event>(events.Single(x => x.Id == event3.Id));
            }
        }

        [Fact]
        public async Task Details()
        {
            var id = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Details")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Events.Add(new Event {Id = id, Address = new Address()});
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var eventManager = new EventManager(context);
                var @event = await eventManager.Details(id);

                Assert.NotNull(@event);
                Assert.IsType<Event>(@event);
                Assert.Equal(id, @event.Id);
            }
        }

        [Fact]
        public async Task Details_Null_Id()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Details_Null_Id")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var eventManager = new EventManager(context);

                await Assert.ThrowsAsync<ArgumentNullException>(() => eventManager.Details(null));
            }
        }

        [Fact]
        public async Task Create()
        {
            Guid id;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Create")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var eventManager = new EventManager(context);
                var @event = await eventManager.Create(new Event());
                id = @event.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var @event = await context.Events.FindAsync(id);

                Assert.NotNull(@event);
                Assert.IsType<Event>(@event);
            }
        }

        [Fact]
        public async Task Create_Null_Event()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Create_Null_Event")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var eventManager = new EventManager(context);

                await Assert.ThrowsAsync<ArgumentNullException>(() => eventManager.Create(null));
            }
        }

        [Fact]
        public async Task Edit()
        {
            Guid id;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Edit")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var @event = new Event();
                context.Events.Add(@event);
                await context.SaveChangesAsync();
                id = @event.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                var eventManager = new EventManager(context);
                var @event = await context.Events.FindAsync(id);
                await eventManager.Edit(@event);
            }

            using (var context = new ApplicationDbContext(options))
            {
                var @event = await context.Events.FindAsync(id);

                Assert.NotNull(@event);
                Assert.IsType<Event>(@event);
            }
        }

        [Fact]
        public async Task Edit_Null_Event()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Edit_Null_Event")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var eventManager = new EventManager(context);

                await Assert.ThrowsAsync<ArgumentNullException>(() => eventManager.Edit(null));
            }
        }

        [Fact]
        public async Task Edit_Bad_Id()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Edit_Bad_Id")
                .Options;
            var Event = new Event();

            using (var context = new ApplicationDbContext(options))
            {
                var eventManager = new EventManager(context);

                await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => eventManager.Edit(Event));
            }
        }

        [Fact]
        public async Task Delete()
        {
            Guid id;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Delete")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var @event = new Event();

                context.Events.Add(@event);
                await context.SaveChangesAsync();

                id = @event.Id;
            }

            using (var context = new ApplicationDbContext(options))
            {
                Assert.Single(context.Events);

                var eventManager = new EventManager(context);
                await eventManager.Delete(id);

                Assert.Empty(context.Events);
            }
        }

        [Fact]
        public async Task Delete_Null_Id()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("EventManager_Delete_Null_Id")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var eventManager = new EventManager(context);

                await Assert.ThrowsAsync<ArgumentNullException>(() => eventManager.Delete(null));
            }
        }
    }
}
