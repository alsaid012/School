using SchoolERP.Domain.Interfaces;


namespace SchoolERP.Domain.Entities
{
    public abstract class SoftDeleteBaseEntity : BaseEntity, ISoftDeleteEntity
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeleteReason { get; set; }

        // ✅ طرق مساعدة
        public void SoftDelete(string? deletedBy = null, string? reason = null)
        {
            IsDeleted = true;
            DeletedAt = DateTime.Now;
            DeletedBy = deletedBy;
            DeleteReason = reason;
            IsActive = false;
            UpdatedAt = DateTime.Now;
            UpdatedBy = deletedBy;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
            DeleteReason = null;
            IsActive = true;
            UpdatedAt = DateTime.Now;
        }
    }
}
