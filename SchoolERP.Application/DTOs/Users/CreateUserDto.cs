using SchoolERP.Application.DTOs.Users.Contacts;
using SchoolERP.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SchoolERP.Application.DTOs.Users
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء مستخدم جديد (Create User DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء المستخدم من العميل إلى الخادم
    /// 📦  الاستخدام: في UsersController (POST endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// اسم المستخدم (مطلوب وفريد)
        /// </summary>
        /// <example>ahmed.hassan</example>
        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [MaxLength(50, ErrorMessage = "اسم المستخدم لا يتجاوز 50 حرف")]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// كلمة المرور (مطلوبة)
        /// </summary>
        /// <example>Password@123</example>
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور لا تقل عن 6 أحرف")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// تأكيد كلمة المرور (مطلوب)
        /// </summary>
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقين")]
        public string ConfirmPassword { get; set; } = string.Empty;

        /// <summary>
        /// الاسم الكامل (مطلوب)
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [MaxLength(100, ErrorMessage = "الاسم لا يتجاوز 100 حرف")]
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// الرقم القومي (مطلوب وفريد - 14 رقم)
        /// </summary>
        /// <example>12345678901234</example>
        [Required(ErrorMessage = "الرقم القومي مطلوب")]
        [MaxLength(14, ErrorMessage = "الرقم القومي 14 رقم")]
        [MinLength(14, ErrorMessage = "الرقم القومي 14 رقم")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "الرقم القومي يجب أن يكون 14 رقم")]
        public string NationalId { get; set; } = string.Empty;

        /// <summary>
        /// البريد الإلكتروني
        /// </summary>
        /// <example>ahmed@example.com</example>
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string? Email { get; set; }

        /// <summary>
        /// رقم الهاتف المحمول
        /// </summary>
        /// <example>01001234567</example>
        [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// تاريخ الميلاد (مطلوب)
        /// </summary>
        /// <example>2000-01-01</example>
        [Required(ErrorMessage = "تاريخ الميلاد مطلوب")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// العنوان
        /// </summary>
        /// <example>مصر الجديدة - القاهرة</example>
        [MaxLength(500, ErrorMessage = "العنوان لا يتجاوز 500 حرف")]
        public string? Address { get; set; }

        /// <summary>
        /// نوع المستخدم (مطلوب)
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "نوع المستخدم مطلوب")]
        public UserType UserType { get; set; }

        /// <summary>
        /// معرف المدرسة (مطلوب)
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "معرف المدرسة مطلوب")]
        public int SchoolId { get; set; }

        public string? Gender { get; set; } 

        /// <summary>
        /// صورة الملف الشخصي
        /// </summary>
        public IFormFile? ProfileImage { get; set; }

        /// <summary>
        /// صورة الملف الشخصي (رابط)
        /// </summary>
        public string? ProfilePicture { get; set; }  // ✅ إضافة

     

        // ✅ خصائص إضافية
        public int? ClassRoomId { get; set; }
        public string? ParentName { get; set; }
        public string? ParentPhone { get; set; }
        public string? Qualification { get; set; }
        public string? Specialization { get; set; }
        public string? JobTitle { get; set; }
        public int? GradeLevelId { get; set; }

        // ✅ قائمة جهات الاتصال
        public List<CreateContactDto> Contacts { get; set; } = new();
    }
}