using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities
{
    public class UserContact : BaseEntity
    {
        public int UserId { get; set; }
        public ContactType ContactType { get; set; }
        public string ContactValue { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public bool IsVerified { get; set; }
   
        
        // Navigation Properties
        public virtual User User { get; set; } = null!;
    }
}