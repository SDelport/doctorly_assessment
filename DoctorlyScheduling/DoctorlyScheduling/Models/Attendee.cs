using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DoctorlyScheduling.Models
{
    public class Attendee
    {
        [Key]
        public Guid AttendeeID { get; set; } = default!;
        [Required]
        [MaxLength(30, ErrorMessage ="Name cannot exceed 30 characters.")]
        public string Name { get; set; } = default!;
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = default!;
        public bool IsAttending { get; set; } = default!;
    }
}
