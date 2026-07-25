
namespace SchoolERP.Domain.Interfaces
{
    public interface ISoftDeleteEntity : IBaseEntity
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        string? DeletedBy { get; set; }
        string? DeleteReason { get; set; }
    }
}