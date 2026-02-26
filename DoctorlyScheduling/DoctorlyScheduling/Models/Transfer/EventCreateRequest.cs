using System.ComponentModel.DataAnnotations;

namespace DoctorlyScheduling.Models.Transfer
{
    public class EventCreateRequest
    {
        public string title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime StartTime { get; set; } = default!;
        public DateTime EndTime { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public ICollection<AttendeeRequest> Attendees { get; set; } = default!;
    }
}
