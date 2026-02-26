using System.ComponentModel.DataAnnotations;

namespace DoctorlyScheduling.Models
{
    public class Event
    {
        [Key]
        public Guid EventID { get; set; } = default!;
        public string title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime StartTime { get; set; } = default!;
        public DateTime EndTime { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
        public ICollection<Attendee> Attendees { get; set; } = default!;
        public int Version { get; set; }
    }
}
