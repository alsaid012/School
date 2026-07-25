using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;

public class UserRole : BaseEntity
{
    public int UserId { get; set; }
    public UserType RoleType { get; set; } // Student, Teacher, Employee, Principal, Admin
    public bool IsPrimary { get; set; } // الدور الأساسي
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
 

    // Navigation Properties
    public virtual User User { get; set; } = null!;
}