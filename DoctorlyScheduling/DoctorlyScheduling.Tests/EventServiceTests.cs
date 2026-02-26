using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using DoctorlyScheduling.Models;
using DoctorlyScheduling.Services;
using DoctorlyScheduling.Interfaces;

namespace DoctorlyScheduling.Tests
{
    public class EventServiceTests
    {
        private DbContextOptions<SchedulingContext> CreateNewContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<SchedulingContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }

        [Fact]
        public async Task CreateEventAsync_AddsEvent_WithVersion1()
        {
            var options = CreateNewContextOptions("create_event_db");

            using (var context = new SchedulingContext(options))
            {
                var attendeeMock = new Mock<IAttendeeService>();
                var service = new EventService(attendeeMock.Object, context);

                var @event = new Event
                {
                    EventID = Guid.NewGuid(),
                    title = "Test Event",
                    Description = "desc",
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddHours(1),
                    IsActive = true,
                    Attendees = new List<Attendee>()
                };

                var created = await service.CreateEventAsync(@event);

                Assert.Equal(1, created.Version);
                var persisted = await context.Events.FirstOrDefaultAsync(e => e.EventID == @event.EventID);
                Assert.NotNull(persisted);
                Assert.Equal("Test Event", persisted!.title);
            }
        }

        [Fact]
        public async Task ListEventsAsync_Filters_ByTitle_And_DateRange()
        {
            var options = CreateNewContextOptions("list_event_db");

            var now = DateTime.UtcNow;

            using (var context = new SchedulingContext(options))
            {
                var events = new[]
                {
                    new Event { EventID = Guid.NewGuid(), title = "Alpha Meeting", Description = "a", StartTime = now.AddHours(-1), EndTime = now.AddHours(1), IsActive = true, Version = 1, Attendees = new List<Attendee>() },
                    new Event { EventID = Guid.NewGuid(), title = "Beta Meeting", Description = "b", StartTime = now.AddDays(1), EndTime = now.AddDays(1).AddHours(1), IsActive = true, Version = 1, Attendees = new List<Attendee>() },
                    new Event { EventID = Guid.NewGuid(), title = "Alpha Followup", Description = "c", StartTime = now.AddHours(-2), EndTime = now.AddHours(-1), IsActive = true, Version = 1, Attendees = new List<Attendee>() }
                };

                await context.Events.AddRangeAsync(events);
                await context.SaveChangesAsync();
            }

            using (var context = new SchedulingContext(options))
            {
                var attendeeMock = new Mock<IAttendeeService>();
                var service = new EventService(attendeeMock.Object, context);

                // search for "Alpha" events within a broad window
                var results = await service.ListEventsAsync("Alpha", now.AddDays(-1), now.AddDays(1));

                Assert.Equal(2, results.Count);
                Assert.All(results, r => Assert.Contains("Alpha", r.title));
            }
        }

        [Fact]
        public async Task CancelEventAsync_ByEvent_And_ById_Sets_IsActive_False()
        {
            var options = CreateNewContextOptions("cancel_event_db");
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();

            using (var context = new SchedulingContext(options))
            {
                var e1 = new Event { EventID = id1, title = "ToCancel1", Description = "x", StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(1), IsActive = true, Version = 1, Attendees = new List<Attendee>() };
                var e2 = new Event { EventID = id2, title = "ToCancel2", Description = "y", StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddHours(1), IsActive = true, Version = 1, Attendees = new List<Attendee>() };
                await context.Events.AddRangeAsync(e1, e2);
                await context.SaveChangesAsync();
            }

            // Cancel by passing the tracked event
            using (var context = new SchedulingContext(options))
            {
                var attendeeMock = new Mock<IAttendeeService>();
                var service = new EventService(attendeeMock.Object, context);

                var toCancel = await context.Events.FindAsync(id1);
                Assert.NotNull(toCancel);

                var res = await service.CancelEventAsync(toCancel!);
                Assert.True(res);

                var persisted = await context.Events.FindAsync(id1);
                Assert.False(persisted!.IsActive);
            }

            // Cancel by id
            using (var context = new SchedulingContext(options))
            {
                var attendeeMock = new Mock<IAttendeeService>();
                var service = new EventService(attendeeMock.Object, context);

                var res = await service.CancelEventAsync(id2);
                Assert.True(res);

                var persisted = await context.Events.FindAsync(id2);
                Assert.False(persisted!.IsActive);
            }
        }

