

namespace SchoolERP.Domain.Entities
{
    public class Employee : SoftDeleteBaseEntity
    {
        public int UserId { get; set; }
        public string EmployeeCode { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string? Department { get; set; }
        public DateTime HireDate { get; set; }
        public decimal? Salary { get; set; }
        
        // Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<EmployeeAttendance> Attendances { get; set; } = new List<EmployeeAttendance>();
    }
}