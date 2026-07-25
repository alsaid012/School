namespace SchoolERP.Domain.Entities
{
    public class ClassRoom : SoftDeleteBaseEntity
    {
        public int GradeLevelId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string? ClassCode { get; set; }
        public string? RoomNumber { get; set; }
        public int Capacity { get; set; }
        public bool HasSmartBoard { get; set; }
        public bool HasProjector { get; set; }
        public int? TeacherId { get; set; } // Homeroom Teacher

        
        // Navigation Properties
        public virtual GradeLevel GradeLevel { get; set; } = null!;
        public virtual Teacher? Teacher { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    }
}