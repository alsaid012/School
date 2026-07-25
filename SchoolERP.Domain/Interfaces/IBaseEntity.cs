namespace SchoolERP.Domain.Interfaces
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
        bool IsActive { get; set; }
    }
}