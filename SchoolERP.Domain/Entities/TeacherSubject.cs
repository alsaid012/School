namespace SchoolERP.Domain.Entities
{
    public class TeacherSubject : BaseEntity
    {
        public int TeacherId { get; set; }
        public int SubjectId { get; set; }
        public bool IsPrimary { get; set; }
        
        // Navigation Properties
        public virtual Teacher Teacher { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
    }
}