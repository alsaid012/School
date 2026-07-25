namespace SchoolERP.Domain.Entities
{
    public class Subject : SoftDeleteBaseEntity
    {
        public int GradeLevelId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string? SubjectCode { get; set; }
        public int? WeeklyHours { get; set; }
        public bool IsRequired { get; set; } = true;
        public string? Description { get; set; }
        
        // Navigation Properties
        public virtual GradeLevel GradeLevel { get; set; } = null!;
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
        public virtual ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();
    }
}