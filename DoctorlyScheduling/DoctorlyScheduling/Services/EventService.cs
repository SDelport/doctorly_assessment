using DoctorlyScheduling.Interfaces;
using DoctorlyScheduling.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorlyScheduling.Services
{
    public class EventService : IEventService
    {
        private IAttendeeService attendeeService;
        private SchedulingContext schedulingContext;

        public EventService(IAttendeeService attendeeService, SchedulingContext schedulingContext)
        {
            this.attendeeService = attendeeService;
            this.schedulingContext = schedulingContext;
        }

        public async Task<bool> CancelEventAsync(Event @event)
        {
            @event.IsActive = false;
            var changes = await schedulingContext.SaveChangesAsync(); 
            return changes > 0;
        }

        public async Task<bool> CancelEventAsync(Guid eventID)
        {
            Event? @event = await schedulingContext.Events.FindAsync(eventID);

            if (@event == null)
                throw new KeyNotFoundException("Event not found.");

            return await CancelEventAsync(@event);
        }

        public async Task<Event> CreateEventAsync(Event @event)
        {
            @event.Version = 1;
            await schedulingContext.Events.AddAsync(@event);
            await schedulingContext.SaveChangesAsync();

            return @event;
        }

        public async Task<Event> GetEventByID(Guid eventID)
        {
            var @event = await schedulingContext.Events.Include(dbEvent => dbEvent.Attendees).FirstOrDefaultAsync(dbEvent => dbEvent.EventID == eventID);
            if (@event == null)
                throw new KeyNotFoundException("Event not found.");
            return @event;
        }

        public async Task<List<Event>> ListEventsAsync(string title, DateTime? searchFrom, DateTime? searchTo)
        {
            var events = schedulingContext.Events.AsQueryable();

            if (!string.IsNullOrEmpty(title))
                events = events.Where(@event => @event.title.Contains(title));


            if (searchFrom.HasValue)
                events = events.Where(@event => @event.StartTime >= searchFrom.Value);


            if (searchTo.HasValue)
                events = events.Where(@event => @event.EndTime <= searchTo.Value);

            return await events.ToListAsync();

        }

        public async Task<Event> UpdateEventAsync(Event @event)
        {
            var existingEvent = await schedulingContext.Events.Include(dbEvent => dbEvent.Attendees).FirstOrDefaultAsync(dbEvent => dbEvent.EventID == @event.EventID);

            if (existingEvent == null)
                throw new KeyNotFoundException("Event not found.");

            existingEvent.title = @event.title;
            existingEvent.Description = @event.Description;
            existingEvent.Version = @event.Version + 1;
            existingEvent.StartTime = @event.StartTime;
            existingEvent.EndTime = @event.EndTime;
            existingEvent.IsActive = @event.IsActive;

            foreach (var attendee in @event.Attendees)
            {
                var existingAttendee = existingEvent.Attendees.FirstOrDefault(a => a.Email == attendee.Email);
                if (existingAttendee == null)
                {
                    existingEvent.Attendees.Add(attendee);
                }
                else
                {
                    existingAttendee.Name = attendee.Name;
                    existingAttendee.IsAttending = attendee.IsAttending;
                }
            }

            existingEvent.Attendees = existingEvent.Attendees
                .Where(attendee => @event.Attendees.Select(existingAttendee => existingAttendee.Email)
                .Any(existingAttendee => existingAttendee == attendee.Email))
                .ToList();

            await schedulingContext.SaveChangesAsync();

            return existingEvent;

        }
    }
}
