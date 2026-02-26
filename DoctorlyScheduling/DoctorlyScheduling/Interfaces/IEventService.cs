using DoctorlyScheduling.Models;

namespace DoctorlyScheduling.Interfaces
{
    public interface IEventService
    {
        public Task<Event> CreateEventAsync(Event @event);
        public Task<Event> GetEventByID(Guid eventID);
        public Task<Event> UpdateEventAsync(Event @event);
        public Task<bool> CancelEventAsync(Event @event);
        public Task<bool> CancelEventAsync(Guid eventID);
        public Task<List<Event>> ListEventsAsync(string title, DateTime? searchFrom, DateTime? searchTo);
    }
}
