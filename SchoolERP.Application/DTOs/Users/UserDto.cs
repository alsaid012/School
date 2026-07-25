using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Users
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// 👤  نموذج بيانات المستخدم الأساسية (User DTO)
    /// 📌  الوظيفة: نقل بيانات المستخدم من الخادم إلى العميل
    /// 📦  الاستخدام: في UsersController (GET endpoints)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// معرف المستخدم (Primary Key)
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        /// <summary>
        /// اسم المستخدم (فريد)
        /// </summary>
        /// <example>ahmed.hassan</example>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// الاسم الكامل
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        /// <example>ahmed@example.com</example>
        public string? Email { get; set; }

        /// <summary>
        /// رقم الهاتف المحمول
        /// </summary>
        /// <example>01001234567</example>
        public string? PhoneNumber { get; set; }



        public string? Gender { get; set; }

        /// <summary>
        /// نوع المستخدم (طالب، معلم، موظف، مدير، أدمن)
        /// </summary>
        /// <example>Student</example>
        public UserType UserType { get; set; }

        /// <summary>
        /// حالة المستخدم (معلق، نشط، موقوف، غير نشط)
        /// </summary>
        /// <example>Active</example>
        public UserStatus Status { get; set; }

        /// <summary>
        /// معرف المدرسة
        /// </summary>
        /// <example>1</example>
        public int SchoolId { get; set; }

        /// <summary>
        /// اسم المدرسة
        /// </summary>
        /// <example>مدرسة النصر الثانوية</example>
        public string? SchoolName { get; set; }

        /// <summary>
        /// صورة الملف الشخصي
        /// </summary>
        public string? ProfilePicture { get; set; }

        /// <summary>
        /// هل المستخدم مفعل؟
        /// </summary>
        /// <example>true</example>
        public bool IsActive { get; set; }

        /// <summary>
        /// تاريخ الإنشاء
        /// </summary>
        /// <example>2024-01-01T12:00:00</example>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// تاريخ آخر تحديث
        /// </summary>
        /// <example>2024-01-15T10:30:00</example>
        public DateTime? UpdatedAt { get; set; }

      

    }
}