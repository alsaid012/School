
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities
{
    public class User : SoftDeleteBaseEntity
    {
        public int SchoolId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string NationalId { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public UserType UserType { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Pending;
        public DateTime? LastLogin { get; set; }
        public string? ProfilePicture { get; set; }
        public string? Gender { get; set; }

        public virtual School School { get; set; } = null!;
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<UserContact> Contacts { get; set; } = new List<UserContact>();
    }
}