namespace SchoolERP.Domain.Entities
{
    public class ClassSchedule : BaseEntity
    {
        public int AcademicYearId { get; set; }
        public int ClassRoomId { get; set; }
        public int SubjectId { get; set; }
         public int TeacherId { get; set; }
     //   public int TeacherSubjectId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int? PeriodNumber { get; set; }
      
        
        // Navigation Properties
        public virtual AcademicYear AcademicYear { get; set; } = null!;
        public virtual ClassRoom ClassRoom { get; set; } = null!;
          public virtual Subject Subject { get; set; } = null!;
          public virtual Teacher Teacher { get; set; } = null!;

     //   public virtual TeacherSubject TeacherSubject { get; set; } = null!;

    }
}