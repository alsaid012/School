namespace SchoolERP.Domain.Entities
{
    public class Department : SoftDeleteBaseEntity
    {
        public int GovernorateId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? DirectorName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        
        // Navigation Properties
        public virtual Governorate Governorate { get; set; } = null!;
        public virtual ICollection<School> Schools { get; set; } = new List<School>();
    }
}