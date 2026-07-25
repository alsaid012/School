using SchoolERP.Application.DTOs.UserContacts;
using SchoolERP.Application.DTOs.UserRoles;

namespace SchoolERP.Application.DTOs.Users
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👤  نموذج تفاصيل المستخدم (User Details DTO)
    /// 📌  الوظيفة: نقل بيانات المستخدم مع التفاصيل الكاملة
    /// 📦  الاستخدام: في UsersController (GET /{id} endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserDetailsDto : UserDto
    {
        /// <summary>
        /// الرقم القومي (14 رقم)
        /// </summary>
        /// <example>12345678901234</example>
        public string NationalId { get; set; } = string.Empty;

        /// <summary>
        /// تاريخ الميلاد
        /// </summary>
        /// <example>2000-01-01</example>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// العنوان
        /// </summary>
        /// <example>مصر الجديدة - القاهرة</example>
        public string? Address { get; set; }

        /// <summary>
        /// تاريخ آخر تسجيل دخول
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        public DateTime? LastLogin { get; set; }

        ///// <summary>
        ///// صورة الملف الشخصي
        ///// </summary>
        ///// <example>/uploads/profiles/ahmed.jpg</example>
        //public string? ProfilePicture { get; set; }


        // ✅ قائمة الأدوار
        public List<UserRoleDto> UserRoles { get; set; } = new();

        // ✅ قائمة جهات الاتصال
        public List<UserContactDto> Contacts { get; set; } = new();

    }
}