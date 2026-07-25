using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities
{
    public class GradeLevel : SoftDeleteBaseEntity
    {
        public int SchoolId { get; set; }
        public string GradeName { get; set; } = string.Empty;
        public int GradeNumber { get; set; }
        public GradeStage GradeStage { get; set; }
        public string? Description { get; set; }
        
        // Navigation Properties
        public virtual School School { get; set; } = null!;
        public virtual ICollection<ClassRoom> ClassRooms { get; set; } = new List<ClassRoom>();
        public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    }
}