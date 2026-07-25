using Microsoft.AspNetCore.Http;
using SchoolERP.Application.DTOs.UserContacts;
using SchoolERP.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SchoolERP.Application.DTOs.Users
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ✏️  نموذج تحديث بيانات المستخدم (Update User DTO)
    /// 📌  الوظيفة: نقل بيانات تحديث المستخدم من العميل إلى الخادم
    /// 📦  الاستخدام: في UsersController (PUT endpoint)
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class UpdateUserDto
    {
        /// <summary>
        /// الاسم الكامل
        /// </summary>
        /// <example>أحمد حسن محمد</example>
        [MaxLength(100, ErrorMessage = "الاسم لا يتجاوز 100 حرف")]
        public string? FullName { get; set; }

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
        /// العنوان
        /// </summary>
        /// <example>مصر الجديدة - القاهرة</example>
        [MaxLength(500, ErrorMessage = "العنوان لا يتجاوز 500 حرف")]
        public string? Address { get; set; }

        /// <summary>
        /// حالة المستخدم
        /// </summary>
        /// <example>1</example>
        public UserStatus? Status { get; set; }


        public string? Gender { get; set; }


        /// <summary>
        /// صورة الملف الشخصي
        /// </summary>
        /// <example>/uploads/profiles/ahmed.jpg</example>
        public string? ProfilePicture { get; set; }

        /// <summary>
        /// صورة الملف الشخصي (للرفع)
        /// </summary>
        public IFormFile? ProfileImage { get; set; } 

        /// <summary>
        /// الصورة الحالية (للعرض)
        /// </summary>
        public string? CurrentProfilePicture { get; set; } 

        /// <summary>
        /// حذف الصورة الحالية؟
        /// </summary>
        public bool RemoveImage { get; set; }


        // ✅ قائمة جهات الاتصال للتحديث
        public List<UpdateUserContactDto>? Contacts { get; set; }


    }
}