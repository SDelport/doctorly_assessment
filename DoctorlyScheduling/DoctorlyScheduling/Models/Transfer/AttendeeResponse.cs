using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DoctorlyScheduling.Models.Transfer
{
    public class AttendeeResponse
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool IsAttending { get; set; } = default!;
    }
}
