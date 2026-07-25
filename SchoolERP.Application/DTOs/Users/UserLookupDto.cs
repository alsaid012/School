using System.ComponentModel;

namespace SchoolERP.Application.DTOs.Users
{
    public class UserLookupDto
    {
        [DisplayName("معرف المستخدم")]
        public int Id { get; set; }

        [DisplayName("اسم المستخدم")]
        public string FullName { get; set; } = string.Empty;

        [DisplayName("اسم المستخدم")]
        public string Username { get; set; } = string.Empty;

        [DisplayName("نوع المستخدم")]
        public string? UserTypeName { get; set; }

        [DisplayName("البريد الإلكتروني")]
        public string? Email { get; set; }

        [DisplayName("مفعل")]
        public bool IsActive { get; set; }
    }
}