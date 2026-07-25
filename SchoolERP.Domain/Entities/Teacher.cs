namespace SchoolERP.Domain.Entities
{
    public class Teacher : SoftDeleteBaseEntity
    {
        public int UserId { get; set; }
        public string TeacherCode { get; set; } = string.Empty;
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public DateTime HireDate { get; set; }
        public decimal? Salary { get; set; }
        public bool IsHomeroomTeacher { get; set; }
        
        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<TeacherSubject> TeacherSubjects { get; set; } = new List<TeacherSubject>();
        public virtual ICollection<ClassSchedule> Schedules { get; set; } = new List<ClassSchedule>();
        public virtual ICollection<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
        public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

    }
}