using DoctorlyScheduling.Models;

namespace DoctorlyScheduling.Interfaces
{
    public interface IAttendeeService
    {
        public Task<Attendee> CreateAttendee(Attendee attendee);
        public Task<Attendee> UpdateAttendeeAsync(Attendee attendee);
        public Task<bool> DeleteAttendee(Attendee attendee);
    }
}