        [Fact]
        public async Task GetEventByID_Returns_Event()
        {
            var options = CreateNewContextOptions("getbyid_db");
            var id = Guid.NewGuid();

            using (var context = new SchedulingContext(options))
            {
                var e = new Event
                {
                    EventID = id,
                    title = "Get Test",
                    Description = "desc",
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddHours(1),
                    IsActive = true,
                    Version = 1,
                    Attendees = new List<Attendee>()
                };
                await context.Events.AddAsync(e);
                await context.SaveChangesAsync();
            }

            using (var context = new SchedulingContext(options))
            {
                var attendeeMock = new Mock<IAttendeeService>();
                var service = new EventService(attendeeMock.Object, context);

                var fetched = await service.GetEventByID(id);

                Assert.Equal(id, fetched.EventID);
                Assert.Equal("Get Test", fetched.title);
            }
        }

        [Fact]
        public async Task UpdateEventAsync_Updates_Fields_And_Attendees()
        {
            var options = CreateNewContextOptions("update_event_db");
            var id = Guid.NewGuid();
            var attendee1 = new Attendee { AttendeeID = Guid.NewGuid(), Name = "Alice", Email = "alice@test.com", IsAttending = true };
            var attendeeToRemove = new Attendee { AttendeeID = Guid.NewGuid(), Name = "RemoveMe", Email = "remove@test.com", IsAttending = true };

            using (var context = new SchedulingContext(options))
            {
                var e = new Event
                {
                    EventID = id,
                    title = "Original",
                    Description = "orig",
                    StartTime = DateTime.UtcNow,
                    EndTime = DateTime.UtcNow.AddHours(1),
                    IsActive = true,
                    Version = 1,
                    Attendees = new List<Attendee> { attendee1, attendeeToRemove }
                };
                await context.Events.AddAsync(e);
                await context.SaveChangesAsync();
            }

            using (var context = new SchedulingContext(options))
            {
                var attendeeMock = new Mock<IAttendeeService>();
                var service = new EventService(attendeeMock.Object, context);

                // prepare updated event: change title, update attendee1, add new attendee, remove attendeeToRemove
                var updated = new Event
                {
                    EventID = id,
                    title = "Updated",
                    Description = "updated desc",
                    StartTime = DateTime.UtcNow.AddDays(1),
                    EndTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                    IsActive = false,
                    Version = 1, // service should increment
                    Attendees = new List<Attendee>
                    {
                        new Attendee { Name = "Alice Updated", Email = "alice@test.com", IsAttending = false },
                        new Attendee { Name = "Bob", Email = "bob@test.com", IsAttending = true }
                    }
                };

                var result = await service.UpdateEventAsync(updated);

                Assert.Equal("Updated", result.title);
                Assert.Equal("updated desc", result.Description);
                Assert.Equal(2, result.Version); // incremented
                Assert.False(result.IsActive);
                Assert.Equal(2, result.Attendees.Count);
                Assert.Contains(result.Attendees, a => a.Email == "alice@test.com" && a.Name == "Alice Updated" && a.IsAttending == false);
                Assert.Contains(result.Attendees, a => a.Email == "bob@test.com");
                Assert.DoesNotContain(result.Attendees, a => a.Email == "remove@test.com");

                // verify persisted
                var persisted = await context.Events.Include(e => e.Attendees).FirstOrDefaultAsync(e => e.EventID == id);
                Assert.NotNull(persisted);
                Assert.Equal("Updated", persisted!.title);
                Assert.Equal(2, persisted.Attendees.Count);
            }
        }
    }
}
