using System.ComponentModel.DataAnnotations;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Users.Contacts
{
    /// <summary>
    /// ▼━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▼
    /// ➕  نموذج إنشاء جهة اتصال (Create Contact DTO)
    /// 📌  الوظيفة: نقل بيانات إنشاء جهة اتصال من العميل إلى الخادم
    /// 📦  الاستخدام: ضمن CreateUserDto
    /// ▲━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━▲
    /// </summary>
    public class CreateContactDto
    {
        /// <summary>
        /// نوع جهة الاتصال (مطلوب)
        /// </summary>
        /// <example>1</example>
        [Required(ErrorMessage = "نوع جهة الاتصال مطلوب")]
        public ContactType ContactType { get; set; }

        /// <summary>
        /// قيمة جهة الاتصال (مطلوبة)
        /// </summary>
        /// <example>01001234567</example>
        [Required(ErrorMessage = "قيمة جهة الاتصال مطلوبة")]
        [MaxLength(200, ErrorMessage = "القيمة لا تتجاوز 200 حرف")]
        public string ContactValue { get; set; } = string.Empty;

        /// <summary>
        /// هل هي جهة الاتصال الأساسية؟
        /// </summary>
        /// <example>true</example>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// ملاحظات إضافية
        /// </summary>
        /// <example>رقم المنزل</example>
        [MaxLength(500, ErrorMessage = "الملاحظات لا تتجاوز 500 حرف")]
        public string? Notes { get; set; }
    }
}