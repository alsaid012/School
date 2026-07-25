using SchoolERP.Domain.Interfaces;

namespace SchoolERP.Domain.Entities
{
    public abstract class BaseEntity: IBaseEntity, IAuditableEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? Notes { get; set; }
    }
}