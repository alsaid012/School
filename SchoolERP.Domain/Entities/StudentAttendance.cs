using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities
{
    public class StudentAttendance : BaseEntity
    {
        public int StudentId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
        public int? DelayMinutes { get; set; }
    
        
        // Navigation Properties
        public virtual Student Student { get; set; } = null!;
    }
}