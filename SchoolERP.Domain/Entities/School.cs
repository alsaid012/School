using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities
{
    public class School : SoftDeleteBaseEntity
    {
        public int DepartmentId { get; set; }
        public string SchoolName { get; set; } = string.Empty;
        public string SchoolCode { get; set; } = string.Empty;
        public SchoolType SchoolType { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? PrincipalName { get; set; }
        public string? LogoUrl { get; set; }
        public int? EstablishedYear { get; set; }
        
        // Navigation Properties
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; } = new List<User>();
        public virtual ICollection<GradeLevel> GradeLevels { get; set; } = new List<GradeLevel>();
        public virtual ICollection<AcademicYear> AcademicYears { get; set; } = new List<AcademicYear>();
    }
}