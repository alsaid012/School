namespace SchoolERP.Domain.Entities
{
    public class AcademicYear : SoftDeleteBaseEntity
    {
        public int SchoolId { get; set; }
        public string YearName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrent { get; set; }
        
        // Navigation Properties
        public virtual School School { get; set; } = null!;
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}