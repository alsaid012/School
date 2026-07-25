using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities
{
    public class Exam : BaseEntity
    {
        public int AcademicYearId { get; set; }
        public int SubjectId { get; set; }
        public int? ClassRoomId { get; set; }
        public string ExamName { get; set; } = string.Empty;
        public ExamType ExamType { get; set; }
        public DateTime ExamDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int MaxScore { get; set; }
        public int? TeacherId { get; set; }
       
        
        // Navigation Properties
        public virtual AcademicYear AcademicYear { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual ClassRoom? ClassRoom { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<ExamResult> Results { get; set; } = new List<ExamResult>();
    }
}