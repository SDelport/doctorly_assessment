using DoctorlyScheduling.Interfaces;
using DoctorlyScheduling.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorlyScheduling.Services
{
    public class AttendeeService : IAttendeeService
    {
        private SchedulingContext schedulingContext;
        public AttendeeService(SchedulingContext schedulingContext)
        {
            this.schedulingContext = schedulingContext;
        }
        public async Task<Attendee> CreateAttendee(Attendee attendee)
        {
            await this.schedulingContext.Attendees.AddAsync(attendee);
            await this.schedulingContext.SaveChangesAsync();
            return attendee;
        }

        public async Task<bool> DeleteAttendee(Attendee attendee)
        {
            schedulingContext.Entry(attendee).State = EntityState.Deleted;
            int changeCount = await schedulingContext.SaveChangesAsync();
            return changeCount > 0;
        }

        public async Task<Attendee> UpdateAttendeeAsync(Attendee attendee)
        {
            var existingAttendee = await schedulingContext.Attendees.FindAsync(attendee.AttendeeID);

            if (existingAttendee == null)
                throw new KeyNotFoundException($"Attendee with ID {attendee.AttendeeID} not found.");

            schedulingContext.Entry(attendee).State = EntityState.Modified;
            await schedulingContext.SaveChangesAsync();

            return attendee;
        }

    }
}
