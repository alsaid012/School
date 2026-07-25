
namespace SchoolERP.Domain.Entities
{
    public class Student : SoftDeleteBaseEntity
    {
        public int UserId { get; set; }
        public string StudentCode { get; set; } = string.Empty;
        public int AcademicYearId { get; set; }
        public int? ClassRoomId { get; set; }
        public string? ParentName { get; set; }
        public string? ParentPhone { get; set; }
        public string? ParentEmail { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        public bool IsGraduated { get; set; }
        
        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual AcademicYear AcademicYear { get; set; } = null!;
        public virtual ClassRoom? ClassRoom { get; set; }
        public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
        public virtual ICollection<StudentAttendance> Attendances { get; set; } = new List<StudentAttendance>();
    }
}