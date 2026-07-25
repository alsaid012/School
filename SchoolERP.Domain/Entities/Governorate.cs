namespace SchoolERP.Domain.Entities
{
    public class Governorate : SoftDeleteBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }
        
        // Navigation Properties
        public virtual ICollection<Department> Departments { get; set; } = new List<Department>();
    }
}