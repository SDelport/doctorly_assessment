namespace DoctorlyScheduling.Models.Transfer
{
    public class EventSearchResponse
    {
        public Guid EventID { get; set; } = default!;
        public string title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime StartTime { get; set; } = default!;
        public DateTime EndTime { get; set; } = default!;
        public bool IsActive { get; set; } = default!;
    }
}
