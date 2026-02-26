using System.ComponentModel;

namespace DoctorlyScheduling.Models.Transfer
{
    public class EventSearchRequest
    {
        [DefaultValue("")]
        public string Title { get; set; }
        [DefaultValue(null)]
        public DateTime? SearchFrom { get; set; }
        [DefaultValue(null)]
        public DateTime? SearchTo { get; set; }
    }
}
