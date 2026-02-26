using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DoctorlyScheduling.Models.Transfer
{
    public class AttendeeRequest
    {
        [Required]
        [MaxLength(30, ErrorMessage = "Name cannot exceed 30 characters.")]
        [DefaultValue("Gregory")]
        public string Name { get; set; } = default!;
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format.")]
        [DefaultValue("myname@email.com")]
        public string Email { get; set; } = default!;
        public bool IsAttending { get; set; } = default!;
    }
}
