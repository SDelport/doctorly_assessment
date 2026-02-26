using DoctorlyScheduling.Interfaces;
using DoctorlyScheduling.Models;
using DoctorlyScheduling.Models.Transfer;
using DoctorlyScheduling.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DoctorlyScheduling.Controllers
{
    [ApiController]
    [Route("event")]
    public class EventController : ControllerBase
    {
        private IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        [HttpPost("list")]
        public async Task<IEnumerable<EventSearchResponse>> List(EventSearchRequest searchRequest)
        {
            var events = await eventService.ListEventsAsync(searchRequest.Title, searchRequest.SearchFrom, searchRequest.SearchTo);

            return new List<EventSearchResponse>(events.Select(e => new EventSearchResponse()
            {
                EventID = e.EventID,
                title = e.title,
                Description = e.Description,
                StartTime = e.StartTime,
                EndTime = e.EndTime,
                IsActive = e.IsActive
            }));
        }

        [HttpGet("get-by-id")]
        public async Task<EventGetByIDResponse> List(Guid eventID)
        {
            var @event = await eventService.GetEventByID(eventID);

            return new EventGetByIDResponse()
            {
                EventID = @event.EventID,
                title = @event.title,
                Description = @event.Description,
                StartTime = @event.StartTime,
                EndTime = @event.EndTime,
                IsActive = @event.IsActive,
                Attendees = @event.Attendees.Select(a => new AttendeeResponse()
                {
                    Name = a.Name,
                    Email = a.Email,
                    IsAttending = a.IsAttending
                }).ToList()
            };
        }

        [SwaggerOperation(Description = "Create Event with Attendees")]
        [HttpPost("create")]
        public async Task<EventCreateResponse> CreateAsync(EventCreateRequest request)
        {
            Event @event = new Event()
            {
                title = request.title,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsActive = request.IsActive,
                Attendees = request.Attendees.Select(a => new Attendee()
                {
                    Name = a.Name,
                    Email = a.Email
                }).ToList()
            };

            var createdEvent = await this.eventService.CreateEventAsync(@event);

            return new EventCreateResponse()
            {
                EventID = createdEvent.EventID,
                title = createdEvent.title,
                Description = createdEvent.Description,
                StartTime = createdEvent.StartTime,
                EndTime = createdEvent.EndTime,
                IsActive = createdEvent.IsActive,
                Attendees = createdEvent.Attendees.Select(a => new AttendeeResponse()
                {
                    Name = a.Name,
                    Email = a.Email,
                    IsAttending = a.IsAttending
                }).ToList()
            };
        }

        [SwaggerOperation(Description = "Update Event and Attendees")]
        [HttpPut("update")]
        public async Task<EventUpdateResponse> UpdateAsync(EventUpdateRequest request)
        {
            Event @event = new Event()
            {
                EventID = request.EventID,
                title = request.title,
                Description = request.Description,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsActive = request.IsActive,
                Attendees = request.Attendees.Select(a => new Attendee()
                {
                    Name = a.Name,
                    Email = a.Email
                }).ToList()
            };

            var createdEvent = await this.eventService.UpdateEventAsync(@event);

            return new EventUpdateResponse()
            {
                EventID = createdEvent.EventID,
                title = createdEvent.title,
                Description = createdEvent.Description,
                StartTime = createdEvent.StartTime,
                EndTime = createdEvent.EndTime,
                IsActive = createdEvent.IsActive,
                Attendees = createdEvent.Attendees.Select(a => new AttendeeResponse()
                {
                    Name = a.Name,
                    Email = a.Email,
                    IsAttending = a.IsAttending
                }).ToList()
            };
        }

        [SwaggerOperation(Description = "Delete Event and Attendees")]
        [HttpDelete("delete:eventID")]
        public async Task<bool> DeleteAsync(Guid eventID)
        {
            return await this.eventService.CancelEventAsync(eventID);
        }
    }
}
