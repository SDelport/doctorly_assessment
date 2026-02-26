using System.ComponentModel.DataAnnotations;

namespace DoctorlyScheduling.Models.Transfer
{
    public class EventListRequest
    {
        public string Title { get; set; }
        public DateTime? SearchFrom { get; set; }
        public DateTime? SearchTo { get; set; }
    }
}
