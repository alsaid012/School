

namespace SchoolERP.Domain.Interfaces
{
    public interface IAuditableEntity : IBaseEntity
    {
        string? CreatedBy { get; set; }
        string? UpdatedBy { get; set; }
    }
}
